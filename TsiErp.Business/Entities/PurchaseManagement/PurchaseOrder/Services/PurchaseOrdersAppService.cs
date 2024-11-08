using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Localization;
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
using TsiErp.Business.Entities.OrderAcceptanceRecord.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ProductReceiptTransaction.Services;
using TsiErp.Business.Entities.PurchaseOrder.Validations;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseOrders.Page;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    [ServiceRegistration(typeof(IPurchaseOrdersAppService), DependencyInjectionType.Scoped)]
    public class PurchaseOrdersAppService : ApplicationService<PurchaseOrdersResource>, IPurchaseOrdersAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();


        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;
        private readonly IOrderAcceptanceRecordsAppService _OrderAcceptanceRecordsAppService;
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IProductReceiptTransactionsAppService _ProductReceiptTransactionsAppService;
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }


        public PurchaseOrdersAppService(IStringLocalizer<PurchaseOrdersResource> l, IPurchaseRequestsAppService PurchaseRequestsAppService, IFicheNumbersAppService ficheNumbersAppService, IOrderAcceptanceRecordsAppService orderAcceptanceRecordsAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IProductReceiptTransactionsAppService productReceiptTransactionsAppService) : base(l)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _OrderAcceptanceRecordsAppService = orderAcceptanceRecordsAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _ProductReceiptTransactionsAppService = productReceiptTransactionsAppService;
        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> CreateAsync(CreatePurchaseOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Insert(new CreatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
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
                PurchaseOrderWayBillStatusEnum = 1,
                NetAmount = input.NetAmount,
                MRPID = input.MRPID.GetValueOrDefault(),
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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
                PricingCurrency = input.PricingCurrency
            });

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                    PurchaseOrderLineWayBillStatusEnum = 1,
                    PartyNo = item.PartyNo,
                    LineDescription = item.LineDescription,
                    SupplierBillNo = item.SupplierBillNo,
                    SupplierWaybillNo = item.SupplierWaybillNo,
                    LineTotalAmount = item.LineTotalAmount,
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    LinkedPurchaseRequestID = Guid.Empty,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                    SupplyDate = item.SupplyDate,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    PurchaseOrderID = addedEntityId,
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

                if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                {
                    if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                    {
                        await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), biggestDate);
                    }
                }
            }

            var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

            StockMovementsService.InsertPurchaseOrders(input);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseOrdersChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseOrdersChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Insert(new CreatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = 1,
                MRPID = input.MRPID.GetValueOrDefault(),
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
                ShippingAdressID = Guid.Empty,
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
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
                PricingCurrency = input.PricingCurrency
            });

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
                {
                    DiscountAmount = item.DiscountAmount,
                    WorkOrderCreationDate = input.WorkOrderCreationDate,
                    DiscountRate = item.DiscountRate,
                    ExchangeRate = item.ExchangeRate,
                    LikedPurchaseRequestLineID = Guid.Empty,
                    LineAmount = item.LineAmount,
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    PurchaseOrderLineWayBillStatusEnum = 1,
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    LineDescription = item.LineDescription,
                    PartyNo = item.PartyNo,
                    SupplyDate = item.SupplyDate,
                    WaitingQuantity = item.WaitingQuantity,
                    PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                    LineTotalAmount = item.LineTotalAmount,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    SupplierBillNo = item.SupplierBillNo,
                    SupplierWaybillNo = item.SupplierWaybillNo,
                    LinkedPurchaseRequestID = Guid.Empty,
                    PaymentPlanID = item.PaymentPlanID,
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    PurchaseOrderID = addedEntityId,
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
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID,
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_,
                    SupplierReferenceNo = item.SupplierReferenceNo
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

            await _PurchaseRequestsAppService.UpdatePurchaseRequestLineState(input.SelectPurchaseOrderLinesDto, TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, purchaseOrder.Id);


            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseOrdersChildMenu", input.FicheNo);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseRequestsChildMenu"], L["PurchaseRequestContextConverttoOrder"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseRequestContextConverttoOrder"],
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
                            ContextMenuName_ = L["PurchaseRequestContextConverttoOrder"],
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

            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderMRPAsync(CreatePurchaseOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Insert(new CreatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = 1,
                MRPID = input.MRPID.GetValueOrDefault(),
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
                ShippingAdressID = Guid.Empty,
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
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
                PricingCurrency = input.PricingCurrency
            });

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
                {
                    DiscountAmount = item.DiscountAmount,
                    WorkOrderCreationDate = input.WorkOrderCreationDate,
                    DiscountRate = item.DiscountRate,
                    ExchangeRate = item.ExchangeRate,
                    PartyNo = item.PartyNo,
                    LikedPurchaseRequestLineID = Guid.Empty,
                    LineAmount = item.LineAmount,
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    PurchaseOrderLineWayBillStatusEnum = 1,
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    LineDescription = item.LineDescription,
                    SupplyDate = item.SupplyDate,
                    WaitingQuantity = item.WaitingQuantity,
                    PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                    LineTotalAmount = item.LineTotalAmount,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    SupplierBillNo = item.SupplierBillNo,
                    SupplierWaybillNo = item.SupplierWaybillNo,
                    LinkedPurchaseRequestID = Guid.Empty,
                    PaymentPlanID = item.PaymentPlanID,
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    PurchaseOrderID = addedEntityId,
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
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID,
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_,
                    SupplierReferenceNo = item.SupplierReferenceNo
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseOrdersChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, purchaseOrder.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["MRPChildMenu"], L["MRPContextConvertPurchase"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["MRPContextConvertPurchase"],
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
                            ContextMenuName_ = L["MRPContextConvertPurchase"],
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

            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("OrderID", new List<string>
            {
                Tables.PurchaseUnsuitabilityReports
            });

            DeleteControl.ControlList.Add("PurchaseOrderID", new List<string>
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
                var query = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { Id = id }, "");

                var purchaseOrders = queryFactory.Get<SelectPurchaseOrdersDto>(query);

                if (purchaseOrders.Id != Guid.Empty && purchaseOrders != null)
                {
                    StockMovementsService.DeletePurchaseOrders(purchaseOrders);

                    var deleteQuery = queryFactory.Query().From(Tables.PurchaseOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Delete(LoginedUserService.UserId).Where(new { PurchaseOrderID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Delete, id);

                    foreach(var line in entity.SelectPurchaseOrderLinesDto)
                    {
                        var productReceipt = (await _ProductReceiptTransactionsAppService.GetbyPurchaseOrderLineIDAsync(line.Id)).Data;

                        if(productReceipt != null && productReceipt.Id != Guid.Empty)
                        {
                            await _ProductReceiptTransactionsAppService.DeletebyPurchaseOrderLineIDAsync (line.Id);
                        }

                    }

                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseOrdersChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                    return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);
                }
                else
                {
                    var queryLineGet = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = id }, "");

                    var purchaseOrdersLineGet = queryFactory.Get<SelectPurchaseOrderLinesDto>(queryLineGet);

                    StockMovementsService.DeletePurchaseOrderLines(purchaseOrdersLineGet);

                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var purchaseOrderLines = queryFactory.Update<SelectPurchaseOrderLinesDto>(queryLine, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseOrderLines, LogType.Delete, id);

                    var productReceipt = (await _ProductReceiptTransactionsAppService.GetbyPurchaseOrderLineIDAsync(id)).Data;

                    if (productReceipt != null && productReceipt.Id != Guid.Empty)
                    {
                        await _ProductReceiptTransactionsAppService.DeletebyPurchaseOrderLineIDAsync(id);
                    }

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectPurchaseOrderLinesDto>(purchaseOrderLines);
                }
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrders.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.PurchaseOrders);

            var purchaseOrders = queryFactory.Get<SelectPurchaseOrdersDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrderLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            purchaseOrders.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            LogsAppService.InsertLogToDatabase(purchaseOrders, purchaseOrders, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrders);

        }

        public async Task<IDataResult<IList<ListPurchaseOrdersDto>>> GetListAsync(ListPurchaseOrdersParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                       .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrders.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PurchaseOrders);

            var purchaseOrders = queryFactory.GetList<ListPurchaseOrdersDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseOrdersDto>>(purchaseOrders);

        }


        public decimal LastPurchasePrice(Guid productId)
        {
            var query = queryFactory
                   .Query()
                     .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrderLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderLineStateEnum  = 3, ProductID = productId}, Tables.PurchaseOrderLines);

            var purchaseOrderLines = queryFactory.GetList<SelectPurchaseOrderLinesDto>(query).ToList();

            decimal lastPurchasePrice = purchaseOrderLines.Select(t => t.UnitPrice).LastOrDefault();

            return lastPurchasePrice;

        }

        public decimal HighestPurchasePrice(Guid productId)
        {
            var query = queryFactory
                   .Query()
                     .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrderLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderLineStateEnum = 3, ProductID = productId }, Tables.PurchaseOrderLines);

            var purchaseOrderLines = queryFactory.GetList<SelectPurchaseOrderLinesDto>(query).ToList();

            decimal highestPurchasePrice = purchaseOrderLines.OrderByDescending(t => t.UnitPrice).Select(t => t.UnitPrice).FirstOrDefault();

            return highestPurchasePrice;

        }

        public decimal LowestPurchasePrice(Guid productId)
        {
            var query = queryFactory
                   .Query()
                     .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrderLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderLineStateEnum = 3, ProductID = productId }, Tables.PurchaseOrderLines);

            var purchaseOrderLines = queryFactory.GetList<SelectPurchaseOrderLinesDto>(query).ToList();

            decimal lowestPurchasePrice = purchaseOrderLines.OrderByDescending(t => t.UnitPrice).Select(t => t.UnitPrice).LastOrDefault();

            return lowestPurchasePrice;

        }

        public decimal AveragePurchasePrice(Guid productId)
        {
            var query = queryFactory
                   .Query()
                     .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseOrderLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderLineStateEnum = 3, ProductID = productId }, Tables.PurchaseOrderLines);

            var purchaseOrderLines = queryFactory.GetList<SelectPurchaseOrderLinesDto>(query).ToList();

            decimal averagePurchasePrice = purchaseOrderLines.Average(t=>t.UnitPrice);

            return averagePurchasePrice;

        }




        [ValidationAspect(typeof(UpdatePurchaseOrdersValidator), Priority = 1)]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrders);

            var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = input.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            #region Depo Onaya Göre İrsaliye Durumu Belirleme

            //if (input.SelectPurchaseOrderLinesDto != null && input.SelectPurchaseOrderLinesDto.Count > 0)
            //{
            //    if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Onaylandi || t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 1;
            //    }
            //    else if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Beklemede && t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 3;
            //    }
            //    else
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 2;
            //    }
            //}

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = input.PurchaseOrderWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                        PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PartyNo = item.PartyNo,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = input.Id,
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

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
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
                            PaymentPlanID = item.PaymentPlanID,
                            PartyNo = item.PartyNo,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
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
                            PurchaseOrderID = input.Id,
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

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            StockMovementsService.UpdatePurchaseOrders(entity, input);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseOrdersChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateApproveOrderAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrders);

            var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = input.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            #region Depo Onaya Göre İrsaliye Durumu Belirleme

            //if (input.SelectPurchaseOrderLinesDto != null && input.SelectPurchaseOrderLinesDto.Count > 0)
            //{
            //    if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Onaylandi || t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 1;
            //    }
            //    else if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Beklemede && t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 3;
            //    }
            //    else
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 2;
            //    }
            //}

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = input.PurchaseOrderWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                        PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PartyNo = item.PartyNo,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = input.Id,
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

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
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
                            PaymentPlanID = item.PaymentPlanID,
                            PartyNo = item.PartyNo,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
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
                            PurchaseOrderID = input.Id,
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

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseOrdersChildMenu"], L["PurchaseOrderContextApproveOrder"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseOrderContextApproveOrder"],
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
                            ContextMenuName_ = L["PurchaseOrderContextApproveOrder"],
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
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateApproveBillAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrders);

            var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = input.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            #region Depo Onaya Göre İrsaliye Durumu Belirleme

            //if (input.SelectPurchaseOrderLinesDto != null && input.SelectPurchaseOrderLinesDto.Count > 0)
            //{
            //    if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Onaylandi || t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 1;
            //    }
            //    else if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Beklemede && t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 3;
            //    }
            //    else
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 2;
            //    }
            //}

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = input.PurchaseOrderWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                        PartyNo = item.PartyNo,
                        PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = input.Id,
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

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
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
                            PaymentPlanID = item.PaymentPlanID,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
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
                            PartyNo = item.PartyNo,
                            VATrate = item.VATrate,
                            PurchaseOrderID = input.Id,
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

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseOrdersChildMenu"], L["PurchaseOrderContextPriceApproval"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseOrderContextPriceApproval"],
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
                            ContextMenuName_ = L["PurchaseOrderContextPriceApproval"],
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
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateApproveWayBillAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrders);

            var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = input.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            #region Depo Onaya Göre İrsaliye Durumu Belirleme

            //if (input.SelectPurchaseOrderLinesDto != null && input.SelectPurchaseOrderLinesDto.Count > 0)
            //{
            //    if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Onaylandi || t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 1;
            //    }
            //    else if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Beklemede && t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 3;
            //    }
            //    else
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 2;
            //    }
            //}

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = input.PurchaseOrderWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                        PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        PartyNo = item.PartyNo,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = input.Id,
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

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
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
                            PaymentPlanID = item.PaymentPlanID,
                            PartyNo = item.PartyNo,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
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
                            PurchaseOrderID = input.Id,
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

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseOrdersChildMenu"], L["PurchaseOrderContextWayBillApproval"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseOrderContextWayBillApproval"],
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
                            ContextMenuName_ = L["PurchaseOrderContextWayBillApproval"],
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
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateCancelOrderAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrders);

            var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = input.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            #region Depo Onaya Göre İrsaliye Durumu Belirleme

            //if (input.SelectPurchaseOrderLinesDto != null && input.SelectPurchaseOrderLinesDto.Count > 0)
            //{
            //    if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Onaylandi || t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 1;
            //    }
            //    else if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Beklemede && t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 3;
            //    }
            //    else
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 2;
            //    }
            //}

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = input.PurchaseOrderWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                        PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        PartyNo = item.PartyNo,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = input.Id,
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

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
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
                            PaymentPlanID = item.PaymentPlanID,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
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
                            PurchaseOrderID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            PartyNo = item.PartyNo,
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

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseOrdersChildMenu"], L["PurchaseOrderContextCancelOrder"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseOrderContextCancelOrder"],
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
                            ContextMenuName_ = L["PurchaseOrderContextCancelOrder"],
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
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateOrderCreateStockFichesAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseOrders.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrders);

            var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        "PaymentPlanLine",
                        JoinType.Left
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = input.Id }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchaseOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseOrders.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrders.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left)

                         .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                        nameof(PurchaseOrders.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            #region Depo Onaya Göre İrsaliye Durumu Belirleme

            //if (input.SelectPurchaseOrderLinesDto != null && input.SelectPurchaseOrderLinesDto.Count > 0)
            //{
            //    if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Onaylandi || t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 1;
            //    }
            //    else if (!input.SelectPurchaseOrderLinesDto.Any(t => t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.Beklemede && t.PurchaseOrderLineWayBillStatusEnum == PurchaseOrderLineWayBillStatusEnum.KismiOnaylandi))
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 3;
            //    }
            //    else
            //    {
            //        input.PurchaseOrderWayBillStatusEnum = 2;
            //    }
            //}

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PurchaseOrderWayBillStatusEnum = input.PurchaseOrderWayBillStatusEnum,
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
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

            DateTime biggestDate = input.SelectPurchaseOrderLinesDto.Select(t => t.SupplyDate).Max().GetValueOrDefault();

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
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
                        PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                        LineTotalAmount = item.LineTotalAmount,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        WaitingQuantity = item.WaitingQuantity,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        PartyNo = item.PartyNo,
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
                        UnitPrice = item.UnitPrice,
                        SupplierBillNo = item.SupplierBillNo,
                        SupplierWaybillNo = item.SupplierWaybillNo,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = input.Id,
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

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
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
                            PaymentPlanID = item.PaymentPlanID,
                            WaitingQuantity = item.WaitingQuantity,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineWayBillStatusEnum = (int)item.PurchaseOrderLineWayBillStatusEnum,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                            UnitPrice = item.UnitPrice,
                            TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            PartyNo = item.PartyNo,
                            TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                            SupplyDate = item.SupplyDate,
                            SupplierBillNo = item.SupplierBillNo,
                            SupplierWaybillNo = item.SupplierWaybillNo,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            PurchaseOrderID = input.Id,
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

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseOrdersChildMenu"], L["PurchaseOrderContextCreateStockFiches"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseOrderContextCreateStockFiches"],
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
                            ContextMenuName_ = L["PurchaseOrderContextCreateStockFiches"],
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
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PurchaseOrders>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
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
                TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                GrossAmount = entity.GrossAmount,
                MaintenanceMRPID = entity.MaintenanceMRPID,
                PurchaseOrderWayBillStatusEnum = (int)entity.PurchaseOrderWayBillStatusEnum,
                MRPID = entity.MRPID,
                LinkedPurchaseRequestID = entity.LinkedPurchaseRequestID,
                NetAmount = entity.NetAmount,
                PaymentPlanID = entity.PaymentPlanID,
                ProductionOrderID = entity.ProductionOrderID,
                PurchaseOrderState = (int)entity.PurchaseOrderState,
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
                PriceApprovalState = (int)entity.PriceApprovalState,
                PricingCurrency = (int)entity.PricingCurrency
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var purchaseOrdersDto = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrdersDto);


        }

        public async Task<IDataResult<IList<SelectPurchaseOrderLinesDto>>> GetLineListAsync()
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, " ");

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<List<SelectPurchaseOrderLinesDto>>(purchaseOrderLine);
        }

        public async Task<IDataResult<SelectPurchaseOrderLinesDto>> GetLinebyProductandProductionOrderAsync(Guid productId, Guid prodeuctionOrderId)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrderLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseOrderLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
            JoinType.Left
            )
                    .Where(new { ProductID = productId, ProductionOrderID = prodeuctionOrderId, PurchaseOrderLineStateEnum = PurchaseOrderStateEnum.Beklemede }, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.Get<SelectPurchaseOrderLinesDto>(queryLines);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrderLinesDto>(purchaseOrderLine);
        }
    }
}
