using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GrandTotalStockMovement.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.GrandTotalStockMovements.Page;

namespace TsiErp.Business.Entities.GrandTotalStockMovement.Services
{
    [ServiceRegistration(typeof(IGrandTotalStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class GrandTotalStockMovementsAppService : ApplicationService<GrandTotalStockMovementsResource>, IGrandTotalStockMovementsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public GrandTotalStockMovementsAppService(IStringLocalizer<GrandTotalStockMovementsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> CreateAsync(CreateGrandTotalStockMovementsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
                {
                    Amount = input.Amount,
                    ProductID = input.ProductID,
                    TotalConsumption = input.TotalConsumption,
                    TotalGoodsIssue = input.TotalGoodsIssue,
                    TotalGoodsReceipt = input.TotalGoodsReceipt,
                    TotalProduction = input.TotalProduction,
                    TotalPurchaseOrder = input.TotalPurchaseOrder,
                    TotalPurchaseRequest = input.TotalPurchaseRequest,
                    TotalSalesOrder = input.TotalSalesOrder,
                    TotalReserved = input.TotalReserved,
                    TotalSalesProposition = input.TotalSalesProposition,
                    TotalWastage = input.TotalWastage,
                    WarehouseID = input.WarehouseID,
                    BranchID = input.BranchID,
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
                });

                var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Insert, grandTotalStockMovements.Id);

                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovements);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Delete, id);

                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovements);
            }

        }


        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.GrandTotalStockMovements).Select<GrandTotalStockMovements>(gt => new {gt.WarehouseID,gt.TotalWastage,gt.TotalSalesProposition,gt.TotalSalesOrder,gt.TotalReserved,gt.TotalPurchaseRequest,gt.TotalPurchaseOrder,gt.TotalProduction,gt.TotalGoodsReceipt,gt.TotalGoodsIssue,gt.TotalConsumption,gt.ProductID,gt.Id,gt.DataOpenStatusUserId,gt.DataOpenStatus,gt.BranchID,gt.Amount })
                            .Join<Branches>
                            (
                                b => new { BranchCode = b.Code, BranchID = b.Id },
                                nameof(GrandTotalStockMovements.BranchID),
                                nameof(Branches.Id),
                                JoinType.Left
                            )
                            .Join<Products>
                            (
                                p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                                nameof(GrandTotalStockMovements.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<Warehouses>
                            (
                                w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                                nameof(GrandTotalStockMovements.WarehouseID),
                                nameof(Warehouses.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.GrandTotalStockMovements);

                var grandTotalStockMovement = queryFactory.Get<SelectGrandTotalStockMovementsDto>(query);

                LogsAppService.InsertLogToDatabase(grandTotalStockMovement, grandTotalStockMovement, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Get, id);

                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovement);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListGrandTotalStockMovementsDto>>> GetListAsync(ListGrandTotalStockMovementsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.GrandTotalStockMovements).Select<GrandTotalStockMovements>(gt => new { gt.WarehouseID, gt.TotalWastage, gt.TotalSalesProposition, gt.TotalSalesOrder, gt.TotalReserved, gt.TotalPurchaseRequest, gt.TotalPurchaseOrder, gt.TotalProduction, gt.TotalGoodsReceipt, gt.TotalGoodsIssue, gt.TotalConsumption, gt.ProductID, gt.Id, gt.DataOpenStatusUserId, gt.DataOpenStatus, gt.BranchID, gt.Amount })
                            .Join<Branches>
                            (
                                b => new { BranchCode = b.Code},
                                nameof(GrandTotalStockMovements.BranchID),
                                nameof(Branches.Id),
                                JoinType.Left
                            )
                            .Join<Products>
                            (
                                p => new {  ProductCode = p.Code, ProductName = p.Name },
                                nameof(GrandTotalStockMovements.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<Warehouses>
                            (
                                w => new { WarehouseCode = w.Code },
                                nameof(GrandTotalStockMovements.WarehouseID),
                                nameof(Warehouses.Id),
                                JoinType.Left
                            ).Where(null, true, true, Tables.Periods);

                var grandTotalStockMovements = queryFactory.GetList<ListGrandTotalStockMovementsDto>(query).ToList();

                return new SuccessDataResult<IList<ListGrandTotalStockMovementsDto>>(grandTotalStockMovements);
            }

        }


        [ValidationAspect(typeof(UpdateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateAsync(UpdateGrandTotalStockMovementsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<GrandTotalStockMovements>(entityQuery);

                var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
                {
                    Amount = input.Amount,
                    ProductID = input.ProductID,
                    TotalConsumption = input.TotalConsumption,
                    TotalGoodsIssue = input.TotalGoodsIssue,
                    TotalGoodsReceipt = input.TotalGoodsReceipt,
                    TotalProduction = input.TotalProduction,
                    TotalPurchaseOrder = input.TotalPurchaseOrder,
                    TotalPurchaseRequest = input.TotalPurchaseRequest,
                    TotalSalesOrder = input.TotalSalesOrder,
                    TotalReserved = input.TotalReserved,
                    TotalSalesProposition = input.TotalSalesProposition,
                    TotalWastage = input.TotalWastage,
                    WarehouseID = input.WarehouseID,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    BranchID = input.BranchID,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, grandTotalStockMovements, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovements);
            }

        }

        public Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
