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
using TsiErp.Business.Entities.Route.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Routes.Page;

namespace TsiErp.Business.Entities.Route.Services
{
    [ServiceRegistration(typeof(IRoutesAppService), DependencyInjectionType.Scoped)]
    public class RoutesAppService : ApplicationService<RoutesResource>, IRoutesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public RoutesAppService(IStringLocalizer<RoutesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Routes).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<Routes>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Routes).Insert(new CreateRoutesDto
            {
                Approval = input.Approval,
                ProductID = input.ProductID,
                ProductionStart = input.ProductionStart,
                TechnicalApproval = input.TechnicalApproval,
                Code = input.Code,
                CreationTime = DateTime.Now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsActive = true,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
            });

            foreach (var item in input.SelectRouteLines)
            {
                var queryLine = queryFactory.Query().From(Tables.RouteLines).Insert(new CreateRouteLinesDto
                {
                    ProductID = item.ProductID,
                    AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                    OperationTime = item.OperationTime,
                    Priority = item.Priority,
                    ProductionPoolDescription = item.ProductionPoolDescription,
                    ProductionPoolID = item.ProductionPoolID,
                    ProductsOperationID = item.ProductsOperationID,
                    RouteID = addedEntityId,
                    CreationTime = DateTime.Now,
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

            return new SuccessDataResult<SelectRoutesDto>(route);

        }

        [CacheRemoveAspect("Get")]
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
                var query = queryFactory.Query().From(Tables.Routes).Select("*").Where(new { Id = id }, true, true, "");

                var routes = queryFactory.Get<SelectRoutesDto>(query);

                if (routes.Id != Guid.Empty && routes != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Routes).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.RouteLines).Delete(LoginedUserService.UserId).Where(new { RouteID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var route = queryFactory.Update<SelectRoutesDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Routes, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectRoutesDto>(route);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.RouteLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
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
                   .Select<Routes>(r => new { r.TechnicalApproval, r.ProductionStart, r.ProductID, r.Name, r.IsActive, r.Id, r.DataOpenStatusUserId, r.DataOpenStatus, r.Code, r.Approval })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, true, true, Tables.Routes);

            var routes = queryFactory.Get<SelectRoutesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.RouteLines)
                   .Select<RouteLines>(rl => new { rl.RouteID, rl.ProductsOperationID, rl.ProductionPoolID, rl.ProductionPoolDescription, rl.ProductID, rl.Priority, rl.OperationTime, rl.LineNr, rl.Id, rl.DataOpenStatusUserId, rl.DataOpenStatus, rl.AdjustmentAndControlTime })
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
                    .Where(new { RouteID = id }, false, false, Tables.RouteLines);

            var routeLine = queryFactory.GetList<SelectRouteLinesDto>(queryLines).ToList();

            routes.SelectRouteLines = routeLine;

            LogsAppService.InsertLogToDatabase(routes, routes, LoginedUserService.UserId, Tables.Routes, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectRoutesDto>(routes);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.Routes)
                   .Select<Routes>(r => new { r.TechnicalApproval, r.ProductionStart, r.ProductID, r.Name, r.IsActive, r.Id, r.DataOpenStatusUserId, r.DataOpenStatus, r.Code, r.Approval })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(null, true, true, Tables.Routes);

            var routes = queryFactory.GetList<ListRoutesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListRoutesDto>>(routes);

        }

        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
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
                    .Where(new { Id = input.Id }, true, true, Tables.Routes);

            var entity = queryFactory.Get<SelectRoutesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.RouteLines)
                   .Select<RouteLines>(rl => new { rl.RouteID, rl.ProductsOperationID, rl.ProductionPoolID, rl.ProductionPoolDescription, rl.ProductID, rl.Priority, rl.OperationTime, rl.LineNr, rl.Id, rl.DataOpenStatusUserId, rl.DataOpenStatus, rl.AdjustmentAndControlTime })
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
                    .Where(new { RouteID = input.Id }, false, false, Tables.RouteLines);

            var routeLine = queryFactory.GetList<SelectRouteLinesDto>(queryLines).ToList();

            entity.SelectRouteLines = routeLine;

            #region Update Control
            var listQuery = queryFactory
                   .Query()
                    .From(Tables.Routes)
                   .Select<Routes>(r => new { r.TechnicalApproval, r.ProductionStart, r.ProductID, r.Name, r.IsActive, r.Id, r.DataOpenStatusUserId, r.DataOpenStatus, r.Code, r.Approval })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(Routes.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, false, false, Tables.Routes);

            var list = queryFactory.GetList<ListRoutesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.Routes).Update(new UpdateRoutesDto
            {
                Approval = input.Approval,
                ProductID = input.ProductID,
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
                IsActive = input.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, true, true, "");

            foreach (var item in input.SelectRouteLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.RouteLines).Insert(new CreateRouteLinesDto
                    {
                        ProductID = item.ProductID,
                        AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                        OperationTime = item.OperationTime,
                        Priority = item.Priority,
                        ProductionPoolDescription = item.ProductionPoolDescription,
                        ProductionPoolID = item.ProductionPoolID,
                        ProductsOperationID = item.ProductsOperationID,
                        RouteID = input.Id,
                        CreationTime = DateTime.Now,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.RouteLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectRouteLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.RouteLines).Update(new UpdateRouteLinesDto
                        {
                            ProductID = item.ProductID,
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
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var route = queryFactory.Update<SelectRoutesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Routes, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectRoutesDto>(route);

        }

        public async Task<IDataResult<List<ListProductsOperationsDto>>> GetProductsOperationAsync(Guid productId)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductsOperations)
                   .Select<ProductsOperations>(po => new { po.WorkCenterID, po.TemplateOperationID, po.ProductID, po.Name, po.IsActive, po.Id, po.DataOpenStatusUserId, po.DataOpenStatus, po.Code })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ProductsOperations.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productId }, true, true, Tables.ProductsOperations);

            var productsOperations = queryFactory.GetList<ListProductsOperationsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<List<ListProductsOperationsDto>>(productsOperations);


        }

        public async Task<IDataResult<SelectRoutesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Routes).Select("*").Where(new { Id = id }, true, true, "");

            var entity = queryFactory.Get<Routes>(entityQuery);

            var query = queryFactory.Query().From(Tables.Routes).Update(new UpdateRoutesDto
            {
                Approval = entity.Approval,
                ProductID = entity.ProductID,
                ProductionStart = entity.ProductionStart,
                TechnicalApproval = entity.TechnicalApproval,
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

            var routesDto = queryFactory.Update<SelectRoutesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectRoutesDto>(routesDto);


        }
    }
}
