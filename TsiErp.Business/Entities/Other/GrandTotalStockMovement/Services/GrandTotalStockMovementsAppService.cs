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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public GrandTotalStockMovementsAppService(IStringLocalizer<GrandTotalStockMovementsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> CreateAsync(CreateGrandTotalStockMovementsDto input)
        {
            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Insert(new CreateGrandTotalStockMovementsDto
            {
                Amount = input.Amount,
                ProductID = input.ProductID.GetValueOrDefault(),
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
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                BranchID = input.BranchID.GetValueOrDefault(),
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
            });

            var grandTotalStockMovements = queryFactory.Insert<SelectGrandTotalStockMovementsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Insert, grandTotalStockMovements.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovements);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovements);
        }


        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.GrandTotalStockMovements).Select<GrandTotalStockMovements>(null)
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

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovement);
        }




        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListGrandTotalStockMovementsDto>>> GetListAsync(ListGrandTotalStockMovementsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.GrandTotalStockMovements).Select("*")
                        .Join<Branches>
                        (
                            b => new { BranchCode = b.Code },
                            nameof(GrandTotalStockMovements.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                        .Join<Products>
                        (
                            p => new { ProductCode = p.Code, ProductName = p.Name },
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
                        ).Where(null, false, false, Tables.GrandTotalStockMovements);

            var grandTotalStockMovements = queryFactory.GetList<ListGrandTotalStockMovementsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListGrandTotalStockMovementsDto>>(grandTotalStockMovements);

        }


        [ValidationAspect(typeof(UpdateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateAsync(UpdateGrandTotalStockMovementsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.GrandTotalStockMovements).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<GrandTotalStockMovements>(entityQuery);

            var query = queryFactory.Query().From(Tables.GrandTotalStockMovements).Update(new UpdateGrandTotalStockMovementsDto
            {
                Amount = input.Amount,
                ProductID = input.ProductID.GetValueOrDefault(),
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
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                BranchID = input.BranchID.GetValueOrDefault(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var grandTotalStockMovements = queryFactory.Update<SelectGrandTotalStockMovementsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, grandTotalStockMovements, LoginedUserService.UserId, Tables.GrandTotalStockMovements, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(grandTotalStockMovements);


        }

        public Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
