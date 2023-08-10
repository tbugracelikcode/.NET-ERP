using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.BillsofMaterial.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    [ServiceRegistration(typeof(IBillsofMaterialsAppService), DependencyInjectionType.Scoped)]
    public class BillsofMaterialsAppService : ApplicationService<BillsofMaterialsResource>, IBillsofMaterialsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public BillsofMaterialsAppService(IStringLocalizer<BillsofMaterialsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> CreateAsync(CreateBillsofMaterialsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<BillsofMaterials>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.BillsofMaterials).Insert(new CreateBillsofMaterialsDto
                {
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
                    FinishedProductID = input.FinishedProductID,
                    _Description = input._Description
                });

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Insert(new CreateBillsofMaterialLinesDto
                    {
                        BoMID = addedEntityId,
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        FinishedProductID = item.FinishedProductID,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        MaterialType = item.MaterialType,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        Size = item.Size,
                        UnitSetID = item.UnitSetID,
                        _Description = item._Description
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var billOfMaterial = queryFactory.Insert<SelectBillsofMaterialsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(new { Id = id }, true, true, "");

                var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

                if (billsOfMaterials.Id!=Guid.Empty && billsOfMaterials != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.BillsofMaterials).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.BillsofMaterialLines).Delete(LoginedUserService.UserId).Where(new { BomID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var billOfMaterial = queryFactory.Update<SelectBillsofMaterialsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Delete, id);
                    return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var billOfMaterialLines = queryFactory.Update<SelectBillsofMaterialLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.BillsofMaterialLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectBillsofMaterialLinesDto>(billOfMaterialLines);
                }
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterials)
                       .Select<BillsofMaterials>(b => new { b.Id, b.Code, b.Name, b._Description, b.IsActive })
                       .Join<Products>
                        (
                            pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id },
                            nameof(BillsofMaterials.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, true, true, Tables.BillsofMaterials);

                var billsOfMaterials = queryFactory.Get<SelectBillsofMaterialsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterialLines)
                       .Select<BillsofMaterialLines>(b => new { b.Id, b.BoMID, b.FinishedProductID, b.MaterialType, b.ProductID, b.UnitSetID, b.Quantity, b._Description, b.LineNr, b.Size, b.CreatorId, b.CreationTime, b.LastModifierId, b.LastModificationTime, b.DeleterId, b.DeletionTime, b.IsDeleted, b.DataOpenStatus, b.DataOpenStatusUserId })
                       .Join<Products>
                        (
                            p => new { FinishedProductCode = p.Code },
                            nameof(BillsofMaterialLines.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(BillsofMaterialLines.ProductID),
                            nameof(Products.Id),
                            "ProductLine",
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(BillsofMaterialLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { BoMID = id }, false, false, Tables.BillsofMaterialLines);

                var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

                billsOfMaterials.SelectBillsofMaterialLines = billsOfMaterialLine;

                LogsAppService.InsertLogToDatabase(billsOfMaterials, billsOfMaterials, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Get, id);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(billsOfMaterials);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBillsofMaterialsDto>>> GetListAsync(ListBillsofMaterialsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterials)
                       .Select<BillsofMaterials>(b => new { b.Id, b.Code, b.Name, b._Description, b.IsActive })
                       .Join<Products>
                        (
                            pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id },
                            nameof(BillsofMaterials.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(null, true, true, Tables.BillsofMaterials);

                var billsOfMaterials = queryFactory.GetList<ListBillsofMaterialsDto>(query).ToList();
                return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(billsOfMaterials);
            }
        }

        [ValidationAspect(typeof(UpdateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateAsync(UpdateBillsofMaterialsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterials)
                       .Select("*")
                       .Join<Products>
                        (
                            pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id },
                            nameof(BillsofMaterials.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, true, true, Tables.BillsofMaterials);

                var entity = queryFactory.Get<SelectBillsofMaterialsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.BillsofMaterialLines)
                       .Select<BillsofMaterialLines>(b => new { b.BoMID, b.FinishedProductID, b.MaterialType, b.Quantity, b._Description, b.LineNr, b.Size })
                       .Join<Products>
                        (
                            p => new { FinishedProductCode = p.Code },
                            nameof(BillsofMaterialLines.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(BillsofMaterialLines.ProductID),
                            nameof(Products.Id),
                            "ProductLine",
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(BillsofMaterialLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { BoMID = input.Id }, false, false, Tables.BillsofMaterialLines);

                var billsOfMaterialLine = queryFactory.GetList<SelectBillsofMaterialLinesDto>(queryLines).ToList();

                entity.SelectBillsofMaterialLines = billsOfMaterialLine;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.BillsofMaterials)
                               .Select<BillsofMaterials>(b => new { b.Id, b.Code, b.Name, b._Description, b.IsActive })
                               .Join<Products>
                                (
                                    pr => new { FinishedProductCode = pr.Code, FinishedProducName = pr.Name, FinishedProductID = pr.Id },
                                    nameof(BillsofMaterials.FinishedProductID),
                                    nameof(Products.Id),
                                    JoinType.Left
                                )
                                .Where(new { Code = input.Code }, false, false, Tables.BillsofMaterials);

                var list = queryFactory.GetList<ListBillsofMaterialsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.BillsofMaterials).Update(new UpdateBillsofMaterialsDto
                {
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
                    FinishedProductID = input.FinishedProductID,
                    _Description = input._Description
                }).Where(new { Id = input.Id }, true, true, "");

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Insert(new CreateBillsofMaterialLinesDto
                        {
                            BoMID = input.Id,
                            CreationTime = DateTime.Now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            FinishedProductID = item.FinishedProductID,
                            Id = GuidGenerator.CreateGuid(),
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            LineNr = item.LineNr,
                            MaterialType = item.MaterialType,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            Size = item.Size,
                            UnitSetID = item.UnitSetID,
                            _Description = item._Description
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.BillsofMaterialLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectBillsofMaterialLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.BillsofMaterialLines).Update(new UpdateBillsofMaterialLinesDto
                            {
                                BoMID = input.Id,
                                CreationTime = line.CreationTime,
                                CreatorId = line.CreatorId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = line.DeleterId.GetValueOrDefault(),
                                DeletionTime = line.DeletionTime.GetValueOrDefault(),
                                FinishedProductID = item.FinishedProductID,
                                Id = item.Id,
                                IsDeleted = item.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = item.LineNr,
                                MaterialType = item.MaterialType,
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                                Size = item.Size,
                                UnitSetID = item.UnitSetID,
                                _Description = item._Description
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var billOfMaterial = queryFactory.Update<SelectBillsofMaterialsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.BillsofMaterials, LogType.Update, billOfMaterial.Id);

                return new SuccessDataResult<SelectBillsofMaterialsDto>(billOfMaterial);
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<BillsofMaterials>(entityQuery);

                var query = queryFactory.Query().From(Tables.BillsofMaterials).Update(new UpdateBillsofMaterialsDto
                {
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
                    FinishedProductID = entity.FinishedProductID,
                    _Description = entity._Description
                }).Where(new { Id = id }, true, true, "");

                var billsofMaterialsDto = queryFactory.Update<SelectBillsofMaterialsDto>(query, "Id", true);
                return new SuccessDataResult<SelectBillsofMaterialsDto>(billsofMaterialsDto);

            }
        }
    }
}
