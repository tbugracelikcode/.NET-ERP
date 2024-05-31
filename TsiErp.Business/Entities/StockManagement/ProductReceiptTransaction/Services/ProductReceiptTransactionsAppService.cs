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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.PurchaseOrdersAwaitingApproval.Services;
using TsiErp.Business.Entities.StockManagement.ProductReceiptTransaction.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductReceiptTransactions.Page;

namespace TsiErp.Business.Entities.ProductReceiptTransaction.Services
{
    [ServiceRegistration(typeof(IProductReceiptTransactionsAppService), DependencyInjectionType.Scoped)]
    public class ProductReceiptTransactionsAppService : ApplicationService<ProductReceiptTransactionsResource>, IProductReceiptTransactionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IPurchaseOrdersAwaitingApprovalsAppService _PurchaseOrdersAwaitingApprovalsAppService;

        public ProductReceiptTransactionsAppService(IStringLocalizer<ProductReceiptTransactionsResource> l, IGetSQLDateAppService getSQLDateAppService, IPurchaseOrdersAwaitingApprovalsAppService purchaseOrdersAwaitingApprovalsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _PurchaseOrdersAwaitingApprovalsAppService = purchaseOrdersAwaitingApprovalsAppService;
        }


        [ValidationAspect(typeof(CreateProductReceiptTransactionsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> CreateAsync(CreateProductReceiptTransactionsDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Insert(new CreateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                ProductReceiptTransactionStateEnum = 1,
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                PurchaseOrderQuantity = input.PurchaseOrderQuantity,
                SupplierProductCode = input.SupplierProductCode,
                WarehouseReceiptQuantity = input.WarehouseReceiptQuantity,
                WaybillDate = input.WaybillDate,
                WaybillNo = input.WaybillNo,
                PartyNo = input.PartyNo,
                WaybillQuantity = input.WaybillQuantity,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty
            });

            var ProductReceiptTransactions = queryFactory.Insert<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Insert, addedEntityId);

            #region Onay Bekleyen Satın Alma Siparişleri Create İşlemi

            CreatePurchaseOrdersAwaitingApprovalsDto createOrderApprovalModel = new CreatePurchaseOrdersAwaitingApprovalsDto
            {
                PurchaseOrdersAwaitingApprovalStateEnum = 1,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductReceiptTransactionID = addedEntityId,
            };

            await _PurchaseOrdersAwaitingApprovalsAppService.CreateAsync(createOrderApprovalModel);

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }




        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductReceiptTransactions).Select<ProductReceiptTransactions>(null)
                    .Join<PurchaseOrders>
                        (
                            p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                            nameof(ProductReceiptTransactions.PurchaseOrderID),
                            nameof(PurchaseOrders.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(ProductReceiptTransactions.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReceiptTransactions.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )

                        .Where(new { Id = id }, false, false, Tables.ProductReceiptTransactions);

            var ProductReceiptTransaction = queryFactory.Get<SelectProductReceiptTransactionsDto>(query);

            LogsAppService.InsertLogToDatabase(ProductReceiptTransaction, ProductReceiptTransaction, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransaction);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductReceiptTransactionsDto>>> GetListAsync(ListProductReceiptTransactionsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductReceiptTransactions).Select<ProductReceiptTransactions>(null)
                        .Join<PurchaseOrders>
                        (
                            p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                            nameof(ProductReceiptTransactions.PurchaseOrderID),
                            nameof(PurchaseOrders.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(ProductReceiptTransactions.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReceiptTransactions.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
               .Where(null, false, false, Tables.ProductReceiptTransactions);

            var productReceiptTransactions = queryFactory.GetList<ListProductReceiptTransactionsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductReceiptTransactionsDto>>(productReceiptTransactions);

        }

        [ValidationAspect(typeof(UpdateProductReceiptTransactionsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> UpdateAsync(UpdateProductReceiptTransactionsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReceiptTransactions).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<ProductReceiptTransactions>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Update(new UpdateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                Description_ = input.Description_,
                PurchaseOrderQuantity = input.PurchaseOrderQuantity,
                WaybillQuantity = input.WaybillQuantity,
                WaybillNo = input.WaybillNo,
                PartyNo = input.PartyNo,
                WaybillDate = input.WaybillDate,
                WarehouseReceiptQuantity = input.WarehouseReceiptQuantity,
                SupplierProductCode = input.SupplierProductCode,
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                ProductReceiptTransactionStateEnum = input.ProductReceiptTransactionStateEnum,
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, ProductReceiptTransactions, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }

        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReceiptTransactions).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<ProductReceiptTransactions>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Update(new UpdateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = entity.CurrentAccountCardID,
                ProductID = entity.ProductID,
                Description_ = entity.Description_,
                CreationTime = entity.CreationTime.Value,
                PartyNo = entity.PartyNo,
                PurchaseOrderQuantity = entity.PurchaseOrderQuantity,
                PurchaseOrderID = entity.PurchaseOrderID,
                ProductReceiptTransactionStateEnum = (int)entity.ProductReceiptTransactionStateEnum,
                PurchaseOrderLineID = entity.PurchaseOrderLineID,
                SupplierProductCode = entity.SupplierProductCode,
                WarehouseReceiptQuantity = entity.WarehouseReceiptQuantity,
                WaybillDate = entity.WaybillDate,
                WaybillNo = entity.WaybillNo,
                WaybillQuantity = entity.WaybillQuantity,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,

            }).Where(new { Id = id }, false, false, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);


        }
    }
}
