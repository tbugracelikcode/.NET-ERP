using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ProductsOperation.Validations;
using TsiErp.Business.Entities.Route.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductsOperations.Page;

namespace TsiErp.Business.Entities.ProductsOperation.Services
{
    [ServiceRegistration(typeof(IProductsOperationsAppService), DependencyInjectionType.Scoped)]

    public class ProductsOperationsAppService : ApplicationService<ProductsOperationsResource>, IProductsOperationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IRoutesAppService _RoutesAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ProductsOperationsAppService(IStringLocalizer<ProductsOperationsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IRoutesAppService routesAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _RoutesAppService = routesAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateProductsOperationsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectProductsOperationsDto>> CreateAsync(CreateProductsOperationsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductsOperations).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<ProductsOperations>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductsOperations).Insert(new CreateProductsOperationsDto
            {
                Name = input.Name,
                ProductID = input.ProductID.GetValueOrDefault(),
                TemplateOperationID = input.TemplateOperationID.GetValueOrDefault(),
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty
            });

            foreach (var item in input.SelectProductsOperationLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ProductsOperationLines).Insert(new CreateProductsOperationLinesDto
                {
                    OperationTime = item.OperationTime,
                    AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                    Alternative = item.Alternative,
                    Priority = item.Priority,
                    ProcessQuantity = item.ProcessQuantity,
                    StationID = item.StationID.GetValueOrDefault(),
                    ProductsOperationID = addedEntityId,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }


            foreach (var item in input.SelectContractOfProductsOperationsLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ContractOfProductsOperations).Insert(new CreateContractOfProductsOperationsDto
                {
                    ProductsOperationID = addedEntityId,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    CurrentAccountCardID = item.CurrentAccountCardID
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var productsOperation = queryFactory.Insert<SelectProductsOperationsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProdOperationsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdOperationsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("OperationID", new List<string>
            {
                Tables.ContractTrackingFicheLines,
                Tables.OperationalSPCLines,
                Tables.OperationStockMovements,
                Tables.OperationUnsuitabilityReports,
                Tables.PFMEAs
            });

            DeleteControl.ControlList.Add("ProductsOperationID", new List<string>
            {
                Tables.OperationalQualityPlans,
                Tables.RouteLines,
                Tables.WorkOrders,
                Tables.ContractOfProductsOperations
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Id = id }, "");

                var productsOperations = queryFactory.Get<SelectProductsOperationsDto>(query);

                if (productsOperations.Id != Guid.Empty && productsOperations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ProductsOperations).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductsOperationLines).Delete(LoginedUserService.UserId).Where(new { ProductsOperationID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql
                        + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var productsOperation = queryFactory.Update<SelectProductsOperationsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Delete, id);

                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdOperationsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains(","))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split(',');

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = notTemplate.ContextMenuName_,
                                        IsViewed = false,
                                        Message_ = notTemplate.Message_,
                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = productsOperations.Code,
                                        NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                        UserId = new Guid(user),
                                        ViewDate = null,
                                    };

                                    await _NotificationsAppService.CreateAsync(createInput);
                                }
                            }
                            else
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                    Message_ = notTemplate.Message_,
                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = productsOperations.Code,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductsOperationLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                    var productsOperationLines = queryFactory.Update<SelectProductsOperationLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductsOperationLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectProductsOperationLinesDto>(productsOperationLines);
                }
            }
        }

        public async Task<IDataResult<SelectProductsOperationsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductsOperations)
                   .Select<ProductsOperations>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ProductsOperations.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<TemplateOperations>
                    (
                        to => new { TemplateOperationID = to.Id, TemplateOperationCode = to.Code, TemplateOperationName = to.Name },
                        nameof(ProductsOperations.TemplateOperationID),
                        nameof(TemplateOperations.Id),
                        JoinType.Left
                    )

                    .Join<StationGroups>
                    (
                        g => new { WorkCenterName = g.Name, WorkCenterCode = g.Code, WorkCenterID = g.Id},
                        nameof(ProductsOperations.WorkCenterID),
                        nameof(StationGroups.Id), JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.ProductsOperations);

            var productsOperations = queryFactory.Get<SelectProductsOperationsDto>(query);

            #region Product Operation Lines
            var queryLines = queryFactory
                           .Query()
                           .From(Tables.ProductsOperationLines)
                           .Select<ProductsOperationLines>(null)
                           .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                nameof(ProductsOperationLines.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Where(new { ProductsOperationID = id }, Tables.ProductsOperationLines);

            var productsOperationLine = queryFactory.GetList<SelectProductsOperationLinesDto>(queryLines).ToList();

            productsOperations.SelectProductsOperationLines = productsOperationLine;
            #endregion

            #region Contract Of Production Operations
            var contractOfProductionOperationQuery = queryFactory
                        .Query()
                        .From(Tables.ContractOfProductsOperations)
                        .Select<ContractOfProductsOperations>(null)
                        .Join<CurrentAccountCards>
                        (
                            s => new { CurrentAccountCardID = s.Id, CurrentAccountCardName = s.Name },
                            nameof(ContractOfProductsOperations.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductsOperationID = id }, Tables.ContractOfProductsOperations);

            var contractOfProductionOperations = queryFactory.GetList<SelectContractOfProductsOperationsDto>(contractOfProductionOperationQuery).ToList();

            productsOperations.SelectContractOfProductsOperationsLines = contractOfProductionOperations;
            #endregion

            LogsAppService.InsertLogToDatabase(productsOperations, productsOperations, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperations);

        }

        public async Task<IDataResult<IList<ListProductsOperationsDto>>> GetListAsync(ListProductsOperationsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductsOperations)
                   .Select<ProductsOperations, OperationStockMovements>(
                (s => new { s.Code, s.Name, s.Id }),
                t => t.TotalAmount, Tables.OperationStockMovements, true, nameof(OperationStockMovements.OperationID) + "=" + Tables.ProductsOperations + "." + nameof(ProductsOperations.Id))
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ProductsOperations.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<StationGroups>
                    (
                        g => new { WorkCenterName = g.Name, WorkCenterCode = g.Code, WorkCenterID = g.Id },
                        nameof(ProductsOperations.WorkCenterID),
                        nameof(StationGroups.Id), JoinType.Left
                    )
                    .Where(null, Tables.ProductsOperations);

            var productsOperations = queryFactory.GetList<ListProductsOperationsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductsOperationsDto>>(productsOperations);

        }

        [ValidationAspect(typeof(UpdateProductsOperationsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateAsync(UpdateProductsOperationsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                    .From(Tables.ProductsOperations)
                   .Select<ProductsOperations>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ProductsOperations.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.ProductsOperations);

            var entity = queryFactory.Get<SelectProductsOperationsDto>(entityQuery);

            #region Product Operation Lines
            var queryLines = queryFactory
                           .Query()
                           .From(Tables.ProductsOperationLines)
                           .Select<ProductsOperationLines>(null)
                           .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                nameof(ProductsOperationLines.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Where(new { ProductsOperationID = input.Id }, Tables.ProductsOperationLines);

            var productsOperationLine = queryFactory.GetList<SelectProductsOperationLinesDto>(queryLines).ToList();

            entity.SelectProductsOperationLines = productsOperationLine;
            #endregion

            #region Contract Of Production Operations
            var contractOfProductionOperationQuery = queryFactory
                        .Query()
                        .From(Tables.ContractOfProductsOperations)
                        .Select<ContractOfProductsOperations>(null)
                        .Join<CurrentAccountCards>
                        (
                            s => new { CurrentAccountCardID = s.Id, CurrentAccountCardName = s.Name },
                            nameof(ContractOfProductsOperations.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductsOperationID = input.Id }, Tables.ContractOfProductsOperations);

            var contractOfProductionOperations = queryFactory.GetList<SelectContractOfProductsOperationsDto>(contractOfProductionOperationQuery).ToList();

            entity.SelectContractOfProductsOperationsLines = contractOfProductionOperations;
            #endregion

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.ProductsOperations)
                   .Select<ProductsOperations>(po => new { po.WorkCenterID, po.TemplateOperationID, po.ProductID, po.Name, po.Id, po.DataOpenStatusUserId, po.DataOpenStatus, po.Code })
                   .Join<Products>
                    (
                        p => new { ProductCode = p.Code, ProductName = p.Name },
                        nameof(ProductsOperations.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.ProductsOperations);

            var list = queryFactory.GetList<ListProductsOperationsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductsOperations).Update(new UpdateProductsOperationsDto
            {
                ProductID = input.ProductID.GetValueOrDefault(),
                TemplateOperationID = input.TemplateOperationID.GetValueOrDefault(),
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, "");

            #region Product Operation Lines
            foreach (var item in input.SelectProductsOperationLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductsOperationLines).Insert(new CreateProductsOperationLinesDto
                    {
                        AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                        Alternative = item.Alternative,
                        OperationTime = item.OperationTime,
                        Priority = item.Priority,
                        ProcessQuantity = item.ProcessQuantity,
                        StationID = item.StationID.GetValueOrDefault(),
                        ProductsOperationID = input.Id,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;




                }

                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ProductsOperationLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectProductsOperationLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ProductsOperationLines).Update(new UpdateProductsOperationLinesDto
                        {
                            AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                            Alternative = item.Alternative,
                            OperationTime = item.OperationTime,
                            Priority = item.Priority,
                            ProcessQuantity = item.ProcessQuantity,
                            StationID = item.StationID.GetValueOrDefault(),
                            ProductsOperationID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;


                    }
                }

                if (item.Priority == 1)
                {
                    #region Rota Satır Update

                    var routeLine = (await _RoutesAppService.GetLinebyProductsOperationIDAsync(input.Id)).Data;

                    var route = (await _RoutesAppService.GetAsync(routeLine.RouteID)).Data;

                    foreach (var line in route.SelectRouteLines)
                    {
                        if (line.Id == routeLine.Id)
                        {
                            line.AdjustmentAndControlTime = (int)item.AdjustmentAndControlTime;
                            line.OperationTime = (int)item.OperationTime;
                        }
                    }

                    //int updatedLineIndex = route.SelectRouteLines.IndexOf(routeLine);

                    //route.SelectRouteLines[updatedLineIndex].AdjustmentAndControlTime = (int)item.AdjustmentAndControlTime;
                    //route.SelectRouteLines[updatedLineIndex].OperationTime = (int)item.OperationTime;

                    var updatedEntity = ObjectMapper.Map<SelectRoutesDto, UpdateRoutesDto>(route);

                    await _RoutesAppService.UpdateAsync(updatedEntity);

                    #endregion
                }
            }
            #endregion

            #region Contract Of Production Operations
            foreach (var item in input.SelectContractOfProductsOperationsLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ContractOfProductsOperations).Insert(new CreateContractOfProductsOperationsDto
                    {
                        ProductsOperationID = input.Id,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        CurrentAccountCardID = item.CurrentAccountCardID
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ContractOfProductsOperations).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectContractOfProductsOperationsDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ContractOfProductsOperations).Update(new UpdateContractOfProductsOperationsDto
                        {
                            ProductsOperationID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            CurrentAccountCardID = item.CurrentAccountCardID
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }
            #endregion

            var productsOperation = queryFactory.Update<SelectProductsOperationsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdOperationsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);

        }

        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ProductsOperations>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductsOperations).Update(new UpdateProductsOperationsDto
            {
                ProductID = entity.ProductID,
                TemplateOperationID = entity.TemplateOperationID,
                WorkCenterID = entity.WorkCenterID,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var productsOperationsDto = queryFactory.Update<SelectProductsOperationsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperationsDto);


        }
    }
}
