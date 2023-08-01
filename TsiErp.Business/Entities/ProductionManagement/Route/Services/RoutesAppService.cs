using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Route.Validations;
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

        public RoutesAppService(IStringLocalizer<RoutesResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Routes).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<Routes>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
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

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Routes, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectRoutesDto>(route);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                    return new SuccessDataResult<SelectRoutesDto>(route);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.RouteLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var routeLines = queryFactory.Update<SelectRouteLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.RouteLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectRouteLinesDto>(routeLines);
                }
            }
        }

        public async Task<IDataResult<SelectRoutesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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

                return new SuccessDataResult<SelectRoutesDto>(routes);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                return new SuccessDataResult<IList<ListRoutesDto>>(routes);
            }
        }

        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> UpdateAsync(UpdateRoutesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
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
                    connection.Close();
                    connection.Dispose();
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

                return new SuccessDataResult<SelectRoutesDto>(route);
            }

        }

        public async Task<IDataResult<List<ListProductsOperationsDto>>> GetProductsOperationAsync(Guid productId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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

                return new SuccessDataResult<List<ListProductsOperationsDto>>(productsOperations);

            }

        }

        public async Task<IDataResult<SelectRoutesDto>> GetSelectListAsync(Guid productId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                        .Where(new { ProductID = productId }, true, true, Tables.Routes);

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
                        .Where(new { RouteID = routes.Id }, false, false, Tables.RouteLines);

                var routeLine = queryFactory.GetList<SelectRouteLinesDto>(queryLines).ToList();

                routes.SelectRouteLines = routeLine;

                LogsAppService.InsertLogToDatabase(routes, routes, LoginedUserService.UserId, Tables.Routes, LogType.Get, routes.Id);

                return new SuccessDataResult<SelectRoutesDto>(routes);
            }
        }

        public async Task<IDataResult<SelectRoutesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                    DeletionTime = entity.DeletionTime.Value,
                    Id = entity.Id,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Name = entity.Name,
                }).Where(new { Id = id }, true, true, "");

                var routesDto = queryFactory.Update<SelectRoutesDto>(query, "Id", true);
                return new SuccessDataResult<SelectRoutesDto>(routesDto);

            }
        }
    }
}
