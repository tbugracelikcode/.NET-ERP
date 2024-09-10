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
using TsiErp.Business.Entities.Route.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Routes.Page;

namespace TsiErp.Business.Entities.Route.Services
{
    [ServiceRegistration(typeof(IRoutesAppService), DependencyInjectionType.Scoped)]
    public class RoutesAppService : ApplicationService<RoutesResource>, IRoutesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public RoutesAppService(IStringLocalizer<RoutesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Routes).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<Routes>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Routes).Insert(new CreateRoutesDto
            {
                Approval = input.Approval,
                StationGroupID = input.StationGroupID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionStart = input.ProductionStart,
                TechnicalApproval = input.TechnicalApproval,
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
                LastModifierId = Guid.Empty,
                Name = input.Name,
            });

            foreach (var item in input.SelectRouteLines)
            {
                var queryLine = queryFactory.Query().From(Tables.RouteLines).Insert(new CreateRouteLinesDto
                {
                    ProductID = item.ProductID.GetValueOrDefault(),
                    AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                    OperationTime = item.OperationTime,
                    Priority = item.Priority,
                    ProductionPoolDescription = item.ProductionPoolDescription,
                    ProductionPoolID = item.ProductionPoolID,
                    ProductsOperationID = item.ProductsOperationID,
                    RouteID = addedEntityId,
                    CreationTime =now,
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

            var route = queryFactory.Insert<SelectRoutesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("RoutesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Routes, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["RoutesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectRoutesDto>(route);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();


            DeleteControl.ControlList.Add("RouteID", new List<string>
            {
                Tables.WorkOrders,
                Tables.ProductionOrders
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.Routes).Select("*").Where(new { Id = id }, "");

                var routes = queryFactory.Get<SelectRoutesDto>(query);

                if (routes.Id != Guid.Empty && routes != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Routes).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.RouteLines).Delete(LoginedUserService.UserId).Where(new { RouteID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var route = queryFactory.Update<SelectRoutesDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Routes, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["BranchesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                        RecordNumber = entity.Code,
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
                                    RecordNumber = entity.Code,
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
                    return new SuccessDataResult<SelectRoutesDto>(route);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.RouteLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                    var routeLines = queryFactory.Update<SelectRouteLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.RouteLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectRouteLinesDto>(routeLines);
                }
            }
        }

        public async Task<IDataResult<SelectRoutesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Routes)
                   .Select<Routes>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<StationGroups>
                    (
                        p => new { StationGroupID = p.Id, ProductionStart = p.Name },
                        nameof(Routes.StationGroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.Routes);

            var routes = queryFactory.Get<SelectRoutesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.RouteLines)
                   .Select<RouteLines>(null)
                   .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(RouteLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        po => new { ProductsOperationID = po.Id, OperationName = po.Name, OperationCode = po.Code },
                        nameof(RouteLines.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { RouteID = id }, Tables.RouteLines);

            var routeLine = queryFactory.GetList<SelectRouteLinesDto>(queryLines).ToList();

            routes.SelectRouteLines = routeLine;

            LogsAppService.InsertLogToDatabase(routes, routes, LoginedUserService.UserId, Tables.Routes, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectRoutesDto>(routes);

        }

        public async Task<IDataResult<SelectRoutesDto>> GetbyProductIDAsync(Guid productId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Routes)
                   .Select<Routes>(s => new {s.Approval,s.Id,s.Code,s.Name,s.TechnicalApproval})
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<StationGroups>
                    (
                        p => new { StationGroupID = p.Id, ProductionStart = p.Name },
                        nameof(Routes.StationGroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productId, Approval=true, TechnicalApproval=true }, Tables.Routes);

            var routes = queryFactory.Get<SelectRoutesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.RouteLines)
                   .Select<RouteLines>(null)
                   .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(RouteLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        po => new { ProductsOperationID = po.Id, OperationName = po.Name, OperationCode = po.Code },
                        nameof(RouteLines.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { RouteID = routes.Id }, Tables.RouteLines);

            var routeLine = queryFactory.GetList<SelectRouteLinesDto>(queryLines).ToList();

            routes.SelectRouteLines = routeLine;

            LogsAppService.InsertLogToDatabase(routes, routes, LoginedUserService.UserId, Tables.Routes, LogType.Get, routes.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectRoutesDto>(routes);

        }

        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.Routes)
                   .Select<Routes>(s => new { s.Code, s.Name, s.Approval, s.TechnicalApproval, s.Id })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                       .Join<StationGroups>
                    (
                        p => new { StationGroupID = p.Id, ProductionStart = p.Name },
                        nameof(Routes.StationGroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.Routes);

            var routes = queryFactory.GetList<ListRoutesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListRoutesDto>>(routes);

        }

        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        public async Task<IDataResult<SelectRoutesDto>> UpdateAsync(UpdateRoutesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.Routes)
                   .Select<Routes>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                       .Join<StationGroups>
                    (
                        p => new { StationGroupID = p.Id, ProductionStart = p.Name },
                        nameof(Routes.StationGroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.Routes);

            var entity = queryFactory.Get<SelectRoutesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.RouteLines)
                   .Select<RouteLines>(null)
                   .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(RouteLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        po => new { ProductsOperationID = po.Id, OperationName = po.Name, OperationCode = po.Code },
                        nameof(RouteLines.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { RouteID = input.Id }, Tables.RouteLines);

            var routeLine = queryFactory.GetList<SelectRouteLinesDto>(queryLines).ToList();

            entity.SelectRouteLines = routeLine;

            #region Update Control
            var listQuery = queryFactory
                   .Query()
                    .From(Tables.Routes)
                   .Select<Routes>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                       .Join<StationGroups>
                    (
                        p => new { StationGroupID = p.Id, ProductionStart = p.Name },
                        nameof(Routes.StationGroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.Routes);

            var list = queryFactory.GetList<ListRoutesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Routes).Update(new UpdateRoutesDto
            {
                Approval = input.Approval,
                ProductID = input.ProductID.GetValueOrDefault(),
                StationGroupID = input.StationGroupID.GetValueOrDefault(),
                ProductionStart = input.ProductionStart,
                TechnicalApproval = input.TechnicalApproval,
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

            foreach (var item in input.SelectRouteLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.RouteLines).Insert(new CreateRouteLinesDto
                    {
                        ProductID = item.ProductID.GetValueOrDefault(),
                        AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                        OperationTime = item.OperationTime,
                        Priority = item.Priority,
                        ProductionPoolDescription = item.ProductionPoolDescription,
                        ProductionPoolID = item.ProductionPoolID,
                        ProductsOperationID = item.ProductsOperationID,
                        RouteID = input.Id,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.RouteLines).Select("*").Where(new { Id = item.Id },"");

                    var line = queryFactory.Get<SelectRouteLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.RouteLines).Update(new UpdateRouteLinesDto
                        {
                            ProductID = item.ProductID.GetValueOrDefault(),
                            AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                            OperationTime = item.OperationTime,
                            Priority = item.Priority,
                            ProductionPoolDescription = item.ProductionPoolDescription,
                            ProductionPoolID = item.ProductionPoolID,
                            ProductsOperationID = item.ProductsOperationID,
                            RouteID = input.Id,
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
            }

            var route = queryFactory.Update<SelectRoutesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Routes, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["RoutesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectRoutesDto>(route);

        }

        public async Task<IDataResult<List<ListProductsOperationsDto>>> GetProductsOperationAsync(Guid productId)
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
                    .Where(new { ProductID = productId }, Tables.ProductsOperations);

            var productsOperations = queryFactory.GetList<ListProductsOperationsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<List<ListProductsOperationsDto>>(productsOperations);


        }

        public async Task<IDataResult<SelectRoutesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Routes).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Routes>(entityQuery);

            var query = queryFactory.Query().From(Tables.Routes).Update(new UpdateRoutesDto
            {
                Approval = entity.Approval,
                ProductID = entity.ProductID,
                ProductionStart = entity.ProductionStart,
                StationGroupID = entity.StationGroupID,
                TechnicalApproval = entity.TechnicalApproval,
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

            var routesDto = queryFactory.Update<SelectRoutesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectRoutesDto>(routesDto);


        }

        public async Task<IDataResult<SelectRouteLinesDto>> GetLinebyProductsOperationIDAsync(Guid productsOperationID)
        {
            var queryLines = queryFactory
                  .Query()
                  .From(Tables.RouteLines)
                  .Select<RouteLines>(null)
                  .Join<Products>
                   (
                       pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                       nameof(RouteLines.ProductID),
                       nameof(Products.Id),
                       JoinType.Left
                   )
                  .Join<ProductsOperations>
                   (
                       po => new { ProductsOperationID = po.Id, OperationName = po.Name, OperationCode = po.Code },
                       nameof(RouteLines.ProductsOperationID),
                       nameof(ProductsOperations.Id),
                       JoinType.Left
                   )
                   .Where(new { ProductsOperationID = productsOperationID }, Tables.RouteLines);

            var routeLine = queryFactory.Get<SelectRouteLinesDto>(queryLines);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectRouteLinesDto>(routeLine);
        }
    }
}
