using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ByDateStockMovement.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.ByDateStockMovement;
using TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ByDateStockMovements.Page;

namespace TsiErp.Business.Entities.ByDateStockMovement.Services
{
    [ServiceRegistration(typeof(IByDateStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class ByDateStockMovementsAppService : ApplicationService<ByDateStockMovementsResource>, IByDateStockMovementsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ByDateStockMovementsAppService(IStringLocalizer<ByDateStockMovementsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> CreateAsync(CreateByDateStockMovementsDto input)
        {
            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Insert(new CreateByDateStockMovementsDto
            {
                Amount = input.Amount,
                Date_ = input.Date_,
                ProductID = input.ProductID,
                TotalConsumption = input.TotalConsumption,
                TotalGoodsIssue = input.TotalGoodsIssue,
                TotalGoodsReceipt = input.TotalGoodsReceipt,
                TotalProduction = input.TotalProduction,
                TotalPurchaseOrder = input.TotalPurchaseOrder,
                TotalPurchaseRequest = input.TotalPurchaseRequest,
                TotalSalesOrder = input.TotalSalesOrder,
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

            var byDateStockMovements = queryFactory.Insert<SelectByDateStockMovementsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ByDateStockMovements, LogType.Insert, byDateStockMovements.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectByDateStockMovementsDto>(byDateStockMovements);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ByDateStockMovements, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectByDateStockMovementsDto>(byDateStockMovements);
        }

        public async Task<IDataResult<SelectByDateStockMovementsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ByDateStockMovements).Select<ByDateStockMovements>(bd => new { bd.WarehouseID, bd.TotalWastage, bd.TotalSalesProposition, bd.TotalSalesOrder, bd.TotalPurchaseRequest, bd.TotalPurchaseOrder, bd.TotalProduction, bd.TotalGoodsReceipt, bd.TotalGoodsIssue, bd.TotalConsumption, bd.ProductID, bd.Id, bd.Date_, bd.DataOpenStatusUserId, bd.DataOpenStatus, bd.BranchID, bd.Amount })
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ByDateStockMovements.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<Branches>
                        (
                            b => new { BranchID = b.Id, BranchCode = b.Code },
                            nameof(ByDateStockMovements.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                         .Join<Warehouses>
                        (
                            w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                            nameof(ByDateStockMovements.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ByDateStockMovements);

            var byDateStockMovement = queryFactory.Get<SelectByDateStockMovementsDto>(query);

            LogsAppService.InsertLogToDatabase(byDateStockMovement, byDateStockMovement, LoginedUserService.UserId, Tables.ByDateStockMovements, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectByDateStockMovementsDto>(byDateStockMovement);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListByDateStockMovementsDto>>> GetListAsync(ListByDateStockMovementsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ByDateStockMovements).Select<ByDateStockMovements>(bd => new { bd.WarehouseID, bd.TotalWastage, bd.TotalSalesProposition, bd.TotalSalesOrder, bd.TotalPurchaseRequest, bd.TotalPurchaseOrder, bd.TotalProduction, bd.TotalGoodsReceipt, bd.TotalGoodsIssue, bd.TotalConsumption, bd.ProductID, bd.Id, bd.Date_, bd.DataOpenStatusUserId, bd.DataOpenStatus, bd.BranchID, bd.Amount })
                        .Join<Products>
                        (
                            p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                            nameof(ByDateStockMovements.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<Branches>
                        (
                            b => new { BranchCode = b.Code, BranchID = b.Id },
                            nameof(ByDateStockMovements.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                         .Join<Warehouses>
                        (
                            w => new { WarehouseCode = w.Code, WarehouseID = w.Id, WarehouseName = w.Name },
                            nameof(ByDateStockMovements.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.ByDateStockMovements);

            var byDateStockMovements = queryFactory.GetList<ListByDateStockMovementsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListByDateStockMovementsDto>>(byDateStockMovements);
        }

        [ValidationAspect(typeof(UpdateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> UpdateAsync(UpdateByDateStockMovementsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ByDateStockMovements).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<ByDateStockMovements>(entityQuery);

            var query = queryFactory.Query().From(Tables.ByDateStockMovements).Update(new UpdateByDateStockMovementsDto
            {
                Amount = input.Amount,
                Date_ = input.Date_,
                ProductID = input.ProductID,
                TotalConsumption = input.TotalConsumption,
                TotalGoodsIssue = input.TotalGoodsIssue,
                TotalGoodsReceipt = input.TotalGoodsReceipt,
                TotalProduction = input.TotalProduction,
                TotalPurchaseOrder = input.TotalPurchaseOrder,
                TotalPurchaseRequest = input.TotalPurchaseRequest,
                TotalSalesOrder = input.TotalSalesOrder,
                TotalSalesProposition = input.TotalSalesProposition,
                TotalWastage = input.TotalWastage,
                WarehouseID = input.WarehouseID,
                BranchID = input.BranchID,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var byDateStockMovements = queryFactory.Update<SelectByDateStockMovementsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, byDateStockMovements, LoginedUserService.UserId, Tables.ByDateStockMovements, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectByDateStockMovementsDto>(byDateStockMovements);
        }

        public Task<IDataResult<SelectByDateStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
