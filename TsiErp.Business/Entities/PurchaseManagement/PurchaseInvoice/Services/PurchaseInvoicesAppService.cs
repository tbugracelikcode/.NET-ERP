using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.OrderAcceptanceRecord.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ProductReceiptTransaction.Services;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseInvoices.Page;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice;
using TsiErp.Business.Entities.PurchaseManagement.PurchaseInvoice.Validations;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine;
using TSI.QueryBuilder.Models;
using TsiErp.Business.Entities.StockFiche.Services;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;

namespace TsiErp.Business.Entities.PurchaseManagement.PurchaseInvoice.Services
{
    [ServiceRegistration(typeof(IPurchaseInvoicesAppService), DependencyInjectionType.Scoped)]
    public class PurchaseInvoicesAppService : ApplicationService<PurchaseInvoicesResource>, IPurchaseInvoicesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();


        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;
        private readonly IOrderAcceptanceRecordsAppService _OrderAcceptanceRecordsAppService;
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IProductReceiptTransactionsAppService _ProductReceiptTransactionsAppService;
        private readonly IStockFichesAppService _StockFichesAppService;
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public PurchaseInvoicesAppService(IStringLocalizer<PurchaseInvoicesResource> l, IPurchaseRequestsAppService PurchaseRequestsAppService, IFicheNumbersAppService ficheNumbersAppService, IOrderAcceptanceRecordsAppService orderAcceptanceRecordsAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IProductReceiptTransactionsAppService productReceiptTransactionsAppService, IStockFichesAppService stockFichesAppService) : base(l)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _OrderAcceptanceRecordsAppService = orderAcceptanceRecordsAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _ProductReceiptTransactionsAppService = productReceiptTransactionsAppService;
            _StockFichesAppService = stockFichesAppService;
        }
        [ValidationAspect(typeof(CreatePurchaseInvoicesValidator), Priority = 1)]
        public async Task<IDataResult<SelectPurchaseInvoicesDto>> CreateAsync(CreatePurchaseInvoicesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseInvoices).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<PurchaseInvoices>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.PurchaseInvoices).Insert(new CreatePurchaseInvoicesDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                PurchaseInvoiceWayBillStatusEnum = input.PurchaseInvoiceWayBillStatusEnum,
                NetAmount = input.NetAmount,
                MRPID = input.MRPID.GetValueOrDefault(),
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseInvoiceState = input.PurchaseInvoiceState,
                ShippingAdressID = Guid.Empty,
                SpecialCode = input.SpecialCode,
                Time_ = time,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID,
                WorkOrderCreationDate = input.WorkOrderCreationDate,
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
                PriceApprovalState = input.PriceApprovalState,
                PricingCurrency = input.PricingCurrency,

            });

            DateTime biggestDate = input.SelectPurchaseInvoiceLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseInvoiceLinesDto)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Insert(new CreatePurchaseInvoiceLinesDto
                {
                    DiscountAmount = item.DiscountAmount,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    WorkOrderCreationDate = item.WorkOrderCreationDate,
                    DiscountRate = item.DiscountRate,
                    PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                    WaitingQuantity = item.WaitingQuantity,
                    ExchangeRate = item.ExchangeRate,
                    LikedPurchaseRequestLineID = Guid.Empty,
                    LineAmount = item.LineAmount,
                    PurchaseInvoiceLineWayBillStatusEnum = item.PurchaseInvoiceLineWayBillStatusEnum,
                    PartyNo = item.PartyNo,
                    LineDescription = item.LineDescription,
                    SupplierBillNo = item.SupplierBillNo,
                    SupplierWaybillNo = item.SupplierWaybillNo,
                    LineTotalAmount = item.LineTotalAmount,
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    LinkedPurchaseRequestID = Guid.Empty,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    PurchaseOrderLineID = item.PurchaseOrderLineID.GetValueOrDefault(),
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PurchaseInvoiceLineStateEnum = item.PurchaseInvoiceLineStateEnum,
                    SupplyDate = item.SupplyDate,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    PurchaseInvoiceID = addedEntityId,
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
                    SupplierReferenceNo = item.SupplierReferenceNo
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

            }

            var purchaseInvoice = queryFactory.Insert<SelectPurchaseInvoicesDto>(query, "Id", true);


            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseInvoicesChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseInvoices, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseInvoicesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectPurchaseInvoicesDto>(purchaseInvoice);


        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("InvoiceID", new List<string>
            {
                Tables.PurchaseUnsuitabilityReports
            });

            DeleteControl.ControlList.Add("PurchaseInvoiceID", new List<string>
            {
                Tables.StockFiches,
                Tables.StockFicheLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.PurchaseInvoices).Select("*").Where(new { Id = id }, "");

                var purchaseInvoices = queryFactory.Get<SelectPurchaseInvoicesDto>(query);

                if (purchaseInvoices.Id != Guid.Empty && purchaseInvoices != null)
                {
                    StockMovementsService.DeletePurchaseInvoices(purchaseInvoices);

                    var deleteQuery = queryFactory.Query().From(Tables.PurchaseInvoices).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Delete(LoginedUserService.UserId).Where(new { PurchaseInvoiceID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var purchaseInvoice = queryFactory.Update<SelectPurchaseInvoicesDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseInvoices, LogType.Delete, id);

                    SelectStockFichesDto stockFiche = (await _StockFichesAppService.GetbyPurchaseInvoiceAsync(id)).Data;

                    if (stockFiche != null && stockFiche.Id != Guid.Empty)
                    {
                        await _StockFichesAppService.DeleteAsync(stockFiche.Id);
                    }


                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseInvoicesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                    return new SuccessDataResult<SelectPurchaseInvoicesDto>(purchaseInvoices);
                }
                else
                {
                    var queryLineGet = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Select("*").Where(new { Id = id }, "");

                    var purchaseInvoicesLineGet = queryFactory.Get<SelectPurchaseInvoiceLinesDto>(queryLineGet);


                    var queryLine = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var purchaseInvoiceLines = queryFactory.Update<SelectPurchaseInvoiceLinesDto>(queryLine, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseInvoiceLines, LogType.Delete, id);

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectPurchaseInvoiceLinesDto>(purchaseInvoiceLines);
                }
            }
        }

        public async Task<IDataResult<SelectPurchaseInvoicesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseInvoices)
                   .Select<PurchaseInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseInvoices.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseInvoices.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseInvoices.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.PurchaseInvoices);

            var purchaseInvoices = queryFactory.Get<SelectPurchaseInvoicesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseInvoiceLines)
                   .Select<PurchaseInvoiceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseInvoiceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseInvoiceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseInvoiceLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoiceLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseInvoiceLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoiceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseInvoiceLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseInvoiceID = id }, Tables.PurchaseInvoiceLines);

            var purchaseInvoiceLine = queryFactory.GetList<SelectPurchaseInvoiceLinesDto>(queryLines).ToList();

            purchaseInvoices.SelectPurchaseInvoiceLinesDto = purchaseInvoiceLine;

            LogsAppService.InsertLogToDatabase(purchaseInvoices, purchaseInvoices, LoginedUserService.UserId, Tables.PurchaseInvoices, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseInvoicesDto>(purchaseInvoices);

        }

        public async Task<IDataResult<IList<ListPurchaseInvoicesDto>>> GetListAsync(ListPurchaseInvoicesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.PurchaseInvoices)
                   .Select<PurchaseInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseInvoices.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                       .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseInvoices.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseInvoices.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PurchaseInvoices);

            var purchaseInvoices = queryFactory.GetList<ListPurchaseInvoicesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseInvoicesDto>>(purchaseInvoices);

        }


        public async Task<IDataResult<IList<SelectPurchaseInvoiceLinesDto>>> GetLineListAsync()
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseInvoiceLines)
                   .Select<PurchaseInvoiceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseInvoiceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseInvoiceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseInvoiceLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoiceLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseInvoiceLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoiceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PurchaseInvoiceLines);

            var purchaseInvoiceLine = queryFactory.GetList<SelectPurchaseInvoiceLinesDto>(queryLines).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<List<SelectPurchaseInvoiceLinesDto>>(purchaseInvoiceLine);
        }


        [ValidationAspect(typeof(UpdatePurchaseInvoicesValidator), Priority = 1)]
        public async Task<IDataResult<SelectPurchaseInvoicesDto>> UpdateAsync(UpdatePurchaseInvoicesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseInvoices)
                   .Select<PurchaseInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseInvoices.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseInvoices.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseInvoices);

            var entity = queryFactory.Get<SelectPurchaseInvoicesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseInvoiceLines)
                   .Select<PurchaseInvoiceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseInvoiceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseInvoiceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseInvoiceLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoiceLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseInvoiceLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoiceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseInvoiceID = input.Id }, Tables.PurchaseInvoiceLines);

            var purchaseInvoiceLine = queryFactory.GetList<SelectPurchaseInvoiceLinesDto>(queryLines).ToList();

            entity.SelectPurchaseInvoiceLinesDto = purchaseInvoiceLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseInvoices)
                   .Select<PurchaseInvoices>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseInvoices.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseInvoices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseInvoices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseInvoices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseInvoices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseInvoices.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseInvoices);

            var list = queryFactory.GetList<ListPurchaseInvoicesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseInvoices).Update(new UpdatePurchaseInvoicesDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseInvoiceWayBillStatusEnum = input.PurchaseInvoiceWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseInvoiceState = input.PurchaseInvoiceState,
                ShippingAdressID = input.ShippingAdressID.GetValueOrDefault(),
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
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
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                PriceApprovalState = input.PriceApprovalState,
                PricingCurrency = input.PricingCurrency
            }).Where(new { Id = input.Id }, "");

            DateTime biggestDate = input.SelectPurchaseInvoiceLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseInvoiceLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Insert(new CreatePurchaseInvoiceLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        WorkOrderCreationDate = item.WorkOrderCreationDate,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LikedPurchaseRequestLineID = item.LikedPurchaseRequestLineID.GetValueOrDefault(),
                        LineAmount = item.LineAmount,
                        TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                        LineDescription = item.LineDescription,
                        PurchaseInvoiceLineWayBillStatusEnum = item.PurchaseInvoiceLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PartyNo = item.PartyNo,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        PurchaseOrderLineID = item.PurchaseOrderLineID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseInvoiceLineStateEnum = item.PurchaseInvoiceLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseInvoiceID = input.Id,
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
                        SupplierReferenceNo = item.SupplierReferenceNo
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;


                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseInvoiceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseInvoiceLines).Update(new UpdatePurchaseInvoiceLinesDto
                        {
                            DiscountAmount = item.DiscountAmount,
                            WorkOrderCreationDate = item.WorkOrderCreationDate,
                            DiscountRate = item.DiscountRate,
                            ExchangeRate = item.ExchangeRate,
                            LikedPurchaseRequestLineID = item.LikedPurchaseRequestLineID.GetValueOrDefault(),
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                            OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                            LineTotalAmount = item.LineTotalAmount,
                            LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                            PurchaseOrderLineID = item.PurchaseOrderLineID.GetValueOrDefault(),
                            PaymentPlanID = item.PaymentPlanID,
                            PartyNo = item.PartyNo,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseInvoiceLineWayBillStatusEnum = item.PurchaseInvoiceLineWayBillStatusEnum,
                            PurchaseInvoiceLineStateEnum = item.PurchaseInvoiceLineStateEnum,
                            UnitPrice = item.UnitPrice,
                            TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                            SupplyDate = item.SupplyDate,
                            SupplierBillNo = item.SupplierBillNo,
                            SupplierWaybillNo = item.SupplierWaybillNo,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            PurchaseInvoiceID = input.Id,
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
                            BranchID = input.BranchID.GetValueOrDefault(),
                            CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                            WarehouseID = input.WarehouseID.GetValueOrDefault(),
                            Date_ = input.Date_,
                            SupplierReferenceNo = item.SupplierReferenceNo
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;


                    }
                }
            }

            var purchaseInvoice = queryFactory.Update<SelectPurchaseInvoicesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseInvoices, LogType.Update, purchaseInvoice.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseInvoicesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPurchaseInvoicesDto>(purchaseInvoice);

        }


        public async Task<IDataResult<SelectPurchaseInvoicesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseInvoices).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PurchaseInvoices>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseInvoices).Update(new UpdatePurchaseInvoicesDto
            {
                FicheNo = entity.FicheNo,
                BranchID = entity.BranchID,
                CurrencyID = entity.CurrencyID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
                ExchangeRate = entity.ExchangeRate,
                TransactionExchangeTotalVatExcludedAmount = entity.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = entity.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = entity.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = entity.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = entity.TransactionExchangeGrossAmount,
                OrderAcceptanceID = entity.OrderAcceptanceID.GetValueOrDefault(),
                PurchaseOrderID = entity.PurchaseOrderID,
                TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                GrossAmount = entity.GrossAmount,
                MaintenanceMRPID = entity.MaintenanceMRPID,
                PurchaseInvoiceWayBillStatusEnum = entity.PurchaseInvoiceWayBillStatusEnum,
                MRPID = entity.MRPID,
                LinkedPurchaseRequestID = entity.LinkedPurchaseRequestID,
                NetAmount = entity.NetAmount,
                PaymentPlanID = entity.PaymentPlanID,
                ProductionOrderID = entity.ProductionOrderID,
                PurchaseInvoiceState = entity.PurchaseInvoiceState,
                ShippingAdressID = entity.ShippingAdressID,
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
                PriceApprovalState = entity.PriceApprovalState,
                PricingCurrency = (int)entity.PricingCurrency
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var purchaseInvoicesDto = queryFactory.Update<SelectPurchaseInvoicesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseInvoicesDto>(purchaseInvoicesDto);


        }
    }
}
