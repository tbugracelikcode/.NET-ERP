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
using TsiErp.Business.Entities.SalesOrder.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesOrders.Page;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    [ServiceRegistration(typeof(ISalesOrdersAppService), DependencyInjectionType.Scoped)]
    public class SalesOrdersAppService : ApplicationService<SalesOrdersResource>, ISalesOrdersAppService
    {
        private readonly ISalesPropositionsAppService _salesPropositionsAppService;
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;


        public SalesOrdersAppService(IStringLocalizer<SalesOrdersResource> l, ISalesPropositionsAppService salesPropositionsAppService, IGetSQLDateAppService getSQLDateAppService, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            _salesPropositionsAppService = salesPropositionsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> CreateAsync(CreateSalesOrderDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.SalesOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
            var list = queryFactory.ControlList<SalesOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            if (input.SalesOrderState == 0) input.SalesOrderState = 1;

            string now = _GetSQLDateAppService.GetDateFromSQL().ToString();

            string[] timeSplit = now.Split(" ");

            string time = timeSplit[1];

            var query = queryFactory.Query().From(Tables.SalesOrders).Insert(new CreateSalesOrderDto
            {
                LinkedSalesPropositionID = Guid.Empty,
                SalesOrderState = input.SalesOrderState,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                CustomerOrderNr = input.CustomerOrderNr,
                OrderAcceptanceRecordID = input.OrderAcceptanceRecordID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID,
                ShippingAdressID = input.ShippingAdressID == null ? Guid.Empty : input.ShippingAdressID,
                SpecialCode = input.SpecialCode,
                Time_ = time,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                WorkOrderCreationDate = input.WorkOrderCreationDate,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                PricingCurrency = input.PricingCurrency
            });

            foreach (var item in input.SelectSalesOrderLines)
            {
                if ((int)item.SalesOrderLineStateEnum == 0) item.SalesOrderLineStateEnum = TsiErp.Entities.Enums.SalesOrderLineStateEnum.Beklemede;

                var queryLine = queryFactory.Query().From(Tables.SalesOrderLines).Insert(new CreateSalesOrderLinesDto
                {
                    LikedPropositionLineID = Guid.Empty,
                    SalesOrderLineStateEnum = (int)item.SalesOrderLineStateEnum,
                    DiscountAmount = item.DiscountAmount,
                    WorkOrderCreationDate = item.WorkOrderCreationDate,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    DiscountRate = item.DiscountRate,
                    ExchangeRate = item.ExchangeRate,
                    LineAmount = item.LineAmount,
                    LineDescription = item.LineDescription,
                    LineTotalAmount = item.LineTotalAmount,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                    OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                    PurchaseSupplyDate = item.PurchaseSupplyDate,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    SalesOrderID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var salesOrder = queryFactory.Insert<SelectSalesOrderDto>(query, "Id", true);

            StockMovementsService.InsertSalesOrders(input);

            await FicheNumbersAppService.UpdateFicheNumberAsync("SalesOrdersChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesOrders, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectSalesOrderDto>(salesOrder);

        }

        [ValidationAspect(typeof(CreateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.SalesOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
            var list = queryFactory.ControlList<SalesOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.SalesOrders).Insert(new CreateSalesOrderDto
            {
                LinkedSalesPropositionID = input.LinkedSalesPropositionID,
                SalesOrderState = input.SalesOrderState,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                OrderAcceptanceRecordID = input.OrderAcceptanceRecordID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                CustomerRequestedDate = input.CustomerRequestedDate,
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                CustomerOrderNr = input.CustomerOrderNr,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID,
                ShippingAdressID = input.ShippingAdressID == null ? Guid.Empty : input.ShippingAdressID,
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID,
                WorkOrderCreationDate = input.WorkOrderCreationDate,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                PricingCurrency = input.PricingCurrency
            });

            foreach (var item in input.SelectSalesOrderLines)
            {
                var queryLine = queryFactory.Query().From(Tables.SalesOrderLines).Insert(new CreateSalesOrderLinesDto
                {
                    LikedPropositionLineID = item.LikedPropositionLineID.GetValueOrDefault(),
                    SalesOrderLineStateEnum = (int)item.SalesOrderLineStateEnum,
                    DiscountAmount = item.DiscountAmount,
                    WorkOrderCreationDate = item.WorkOrderCreationDate,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                    OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                    DiscountRate = item.DiscountRate,
                    ExchangeRate = item.ExchangeRate,
                    LineAmount = item.LineAmount,
                    LineDescription = item.LineDescription,
                    PurchaseSupplyDate = item.PurchaseSupplyDate,
                    LineTotalAmount = item.LineTotalAmount,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    SalesOrderID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var salesOrder = queryFactory.Insert<SelectSalesOrderDto>(query, "Id", true);

            await _salesPropositionsAppService.UpdateSalesPropositionLineState(input.SelectSalesOrderLines, TsiErp.Entities.Enums.SalesPropositionLineStateEnum.Siparis);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesOrders, LogType.Insert, salesOrder.Id);

            return new SuccessDataResult<SelectSalesOrderDto>(salesOrder);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.SalesOrders).Select("*").Where(new { Id = id }, false, false, "");

            var salesOrders = queryFactory.Get<SelectSalesOrderDto>(query);

            if (salesOrders.Id != Guid.Empty && salesOrders != null)
            {
                StockMovementsService.DeleteSalesOrders(salesOrders);

                var deleteQuery = queryFactory.Query().From(Tables.SalesOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.SalesOrderLines).Delete(LoginedUserService.UserId).Where(new { SalesOrderID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var salesOrder = queryFactory.Update<SelectSalesOrderDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesOrders, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectSalesOrderDto>(salesOrder);
            }
            else
            {
                var queryLineGet = queryFactory.Query().From(Tables.SalesOrderLines).Select("*").Where(new { Id = id }, false, false, "");

                var salesOrdersLineGet = queryFactory.Get<SelectSalesOrderLinesDto>(queryLineGet);

                StockMovementsService.DeleteSalesOrderLines(salesOrdersLineGet);

                var queryLine = queryFactory.Query().From(Tables.SalesOrderLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var salesOrderLines = queryFactory.Update<SelectSalesOrderLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesOrderLines, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectSalesOrderLinesDto>(salesOrderLines);
            }

        }

        public async Task<IDataResult<SelectSalesOrderDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.SalesOrders)
                   .Select<SalesOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(SalesOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        c => new { OrderAcceptanceRecordID = c.Id, ConfirmedLoadingDate = c.ConfirmedLoadingDate },
                        nameof(SalesOrders.OrderAcceptanceRecordID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(SalesOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.SalesOrders);

            var salesOrders = queryFactory.Get<SelectSalesOrderDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesOrderLines)
                   .Select<SalesOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesOrderLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesOrderID = id }, false, false, Tables.SalesOrderLines);

            var salesOrderLine = queryFactory.GetList<SelectSalesOrderLinesDto>(queryLines).ToList();

            salesOrders.SelectSalesOrderLines = salesOrderLine;

            LogsAppService.InsertLogToDatabase(salesOrders, salesOrders, LoginedUserService.UserId, Tables.SalesOrders, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesOrderDto>(salesOrders);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesOrderDto>>> GetListAsync(ListSalesOrderParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.SalesOrders)
                   .Select<SalesOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(SalesOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        c => new { OrderAcceptanceRecordID = c.Id, ConfirmedLoadingDate = c.ConfirmedLoadingDate },
                        nameof(SalesOrders.OrderAcceptanceRecordID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(SalesOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode, CurrentAccountCardID = ca.Id },
                        nameof(SalesOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(SalesOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.SalesOrders);

            var salesOrders = queryFactory.GetList<ListSalesOrderDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListSalesOrderDto>>(salesOrders);

        }

        public async Task<IDataResult<IList<SelectSalesOrderLinesDto>>> GetLineSelectListAsync()
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.SalesOrderLines)
                   .Select<SalesOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesOrderLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    //.Where(null, false, false, Tables.SalesOrderLines)
                    .Where("SalesOrderLineStateEnum", "<>", 5, Tables.SalesOrderLines);

            var salesOrderLines = queryFactory.GetList<SelectSalesOrderLinesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectSalesOrderLinesDto>>(salesOrderLines);

        }

        [ValidationAspect(typeof(UpdateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> UpdateAsync(UpdateSalesOrderDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                     .From(Tables.SalesOrders)
                   .Select<SalesOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(SalesOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        c => new { OrderAcceptanceRecordID = c.Id, ConfirmedLoadingDate = c.ConfirmedLoadingDate },
                        nameof(SalesOrders.OrderAcceptanceRecordID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code },
                        nameof(SalesOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, false, false, Tables.SalesOrders);

            var entity = queryFactory.Get<SelectSalesOrderDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.SalesOrderLines)
                   .Select<SalesOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesOrderLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesOrderID = input.Id }, false, false, Tables.SalesOrderLines);

            var salesOrderLine = queryFactory.GetList<SelectSalesOrderLinesDto>(queryLines).ToList();

            entity.SelectSalesOrderLines = salesOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                          .From(Tables.SalesOrders)
                   .Select<SalesOrders>(so => new { so.WorkOrderCreationDate, so.WarehouseID, so.TotalVatExcludedAmount, so.TotalVatAmount, so.TotalDiscountAmount, so.Time_, so.SpecialCode, so.ShippingAdressID, so.SalesOrderState, so.PaymentPlanID, so.NetAmount, so.LinkedSalesPropositionID, so.Id, so.GrossAmount, so.FicheNo, so.ExchangeRate, so.Description_, so.Date_, so.DataOpenStatusUserId, so.DataOpenStatus, so.CurrentAccountCardID, so.CurrencyID, so.BranchID })
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(SalesOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(SalesOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(SalesOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, false, false, Tables.SalesOrders);

            var list = queryFactory.GetList<ListSalesOrderDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.SalesOrders).Update(new UpdateSalesOrderDto
            {
                LinkedSalesPropositionID = input.LinkedSalesPropositionID,
                SalesOrderState = input.SalesOrderState,
                FicheNo = input.FicheNo,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                OrderAcceptanceRecordID = input.OrderAcceptanceRecordID.GetValueOrDefault(),
                BranchID = input.BranchID,
                CurrencyID = input.CurrencyID,
                CurrentAccountCardID = input.CurrentAccountCardID,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                CustomerOrderNr = input.CustomerOrderNr,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID,
                ShippingAdressID = input.ShippingAdressID.GetValueOrDefault(),
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                CustomerRequestedDate = input.CustomerRequestedDate,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID,
                WorkOrderCreationDate = input.WorkOrderCreationDate,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                PricingCurrency = input.PricingCurrency
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectSalesOrderLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesOrderLines).Insert(new CreateSalesOrderLinesDto
                    {
                        SalesOrderLineStateEnum = (int)item.SalesOrderLineStateEnum,
                        LikedPropositionLineID = item.LikedPropositionLineID.GetValueOrDefault(),
                        DiscountAmount = item.DiscountAmount,
                        WorkOrderCreationDate = item.WorkOrderCreationDate,
                        DiscountRate = item.DiscountRate,
                        TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                        OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                        OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                        ExchangeRate = item.ExchangeRate,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
                        LineTotalAmount = item.LineTotalAmount,
                        PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        SalesOrderID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        BranchID = input.BranchID,
                        CurrentAccountCardID = input.CurrentAccountCardID,
                        Date_ = input.Date_,
                        WarehouseID = input.WarehouseID
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.SalesOrderLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectSalesOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesOrderLines).Update(new UpdateSalesOrderLinesDto
                        {
                            LikedPropositionLineID = item.LikedPropositionLineID.GetValueOrDefault(),
                            SalesOrderLineStateEnum = (int)item.SalesOrderLineStateEnum,
                            DiscountAmount = item.DiscountAmount,
                            TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                            WorkOrderCreationDate = item.WorkOrderCreationDate.GetValueOrDefault(),
                            OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                            DiscountRate = item.DiscountRate,
                            ExchangeRate = item.ExchangeRate,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            PurchaseSupplyDate = item.PurchaseSupplyDate,
                            LineTotalAmount = item.LineTotalAmount,
                            PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                            UnitPrice = item.UnitPrice,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            SalesOrderID = input.Id,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            BranchID = input.BranchID,
                            CurrentAccountCardID = input.CurrentAccountCardID,
                            Date_ = input.Date_,
                            WarehouseID = input.WarehouseID
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var salesOrder = queryFactory.Update<SelectSalesOrderDto>(query, "Id", true);

            StockMovementsService.UpdateSalesOrders(entity, input);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.SalesOrders, LogType.Update, salesOrder.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesOrderDto>(salesOrder);

        }

        public async Task<IDataResult<SelectSalesOrderDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.SalesOrders).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<SalesOrders>(entityQuery);

            var query = queryFactory.Query().From(Tables.SalesOrders).Update(new UpdateSalesOrderDto
            {
                LinkedSalesPropositionID = entity.LinkedSalesPropositionID,
                SalesOrderState = (int)entity.SalesOrderState,
                FicheNo = entity.FicheNo,
                BranchID = entity.BranchID,
                TransactionExchangeGrossAmount = entity.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = entity.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = entity.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = entity.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = entity.TransactionExchangeTotalVatExcludedAmount,
                OrderAcceptanceRecordID = entity.OrderAcceptanceRecordID.GetValueOrDefault(),
                CurrencyID = entity.CurrencyID,
                CustomerOrderNr = entity.CustomerOrderNr,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
                CustomerRequestedDate = entity.CustomerRequestedDate,
                ExchangeRate = entity.ExchangeRate,
                GrossAmount = entity.GrossAmount,
                NetAmount = entity.NetAmount,
                PaymentPlanID = entity.PaymentPlanID,
                ShippingAdressID = entity.ShippingAdressID == null ? Guid.Empty : entity.ShippingAdressID,
                SpecialCode = entity.SpecialCode,
                Time_ = entity.Time_,
                TotalDiscountAmount = entity.TotalDiscountAmount,
                TotalVatAmount = entity.TotalVatAmount,
                TotalVatExcludedAmount = entity.TotalVatExcludedAmount,
                WarehouseID = entity.WarehouseID,
                WorkOrderCreationDate = entity.WorkOrderCreationDate,
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
                PricingCurrency = (int)entity.PricingCurrency
            }).Where(new { Id = id }, false, false, "");

            var salesOrdersDto = queryFactory.Update<SelectSalesOrderDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesOrderDto>(salesOrdersDto);

        }

        public decimal GetLastOrderPriceByCurrentAccountProduct(Guid CurrentAccountID, Guid ProductID)
        {
            var query = queryFactory
                  .Query()
                  .From(Tables.SalesOrders)
                  .Select<SalesOrders>(null)
                  .Join<PaymentPlans>
                   (
                       pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                       nameof(SalesOrders.PaymentPlanID),
                       nameof(PaymentPlans.Id),
                       JoinType.Left
                   )
                   .Join<Branches>
                   (
                       b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                       nameof(SalesOrders.BranchID),
                       nameof(Branches.Id),
                       JoinType.Left
                   )
                    .Join<Warehouses>
                   (
                       w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                       nameof(SalesOrders.WarehouseID),
                       nameof(Warehouses.Id),
                       JoinType.Left
                   )
                   .Join<OrderAcceptanceRecords>
                   (
                       c => new { OrderAcceptanceRecordID = c.Id, ConfirmedLoadingDate = c.ConfirmedLoadingDate },
                       nameof(SalesOrders.OrderAcceptanceRecordID),
                       nameof(OrderAcceptanceRecords.Id),
                       JoinType.Left
                   )
                    .Join<Currencies>
                   (
                       c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                       nameof(SalesOrders.CurrencyID),
                       nameof(Currencies.Id),
                       JoinType.Left
                   )
                     .Join<Currencies>
                   (
                       w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                       nameof(SalesOrders.TransactionExchangeCurrencyID),
                       nameof(Currencies.Id),
                       "TransactionExchangeCurrency",
                       JoinType.Left
                   )
                    .Join<CurrentAccountCards>
                   (
                       ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                       nameof(SalesOrders.CurrentAccountCardID),
                       nameof(CurrentAccountCards.Id),
                       JoinType.Left)

                        .Join<ShippingAdresses>
                   (
                       sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                       nameof(SalesOrders.ShippingAdressID),
                       nameof(ShippingAdresses.Id),
                       JoinType.Left
                   )
                   .Where(new { CurrentAccountCardID = CurrentAccountID }, false, false, Tables.SalesOrders);

            var salesOrders = queryFactory.Get<SelectSalesOrderDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesOrderLines)
                   .Select<SalesOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesOrderLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesOrderID = salesOrders.Id }, false, false, Tables.SalesOrderLines)
                    .Where(new { ProductID = ProductID }, false, false, Tables.SalesOrderLines);

            var salesOrderLine = queryFactory.Get<SelectSalesOrderLinesDto>(queryLines);

            return salesOrderLine.UnitPrice;
        }
    }
}
