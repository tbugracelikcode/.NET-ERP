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
using TsiErp.Business.Entities.ProductsOperation.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
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

        public ProductsOperationsAppService(IStringLocalizer<ProductsOperationsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> CreateAsync(CreateProductsOperationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<ProductsOperations>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ProductsOperations).Insert(new CreateProductsOperationsDto
                {
                    Name = input.Name,
                    ProductID = input.ProductID,
                    TemplateOperationID = input.TemplateOperationID,
                    WorkCenterID = Guid.Empty,
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
                        StationID = item.StationID,
                        ProductsOperationID = addedEntityId,
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

                var productsOperation = queryFactory.Insert<SelectProductsOperationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ProductsOperations).Select("*").Where(new { Id = id }, true, true, "");

                var productsOperations = queryFactory.Get<SelectProductsOperationsDto>(query);

                if (productsOperations.Id != Guid.Empty && productsOperations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ProductsOperations).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductsOperationLines).Delete(LoginedUserService.UserId).Where(new { ProductsOperationID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var productsOperation = queryFactory.Update<SelectProductsOperationsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Delete, id);
                    return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductsOperationLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var productsOperationLines = queryFactory.Update<SelectProductsOperationLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductsOperationLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectProductsOperationLinesDto>(productsOperationLines);
                }
            }
        }

        public async Task<IDataResult<SelectProductsOperationsDto>> GetAsync(Guid id)
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
                        .Join<TemplateOperations>
                        (
                            to => new { TemplateOperationID = to.Id, TemplateOperationCode = to.Code, TemplateOperationName = to.Name },
                            nameof(ProductsOperations.TemplateOperationID),
                            nameof(TemplateOperations.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, true, true, Tables.ProductsOperations);

                var productsOperations = queryFactory.Get<SelectProductsOperationsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ProductsOperationLines)
                       .Select<ProductsOperationLines>(pol => new { pol.StationID, pol.ProductsOperationID, pol.ProcessQuantity, pol.Priority, pol.OperationTime, pol.LineNr, pol.Id, pol.DataOpenStatusUserId, pol.DataOpenStatus, pol.Alternative, pol.AdjustmentAndControlTime })
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

                LogsAppService.InsertLogToDatabase(productsOperations, productsOperations, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Get, id);

                return new SuccessDataResult<SelectProductsOperationsDto>(productsOperations);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsOperationsDto>>> GetListAsync(ListProductsOperationsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.ProductsOperations)
                       .Select<ProductsOperations>(po => new { po.WorkCenterID, po.ProductID, po.TemplateOperationID,  po.Name, po.IsActive, po.Id, po.DataOpenStatusUserId, po.DataOpenStatus, po.Code })
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductsOperations.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(null, true, true, Tables.ProductsOperations);

                var productsOperations = queryFactory.GetList<ListProductsOperationsDto>(query).ToList();
                return new SuccessDataResult<IList<ListProductsOperationsDto>>(productsOperations);
            }
        }

        [ValidationAspect(typeof(UpdateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateAsync(UpdateProductsOperationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
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
                        .Where(new { Id = input.Id }, true, true, Tables.ProductsOperations);

                var entity = queryFactory.Get<SelectProductsOperationsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ProductsOperationLines)
                       .Select<ProductsOperationLines>(pol => new { pol.StationID, pol.ProductsOperationID, pol.ProcessQuantity, pol.Priority, pol.OperationTime, pol.LineNr, pol.Id, pol.DataOpenStatusUserId, pol.DataOpenStatus, pol.Alternative, pol.AdjustmentAndControlTime })
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
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.ProductsOperations).Update(new UpdateProductsOperationsDto
                {
                    ProductID = input.ProductID,
                    TemplateOperationID = input.TemplateOperationID,
                    WorkCenterID = input.WorkCenterID,
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
                            StationID = item.StationID,
                            ProductsOperationID = input.Id,
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
                                StationID = item.StationID,
                                ProductsOperationID = input.Id,
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

                var productsOperation = queryFactory.Update<SelectProductsOperationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductsOperations, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectProductsOperationsDto>(productsOperation);
            }
        }

        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                    DeletionTime = entity.DeletionTime.Value,
                    Id = entity.Id,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Name = entity.Name,
                }).Where(new { Id = id }, true, true, "");

                var productsOperationsDto = queryFactory.Update<SelectProductsOperationsDto>(query, "Id", true);
                return new SuccessDataResult<SelectProductsOperationsDto>(productsOperationsDto);

            }
        }
    }
}
