using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ProductsOperation.Validations;
using TsiErp.Business.Entities.Route.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
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

        public ProductsOperationsAppService(IStringLocalizer<ProductsOperationsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IRoutesAppService routesAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _RoutesAppService = routesAppService;
        }

        [ValidationAspect(typeof(CreateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> CreateAsync(CreateProductsOperationsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<ProductsOperations>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ProductsOperations).Insert(new CreateProductsOperationsDto
            {
                Name = input.Name,
                ProductID = input.ProductID.GetValueOrDefault(),
                TemplateOperationID = input.TemplateOperationID.GetValueOrDefault(),
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsActive = true,
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
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);

        }

        [CacheRemoveAspect("Get")]
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
                Tables.WorkOrders
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Id = id }, true, true, "");

                var productsOperations = queryFactory.Get<SelectProductsOperationsDto>(query);

                if (productsOperations.Id != Guid.Empty && productsOperations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ProductsOperations).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductsOperationLines).Delete(LoginedUserService.UserId).Where(new { ProductsOperationID = id }, false, false, "");

                    var lineQualityPlansDeleteQuery = queryFactory.Query().From(Tables.ProductOperationQualityPlans).Delete(LoginedUserService.UserId).Where(new { ProductsOperationID = id }, false, false, "");

                    var lineContractDeleteQuery = queryFactory.Query().From(Tables.ContractOfProductsOperations).Delete(LoginedUserService.UserId).Where(new { ProductsOperationID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql
                        + QueryConstants.QueryConstant + lineDeleteQuery.Sql
                        + QueryConstants.QueryConstant + lineQualityPlansDeleteQuery.Sql
                        + QueryConstants.QueryConstant + lineContractDeleteQuery.Sql
                        + " where " + lineDeleteQuery.WhereSentence;

                    var productsOperation = queryFactory.Update<SelectProductsOperationsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductsOperationLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
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
                        g => new { WorkCenterName = g.Name, WorkCenterCode = g.Code },
                        nameof(ProductsOperations.WorkCenterID),
                        nameof(StationGroups.Id), JoinType.Left
                    )
                    .Where(new { Id = id }, true, true, Tables.ProductsOperations);

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
                            .Where(new { ProductsOperationID = id }, false, false, Tables.ProductsOperationLines);

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
                        .Where(new { ProductsOperationID = id }, false, false, Tables.ContractOfProductsOperations);

            var contractOfProductionOperations = queryFactory.GetList<SelectContractOfProductsOperationsDto>(contractOfProductionOperationQuery).ToList();

            productsOperations.SelectContractOfProductsOperationsLines = contractOfProductionOperations;
            #endregion

            LogsAppService.InsertLogToDatabase(productsOperations, productsOperations, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperations);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsOperationsDto>>> GetListAsync(ListProductsOperationsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductsOperations)
                   .Select<ProductsOperations, OperationStockMovements>(
                null, 
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
                        g => new { WorkCenterName = g.Name, WorkCenterCode = g.Code },
                        nameof(ProductsOperations.WorkCenterID),
                        nameof(StationGroups.Id), JoinType.Left
                    )
                    .Where(null, true, true, Tables.ProductsOperations);

            var productsOperations = queryFactory.GetList<ListProductsOperationsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductsOperationsDto>>(productsOperations);

        }

        [ValidationAspect(typeof(UpdateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
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
                    .Where(new { Id = input.Id }, true, true, Tables.ProductsOperations);

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
                            .Where(new { ProductsOperationID = input.Id }, false, false, Tables.ProductsOperationLines);

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
                        .Where(new { ProductsOperationID = input.Id }, false, false, Tables.ContractOfProductsOperations);

            var contractOfProductionOperations = queryFactory.GetList<SelectContractOfProductsOperationsDto>(contractOfProductionOperationQuery).ToList();

            entity.SelectContractOfProductsOperationsLines = contractOfProductionOperations;
            #endregion

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.ProductsOperations)
                   .Select<ProductsOperations>(po => new { po.WorkCenterID, po.TemplateOperationID, po.ProductID, po.Name, po.IsActive, po.Id, po.DataOpenStatusUserId, po.DataOpenStatus, po.Code })
                   .Join<Products>
                    (
                        p => new { ProductCode = p.Code, ProductName = p.Name },
                        nameof(ProductsOperations.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, false, false, Tables.ProductsOperations);

            var list = queryFactory.GetList<ListProductsOperationsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

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
                IsActive = input.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, true, true, "");

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
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    var lineGetQuery = queryFactory.Query().From(Tables.ProductsOperationLines).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                    
                    }
                }

                if (item.Priority == 1)
                {
                    #region Rota Satır Update

                    var routeLine = (await _RoutesAppService.GetLinebyProductsOperationIDAsync(input.Id)).Data;

                    var route = (await _RoutesAppService.GetAsync(routeLine.RouteID)).Data;

                    int updatedLineIndex = route.SelectRouteLines.IndexOf(routeLine);

                    route.SelectRouteLines[updatedLineIndex].AdjustmentAndControlTime = (int)item.AdjustmentAndControlTime;
                    route.SelectRouteLines[updatedLineIndex].OperationTime = (int)item.OperationTime;

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
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    var lineGetQuery = queryFactory.Query().From(Tables.ContractOfProductsOperations).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            CurrentAccountCardID = item.CurrentAccountCardID
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }
            #endregion

            var productsOperation = queryFactory.Update<SelectProductsOperationsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Update, entity.Id);

           

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);

        }

        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Id = id }, true, true, "");

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
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
            }).Where(new { Id = id }, true, true, "");

            var productsOperationsDto = queryFactory.Update<SelectProductsOperationsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductsOperationsDto>(productsOperationsDto);


        }
    }
}
