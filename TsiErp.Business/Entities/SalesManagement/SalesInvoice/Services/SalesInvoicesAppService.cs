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
using TsiErp.Business.Entities.SalesInvoice.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Entities.StockFiche.Services;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesInvoices.Page;

namespace TsiErp.Business.Entities.SalesInvoice.Services
{
    [ServiceRegistration(typeof(ISalesInvoicesAppService), DependencyInjectionType.Scoped)]
    public class SalesInvoicesAppService : ApplicationService<SalesInvoicesResource>, ISalesInvoicesAppService
    {
        private readonly ISalesPropositionsAppService _salesPropositionsAppService;
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IStockFichesAppService _StockFichesAppService;


        public SalesInvoicesAppService(IStringLocalizer<SalesInvoicesResource> l, ISalesPropositionsAppService salesPropositionsAppService, IGetSQLDateAppService getSQLDateAppService, IFicheNumbersAppService ficheNumbersAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IStockFichesAppService stockFichesAppService) : base(l)
        {
            _salesPropositionsAppService = salesPropositionsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _StockFichesAppService = stockFichesAppService;
        }



        [ValidationAspect(typeof(CreateSalesInvoiceValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectSalesInvoiceDto>> CreateAsync(CreateSalesInvoiceDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.SalesInvoices).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<SalesInvoices>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.SalesInvoices).Insert(new CreateSalesInvoiceDto
            {
                LinkedSalesPropositionID = Guid.Empty,
                SalesInvoicesState = input.SalesInvoicesState,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                CustomerOrderNr = input.CustomerOrderNr,
                OrderAcceptanceRecordID = input.OrderAcceptanceRecordID.GetValueOrDefault(),
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                CustomerRequestedDate = input.CustomerRequestedDate.GetValueOrDefault(),
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID,
                ShippingAdressID = input.ShippingAdressID == null ? Guid.Empty : input.ShippingAdressID,
                SpecialCode = input.SpecialCode,
                Time_ = time,
                TotalDiscountAmount = input.TotalDiscountAmount,
                isStandart = input.isStandart,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                WorkOrderCreationDate = input.WorkOrderCreationDate.GetValueOrDefault(),
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
                PricingCurrency = input.PricingCurrency

            });

            foreach (var item in input.SelectSalesInvoiceLines)
            {

                var queryLine = queryFactory.Query().From(Tables.SalesInvoiceLines).Insert(new CreateSalesInvoiceLinesDto
                {
                    LikedPropositionLineID = Guid.Empty,
                    SalesInvoiceLinesStateEnum = (int)item.SalesInvoiceLinesStateEnum,
                    DiscountAmount = item.DiscountAmount,
                    WorkOrderCreationDate = item.WorkOrderCreationDate.GetValueOrDefault(),
                    ProductGroupID = item.ProductGroupID.GetValueOrDefault(),
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
                    SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                    OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                    OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                    PurchaseSupplyDate = item.PurchaseSupplyDate.GetValueOrDefault(),
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    SalesInvoiceID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var SalesInvoice = queryFactory.Insert<SelectSalesInvoiceDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("SalesInvoicesChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesInvoices, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["SalesInvoicesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.FicheNo,
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

                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.FicheNo,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            return new SuccessDataResult<SelectSalesInvoiceDto>(SalesInvoice);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.SalesInvoices).Select("*").Where(new { Id = id }, "");

            var SalesInvoices = queryFactory.Get<SelectSalesInvoiceDto>(query);

            if (SalesInvoices.Id != Guid.Empty && SalesInvoices != null)
            {

                var deleteQuery = queryFactory.Query().From(Tables.SalesInvoices).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.SalesInvoiceLines).Delete(LoginedUserService.UserId).Where(new { SalesInvoiceID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var SalesInvoice = queryFactory.Update<SelectSalesInvoiceDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesInvoices, LogType.Delete, id);

                SelectStockFichesDto stockFiche = (await _StockFichesAppService.GetbySalesInvoiceAsync(id)).Data;

                if (stockFiche != null && stockFiche.Id != Guid.Empty)
                {
                    await _StockFichesAppService.DeleteAsync(stockFiche.Id);
                }

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["SalesInvoicesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains("*Not*"))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.FicheNo,
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

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = entity.FicheNo,
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
                return new SuccessDataResult<SelectSalesInvoiceDto>(SalesInvoice);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.SalesInvoiceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var SalesInvoiceLines = queryFactory.Update<SelectSalesInvoiceLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesInvoiceLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectSalesInvoiceLinesDto>(SalesInvoiceLines);
            }

        }

        public async Task<IDataResult<SelectSalesInvoiceDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.SalesInvoices)
                   .Select<SalesInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(SalesInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        c => new { OrderAcceptanceRecordID = c.Id },
                        nameof(SalesInvoices.OrderAcceptanceRecordID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesInvoices.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(SalesInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.SalesInvoices);

            var salesInvoices = queryFactory.Get<SelectSalesInvoiceDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesInvoiceLines)
                   .Select<SalesInvoiceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesInvoiceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductGroups>
                    (
                        p => new { ProductGroupID = p.Id, ProductGroupName = p.Name },
                        nameof(SalesInvoiceLines.ProductGroupID),
                        nameof(ProductGroups.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesInvoiceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesInvoiceLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesInvoiceLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesInvoiceLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesInvoiceLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesInvoiceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesInvoiceID = id }, Tables.SalesInvoiceLines);

            var SalesInvoiceLine = queryFactory.GetList<SelectSalesInvoiceLinesDto>(queryLines).ToList();

            salesInvoices.SelectSalesInvoiceLines = SalesInvoiceLine;

            LogsAppService.InsertLogToDatabase(salesInvoices, salesInvoices, LoginedUserService.UserId, Tables.SalesInvoices, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesInvoiceDto>(salesInvoices);

        }

        public async Task<IDataResult<IList<ListSalesInvoiceDto>>> GetListAsync(ListSalesInvoiceParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.SalesInvoices)
                   .Select<SalesInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        c => new { OrderAcceptanceRecordID = c.Id },
                        nameof(SalesInvoices.OrderAcceptanceRecordID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(SalesInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesInvoices.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode, CurrentAccountCardID = ca.Id },
                        nameof(SalesInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(SalesInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.SalesInvoices);

            var salesInvoices = queryFactory.GetList<ListSalesInvoiceDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListSalesInvoiceDto>>(salesInvoices);

        }


        [ValidationAspect(typeof(UpdateSalesInvoiceValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectSalesInvoiceDto>> UpdateAsync(UpdateSalesInvoiceDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                     .From(Tables.SalesInvoices)
                   .Select<SalesInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(SalesInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        c => new { OrderAcceptanceRecordID = c.Id },
                        nameof(SalesInvoices.OrderAcceptanceRecordID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesInvoices.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCard = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code },
                        nameof(SalesInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.SalesInvoices);

            var entity = queryFactory.Get<SelectSalesInvoiceDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.SalesInvoiceLines)
                   .Select<SalesInvoiceLines>(null)
                    .Join<ProductGroups>
                    (
                        p => new { ProductGroupID = p.Id, ProductGroupName = p.Name },
                        nameof(SalesInvoiceLines.ProductGroupID),
                        nameof(ProductGroups.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name, p.isStandart },
                        nameof(SalesInvoiceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesInvoiceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(SalesInvoiceLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                      .Join<SalesPropositionLines>
                    (
                        spl => new { LikedPropositionLineID = spl.Id, LinkedSalesPropositionID = spl.SalesPropositionID },
                        nameof(SalesInvoiceLines.LikedPropositionLineID),
                        nameof(SalesPropositionLines.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesInvoiceLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesInvoiceLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesInvoiceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesInvoiceID = input.Id }, Tables.SalesInvoiceLines);

            var SalesInvoiceLine = queryFactory.GetList<SelectSalesInvoiceLinesDto>(queryLines).ToList();

            entity.SelectSalesInvoiceLines = SalesInvoiceLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                          .From(Tables.SalesInvoices)
                   .Select<SalesInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(SalesInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(SalesInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(SalesInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.SalesInvoices);

            var list = queryFactory.GetList<ListSalesInvoiceDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.SalesInvoices).Update(new UpdateSalesInvoiceDto
            {
                LinkedSalesPropositionID = input.LinkedSalesPropositionID,
                SalesInvoicesState = input.SalesInvoicesState,
                FicheNo = input.FicheNo,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                OrderAcceptanceRecordID = input.OrderAcceptanceRecordID.GetValueOrDefault(),
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                BranchID = input.BranchID,
                isStandart = input.isStandart,
                CurrencyID = input.CurrencyID,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ConfirmedLoadingDate = input.ConfirmedLoadingDate.GetValueOrDefault(),
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
                CustomerRequestedDate = input.CustomerRequestedDate.GetValueOrDefault(),
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID,
                WorkOrderCreationDate = input.WorkOrderCreationDate.GetValueOrDefault(),
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
                PricingCurrency = input.PricingCurrency,
                CurrentAccountCardID = input.CurrentAccountCardID,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectSalesInvoiceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesInvoiceLines).Insert(new CreateSalesInvoiceLinesDto
                    {
                        SalesInvoiceLinesStateEnum = (int)item.SalesInvoiceLinesStateEnum,
                        LikedPropositionLineID = item.LikedPropositionLineID.GetValueOrDefault(),
                        DiscountAmount = item.DiscountAmount,
                        WorkOrderCreationDate = item.WorkOrderCreationDate.GetValueOrDefault(),
                        DiscountRate = item.DiscountRate,
                        TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                        OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                        OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                        ProductGroupID = item.ProductGroupID.GetValueOrDefault(),
                        ExchangeRate = item.ExchangeRate,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        PurchaseSupplyDate = item.PurchaseSupplyDate.GetValueOrDefault(),
                        LineTotalAmount = item.LineTotalAmount,
                        PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                        SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        SalesInvoiceID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        BranchID = input.BranchID,
                        CurrentAccountCardID = input.CurrentAccountCardID,
                        Date_ = input.Date_,
                        WarehouseID = input.WarehouseID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.SalesInvoiceLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectSalesInvoiceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesInvoiceLines).Update(new UpdateSalesInvoiceLinesDto
                        {
                            LikedPropositionLineID = item.LikedPropositionLineID.GetValueOrDefault(),
                            SalesInvoiceLinesStateEnum = (int)item.SalesInvoiceLinesStateEnum,
                            DiscountAmount = item.DiscountAmount,
                            TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                            WorkOrderCreationDate = item.WorkOrderCreationDate.GetValueOrDefault(),
                            SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                            OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID.GetValueOrDefault(),
                            ProductGroupID = item.ProductGroupID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = item.OrderAcceptanceRecordID.GetValueOrDefault(),
                            DiscountRate = item.DiscountRate,
                            ExchangeRate = item.ExchangeRate,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            PurchaseSupplyDate = item.PurchaseSupplyDate.GetValueOrDefault(),
                            LineTotalAmount = item.LineTotalAmount,
                            PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                            UnitPrice = item.UnitPrice,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            SalesInvoiceID = input.Id,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            BranchID = input.BranchID,
                            CurrentAccountCardID = input.CurrentAccountCardID,
                            Date_ = input.Date_,
                            WarehouseID = input.WarehouseID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var SalesInvoice = queryFactory.Update<SelectSalesInvoiceDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.SalesInvoices, LogType.Update, SalesInvoice.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["SalesInvoicesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.FicheNo,
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

                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectSalesInvoiceDto>(SalesInvoice);

        }

        public async Task<IDataResult<SelectSalesInvoiceDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.SalesInvoices).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<SalesInvoices>(entityQuery);

            var query = queryFactory.Query().From(Tables.SalesInvoices).Update(new UpdateSalesInvoiceDto
            {
                LinkedSalesPropositionID = entity.LinkedSalesPropositionID,
                SalesInvoicesState = (int)entity.SalesInvoicesState,
                FicheNo = entity.FicheNo,
                BranchID = entity.BranchID,
                TransactionExchangeGrossAmount = entity.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = entity.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = entity.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = entity.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = entity.TransactionExchangeTotalVatExcludedAmount,
                OrderAcceptanceRecordID = entity.OrderAcceptanceRecordID.GetValueOrDefault(),
                SalesOrderID = entity.SalesOrderID,
                CurrencyID = entity.CurrencyID,
                isStandart = entity.isStandart,
                ConfirmedLoadingDate = entity.ConfirmedLoadingDate,
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
                PricingCurrency = (int)entity.PricingCurrency,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var SalesInvoicesDto = queryFactory.Update<SelectSalesInvoiceDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesInvoiceDto>(SalesInvoicesDto);

        }
    }
}
