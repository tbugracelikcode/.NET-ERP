using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.SplitButtons;
using Syncfusion.XlsIO;
using System.Data;
using System.Dynamic;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.OrderAcceptanceRecord
{
    public partial class OrderAcceptanceRecordsListPage : IDisposable
    {
        private SfGrid<VirtualLineModel> _VirtualLineGrid;
        private SfGrid<OrderLineModel> _OrderLineGrid;
        private SfGrid<SelectOrderAcceptanceRecordLinesDto> _ConvertToOrderGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private SfGrid<SelectMRPLinesDto> _MRPLineGrid;

        VirtualLineModel VirtualLineDataSource;

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }
        public List<ContextMenuItemModel> MRPLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<VirtualLineModel> GridVirtualLineList = new List<VirtualLineModel>();

        List<OrderLineModel> GridOrderLineList = new List<OrderLineModel>();

        List<SelectOrderAcceptanceRecordLinesDto> GridConvertToOrderList = new List<SelectOrderAcceptanceRecordLinesDto>();

        List<SelectOrderAcceptanceRecordLinesDto> SelectedToOrderList = new List<SelectOrderAcceptanceRecordLinesDto>();

        List<ListSalesOrderDto> SalesOrdersList = new List<ListSalesOrderDto>();

        public List<SelectMRPLinesDto> MRPLinesList = new List<SelectMRPLinesDto>();

        public List<ListProductReferanceNumbersDto> ProductReferanceNumbersList = new List<ListProductReferanceNumbersDto>();

        SelectMRPsDto MRPDataSource;
        SelectMRPLinesDto MRPLineDataSource;

        public bool MRPCrudModalVisible = false;
        public bool MRPLineCrudPopup = false;
        public bool OrderAcceptanceControlPopup = false;
        private bool VirtualLineCrudPopup = false;
        private bool OrderLineCrudPopup = false;
        private bool ConvertToOrderCrudPopup = false;

        SfProgressButton ProgressBtn;
        bool HideCreateProductionOrderPopupButtonDisabled = false;
        bool LoadingModalVisibility = false;

        #region Planning Parameters

        Guid? BranchIDParameter;
        Guid? WarehouseIDParameter;
        Guid? BranchIDButtonEdit;
        Guid? WarehouseIDButtonEdit;
        string WarehouseCodeButtonEdit;
        string BranchCodeButtonEdit;

        #endregion

        List<Models.SupplierSelectionGrid> SupplierSelectionList = new List<Models.SupplierSelectionGrid>();

        public bool SupplierSelectionPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = OrderAcceptanceRecordsAppService;
            _L = L;


            var purchaseParameter = (await PurchaseManagementParametersAppService.GetPurchaseManagementParametersAsync()).Data;
            BranchIDParameter = purchaseParameter.BranchID;
            WarehouseIDParameter = purchaseParameter.WarehouseID;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "OrderAcceptanceRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateMRPLineContextMenuItems();

        }

        #region Satışa Dönüştürme Modalı İşlemleri

        protected async Task OnConvertToOrderBtnClicked()
        {
            if (DataSource.SelectOrderAcceptanceRecordLines.Count > 0)
            {
                SpinnerService.Show();
                await Task.Delay(100);

                #region Sipariş Insert

                var shippingaddressID = (await ShippingAdressesAppService.GetListAsync(new ListShippingAdressesParameterDto())).Data.Where(t => t.CustomerCardName == DataSource.CurrentAccountCardName).Select(t => t.Id).FirstOrDefault();

                var localCurrency = (await CurrentAccountCardsAppService.GetAsync(DataSource.CurrentAccountCardID.GetValueOrDefault())).Data.CurrencyID.GetValueOrDefault();


                Guid branchID = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data.DefaultBranchID;
                Guid warehouseID = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data.DefaultWarehouseID;

                var localCurrencyDataSource = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.Where(t => t.IsLocalCurrency).FirstOrDefault();

                #region Fiyatlandırma Dövizi Combobox Set

                int pricing = 1;

                if (DataSource.CurrenyID != localCurrencyDataSource.Id)
                {
                    pricing = 2;
                }
                else
                {
                    pricing = 1;
                }
                #endregion



                CreateSalesOrderDto createdSalesOrderEntity = new CreateSalesOrderDto
                {
                    BranchID = branchID,
                    CurrencyID = localCurrency,
                    CurrentAccountCardID = DataSource.CurrentAccountCardID,
                    CustomerRequestedDate = DataSource.CustomerRequestedDate,
                    OrderAcceptanceRecordID = DataSource.Id,
                    CustomerOrderNr = DataSource.CustomerOrderNo,
                    Date_ = GetSQLDateAppService.GetDateFromSQL(),
                    Description_ = string.Empty,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesOrdersChildMenu"),
                    WorkOrderCreationDate = null,
                    WarehouseID = warehouseID,
                    TransactionExchangeCurrencyID = DataSource.CurrenyID,
                    TransactionExchangeGrossAmount = 0,
                    TransactionExchangeNetAmount = 0,
                    TransactionExchangeTotalDiscountAmount = 0,
                    TransactionExchangeTotalVatAmount = 0,
                    TransactionExchangeTotalVatExcludedAmount = 0,
                    PricingCurrency = pricing,
                    TotalVatExcludedAmount = 0,
                    TotalVatAmount = 0,
                    TotalDiscountAmount = 0,
                    Time_ = string.Empty,
                    SpecialCode = string.Empty,
                    ShippingAdressID = shippingaddressID,
                    SalesOrderState = 1,
                    PaymentPlanID = DataSource.PaymentPlanID,
                    NetAmount = 0,
                    LinkedSalesPropositionID = Guid.Empty,
                    GrossAmount = 0,
                    ExchangeRate = DataSource.ExchangeRateAmount,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = Guid.Empty,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                     ConfirmedLoadingDate = DataSource.ConfirmedLoadingDate


                };

                createdSalesOrderEntity.SelectSalesOrderLines = new List<SelectSalesOrderLinesDto>();

                foreach (var line in DataSource.SelectOrderAcceptanceRecordLines)
                {
                    if (localCurrencyDataSource != null && localCurrencyDataSource.Id != Guid.Empty)
                    {
                        if (DataSource.CurrenyID != localCurrencyDataSource.Id) // döviz
                        {
                            SelectSalesOrderLinesDto createdSalesOrderLine = new SelectSalesOrderLinesDto
                            {
                                ExchangeRate = DataSource.ExchangeRateAmount,
                                LinkedSalesPropositionID = Guid.Empty,
                                DiscountAmount = 0,
                                DiscountRate = 0,
                                LikedPropositionLineID = Guid.Empty,
                                CurrentAccountCardCode = DataSource.CurrentAccountCardCode,
                                CurrentAccountCardID = DataSource.CurrentAccountCardID.GetValueOrDefault(),
                                CurrentAccountCardName = DataSource.CurrentAccountCardName,
                                OrderAcceptanceRecordID = DataSource.Id,
                                OrderAcceptanceRecordLineID = line.Id,
                                PurchaseSupplyDate = line.PurchaseSupplyDate,
                                Date_ = DataSource.Date_,
                                TransactionExchangeDiscountAmount = 0,
                                TransactionExchangeLineAmount = line.LineAmount,
                                TransactionExchangeLineTotalAmount = (line.LineAmount + ((line.LineAmount * line.VATrate) / 100)),
                                TransactionExchangeUnitPrice = line.OrderUnitPrice,
                                TransactionExchangeVATamount = ((line.LineAmount * line.VATrate) / 100),
                                WarehouseID = warehouseID,
                                BranchID = branchID,
                                BranchCode = string.Empty,
                                BranchName = string.Empty,
                                WarehouseCode = string.Empty,
                                WarehouseName = string.Empty,
                                LineAmount = line.LineAmount * DataSource.ExchangeRateAmount,
                                LineDescription = line.Description_,
                                LineNr = line.LineNr,
                                LineTotalAmount = (line.LineAmount + ((line.LineAmount * line.VATrate) / 100)) * DataSource.ExchangeRateAmount,
                                PaymentPlanID = DataSource.PaymentPlanID,
                                PaymentPlanName = DataSource.PaymentPlanName,
                                WorkOrderCreationDate = null,
                                VATrate = line.VATrate,
                                VATamount = ((line.LineAmount * line.VATrate) / 100) * DataSource.ExchangeRateAmount,
                                UnitSetID = line.UnitSetID,
                                UnitSetCode = line.UnitSetCode,
                                UnitPrice = line.OrderUnitPrice * DataSource.ExchangeRateAmount,
                                SalesOrderLineStateEnum = Entities.Enums.SalesOrderLineStateEnum.Beklemede,
                                SalesOrderID = Guid.Empty,
                                Quantity = line.OrderAmount,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                ProductCode = line.ProductCode,
                                CreationTime = DateTime.Now,
                                CreatorId = LoginedUserService.UserId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = Guid.Empty,
                                DeletionTime = null,
                                Id = Guid.Empty,
                                IsDeleted = false,
                                LastModificationTime = null,
                                LastModifierId = Guid.Empty,
                            };

                            createdSalesOrderEntity.SelectSalesOrderLines.Add(createdSalesOrderLine);
                        }
                        else // yerel veya boş
                        {
                            SelectSalesOrderLinesDto createdSalesOrderLine = new SelectSalesOrderLinesDto
                            {
                                ExchangeRate = DataSource.ExchangeRateAmount,
                                LinkedSalesPropositionID = Guid.Empty,
                                DiscountAmount = 0,
                                DiscountRate = 0,
                                LikedPropositionLineID = Guid.Empty,
                                CurrentAccountCardCode = DataSource.CurrentAccountCardCode,
                                CurrentAccountCardID = DataSource.CurrentAccountCardID.GetValueOrDefault(),
                                CurrentAccountCardName = DataSource.CurrentAccountCardName,
                                OrderAcceptanceRecordID = DataSource.Id,
                                OrderAcceptanceRecordLineID = line.Id,
                                PurchaseSupplyDate = line.PurchaseSupplyDate,
                                Date_ = DataSource.Date_,
                                //TransactionExchangeDiscountAmount = 0,
                                //TransactionExchangeLineAmount = 0,
                                //TransactionExchangeLineTotalAmount = 0,
                                //TransactionExchangeUnitPrice = 0,
                                //TransactionExchangeVATamount = 0,
                                WarehouseID = warehouseID,
                                BranchID = branchID,
                                BranchCode = string.Empty,
                                BranchName = string.Empty,
                                WarehouseCode = string.Empty,
                                WarehouseName = string.Empty,
                                LineAmount = line.LineAmount,
                                LineDescription = line.Description_,
                                LineNr = line.LineNr,
                                LineTotalAmount = line.LineAmount + ((line.LineAmount * line.VATrate) / 100),
                                PaymentPlanID = DataSource.PaymentPlanID,
                                PaymentPlanName = DataSource.PaymentPlanName,
                                WorkOrderCreationDate = null,
                                VATrate = line.VATrate,
                                VATamount = (line.LineAmount * line.VATrate) / 100,
                                UnitSetID = line.UnitSetID,
                                UnitSetCode = line.UnitSetCode,
                                UnitPrice = line.OrderUnitPrice,
                                SalesOrderLineStateEnum = Entities.Enums.SalesOrderLineStateEnum.Beklemede,
                                SalesOrderID = Guid.Empty,
                                Quantity = line.OrderAmount,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                ProductCode = line.ProductCode,
                                CreationTime = DateTime.Now,
                                CreatorId = LoginedUserService.UserId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = Guid.Empty,
                                DeletionTime = null,
                                Id = Guid.Empty,
                                IsDeleted = false,
                                LastModificationTime = null,
                                LastModifierId = Guid.Empty,
                            };

                            createdSalesOrderEntity.SelectSalesOrderLines.Add(createdSalesOrderLine);
                        }
                    }




                }

                #region Toplam Alanlarını Set Etme

                createdSalesOrderEntity.TransactionExchangeGrossAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.TransactionExchangeLineAmount) + createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.TransactionExchangeDiscountAmount);

                createdSalesOrderEntity.TransactionExchangeNetAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.TransactionExchangeLineTotalAmount);

                createdSalesOrderEntity.TransactionExchangeTotalVatAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.TransactionExchangeVATamount);

                createdSalesOrderEntity.TransactionExchangeTotalVatExcludedAmount = createdSalesOrderEntity.TransactionExchangeGrossAmount - createdSalesOrderEntity.TransactionExchangeTotalDiscountAmount;

                createdSalesOrderEntity.GrossAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.LineAmount) + createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.DiscountAmount);
                createdSalesOrderEntity.TotalDiscountAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.DiscountAmount);
                createdSalesOrderEntity.TotalVatExcludedAmount = createdSalesOrderEntity.GrossAmount - createdSalesOrderEntity.TotalDiscountAmount;
                createdSalesOrderEntity.TotalVatAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.VATamount);
                createdSalesOrderEntity.NetAmount = createdSalesOrderEntity.SelectSalesOrderLines.Sum(x => x.LineTotalAmount);

                #endregion

                await SalesOrdersAppService.CreateAsync(createdSalesOrderEntity);

                #endregion

                #region Sipariş Kabul Durum Güncelleme
                DataSource.OrderAcceptanceRecordState = Entities.Enums.OrderAcceptanceRecordStateEnum.SiparisOlusturuldu;

                var updatedDataSource = ObjectMapper.Map<SelectOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto>(DataSource);

                await OrderAcceptanceRecordsAppService.UpdateAcceptanceOrderAsync(updatedDataSource);

                #endregion

                SpinnerService.Hide();
                await ModalManager.MessagePopupAsync(L["UIMessageConvertTitle"], L["UIMessageConvertMessage"]);

                //HideCreateProductionOrderPopupButtonDisabled = false;
                //await ProgressBtn.EndProgressAsync();

                await GetListDataSourceAsync();

                await _grid.Refresh();

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIWarningConvertTitle2"], L["UIWarningConvertMessage2"]);
            }

        }

        public void HideConvertToOrderPopup()
        {
            ConvertToOrderCrudPopup = false;
        }

        #endregion

        #region Sipariş Kabul Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOrderAcceptanceRecordsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                ConfirmedLoadingDate = GetSQLDateAppService.GetDateFromSQL().Date,
                CustomerRequestedDate = GetSQLDateAppService.GetDateFromSQL().Date,
                ProductionOrderLoadingDate = GetSQLDateAppService.GetDateFromSQL().Date,
                OrderAcceptanceRecordState = Entities.Enums.OrderAcceptanceRecordStateEnum.Beklemede,
                Code = FicheNumbersAppService.GetFicheNumberAsync("OrderAcceptanceRecordsChildMenu"),

            };

            DataSource.SelectOrderAcceptanceRecordLines = new List<SelectOrderAcceptanceRecordLinesDto>();
            GridVirtualLineList = new List<VirtualLineModel>();

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "OrderAcceptanceRecordLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextAdd"], Id = "new" }); break;
                            case "OrderAcceptanceRecordLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextChange"], Id = "changed" }); break;
                            case "OrderAcceptanceRecordLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextDelete"], Id = "delete" }); break;
                            case "OrderAcceptanceRecordLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "OrderAcceptanceRecordsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextAdd"], Id = "new" }); break;
                            case "OrderAcceptanceRecordsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextChange"], Id = "changed" }); break;
                            case "OrderAcceptanceRecordsContextConverttoOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextConverttoOrder"], Id = "convertorder" }); break;
                            case "OrderAcceptanceRecordsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextDelete"], Id = "delete" }); break;
                            case "OrderAcceptanceRecordsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextRefresh"], Id = "refresh" }); break;
                            case "OrderAcceptanceRecordsContextMRP":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextMRP"], Id = "mrp" }); break;

                            case "OrderAcceptanceRecordsContextStatus":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "OrderAcceptanceRecordsContextTechnicalApproval":
                                                subMenus.Add(new MenuItem { Text = L["OrderAcceptanceRecordsContextTechnicalApproval"], Id = "techapproval" }); break;
                                            case "OrderAcceptanceRecordsContextOrderApproval":
                                                subMenus.Add(new MenuItem { Text = L["OrderAcceptanceRecordsContextOrderApproval"], Id = "orderapproval" }); break;
                                            case "OrderAcceptanceRecordsContextPending":
                                                subMenus.Add(new MenuItem { Text = L["OrderAcceptanceRecordsContextPending"], Id = "pending" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextStatus"], Id = "status", Items = subMenus }); break;

                            case "OrderAcceptanceRecordsContextControl":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextControl"], Id = "control" }); break;
                            case "OrderAcceptanceRecordsContextOrderLines":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderAcceptanceRecordsContextOrderLines"], Id = "orderlines" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMRPLineContextMenuItems()
        {
            if (MRPLineGridContextMenu.Count() == 0)
            {
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDoNotCalculate"], Id = "dontcalculate" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextStockUsage"], Id = "stockusage" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextChange"], Id = "changed" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextRefresh"], Id = "refresh" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextSupplier"], Id = "supplier" });

            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListOrderAcceptanceRecordsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                        DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        GridVirtualLineList.Clear();

                        //var productList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                        ProductReferanceNumbersList = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.CurrentAccountCardID == DataSource.CurrentAccountCardID).ToList();

                        foreach (var line in DataSource.SelectOrderAcceptanceRecordLines)
                        {
                            if (line.ProductID != Guid.Empty)
                            {
                                var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                                VirtualLineModel virtualModel = new VirtualLineModel
                                {
                                    Id = line.Id,
                                    CustomerBarcodeNo = line.CustomerBarcodeNo,
                                    CustomerReferanceNo = line.CustomerReferanceNo,
                                    DefinedUnitPrice = line.DefinedUnitPrice,
                                    Description_ = line.Description_,
                                    IsProductExists = true,
                                    LineAmount = line.LineAmount,
                                    LineNr = line.LineNr,
                                    MinOrderAmount = line.MinOrderAmount,
                                    OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                    OrderAmount = line.OrderAmount,
                                    OrderReferanceNo = line.OrderReferanceNo,
                                    OrderUnitPrice = line.OrderUnitPrice,
                                    ProductCode = product.Code,
                                    ProductID = line.ProductID,
                                    ProductName = product.Name,
                                    ProductReferanceNumberID = line.ProductReferanceNumberID,
                                    UnitSetCode = line.UnitSetCode,
                                    UnitSetID = line.UnitSetID,
                                };

                                GridVirtualLineList.Add(virtualModel);

                            }

                            else
                            {
                                VirtualLineModel virtualModel = new VirtualLineModel
                                {
                                    Id = line.Id,
                                    CustomerBarcodeNo = line.CustomerBarcodeNo,
                                    CustomerReferanceNo = line.CustomerReferanceNo,
                                    DefinedUnitPrice = line.DefinedUnitPrice,
                                    Description_ = line.Description_,
                                    IsProductExists = false,
                                    LineAmount = line.LineAmount,
                                    LineNr = line.LineNr,
                                    MinOrderAmount = line.MinOrderAmount,
                                    OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                    OrderAmount = line.OrderAmount,
                                    OrderReferanceNo = line.OrderReferanceNo,
                                    OrderUnitPrice = line.OrderUnitPrice,
                                    ProductCode = line.ProductCode,
                                    ProductID = Guid.Empty,
                                    ProductName = string.Empty,
                                    ProductReferanceNumberID = line.ProductReferanceNumberID,
                                    UnitSetCode = line.UnitSetCode,
                                    UnitSetID = line.UnitSetID,
                                };

                                GridVirtualLineList.Add(virtualModel);
                            }
                        }


                        ShowEditPage();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "techapproval":
                    if (args.RowInfo.RowData != null)
                    {

                        SpinnerService.Show();
                        await Task.Delay(100);
                        DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        var lineList = DataSource.SelectOrderAcceptanceRecordLines;

                        bool isEmptyProduct = false;

                        foreach (var line in lineList)
                        {
                            var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                            if (product == null || product.Id == Guid.Empty)
                            {
                                isEmptyProduct = true;
                                break;
                            }
                        }

                        if (!isEmptyProduct)
                        {
                            if (DataSource.OrderAcceptanceRecordState == Entities.Enums.OrderAcceptanceRecordStateEnum.Beklemede)
                            {
                                DataSource.OrderAcceptanceRecordState = Entities.Enums.OrderAcceptanceRecordStateEnum.TeknikOnayVerildi;

                                var updatedEntity = ObjectMapper.Map<SelectOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto>(DataSource);

                                await OrderAcceptanceRecordsAppService.UpdateTechApprovalAsync(updatedEntity);

                                await GetListDataSourceAsync();

                                await _grid.Refresh();
                            }
                            else
                            {
                                SpinnerService.Hide();
                                await ModalManager.WarningPopupAsync(L["UIOrderCreatedTitle"], L["UIOrderCreatedMessage"]);
                            }
                        }
                        else
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UILineIncludeEmptyProductTitle"], L["UILineIncludeEmptyProductMessage"]);
                        }


                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "orderapproval":
                    SpinnerService.Show();
                    await Task.Delay(100);
                    DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    var lineList2 = DataSource.SelectOrderAcceptanceRecordLines;

                    bool isEmptyProduct2 = false;

                    foreach (var line in lineList2)
                    {
                        var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                        if (product == null || product.Id == Guid.Empty)
                        {
                            isEmptyProduct2 = true;
                            break;
                        }
                    }

                    if (!isEmptyProduct2)
                    {
                        if (DataSource.OrderAcceptanceRecordState == Entities.Enums.OrderAcceptanceRecordStateEnum.TeknikOnayVerildi)
                        {
                            DataSource.OrderAcceptanceRecordState = Entities.Enums.OrderAcceptanceRecordStateEnum.SiparisFiyatOnayiVerildi;

                            var updatedEntity = ObjectMapper.Map<SelectOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto>(DataSource);

                            await OrderAcceptanceRecordsAppService.UpdateOrderApprovalAsync(updatedEntity);

                            await GetListDataSourceAsync();

                            await _grid.Refresh();
                        }
                        else
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIOrderCreatedTitle"], L["UIOrderCreatedMessage2"]);
                        }
                    }
                    else
                    {
                        SpinnerService.Hide();
                        await ModalManager.WarningPopupAsync(L["UILineIncludeEmptyProductTitle"], L["UILineIncludeEmptyProductMessage"]);
                    }


                    await InvokeAsync(StateHasChanged);

                    break;

                case "pending":
                    SpinnerService.Show();
                    await Task.Delay(100);

                    DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    var lineList3 = DataSource.SelectOrderAcceptanceRecordLines;

                    bool isEmptyProduct3 = false;

                    foreach (var line in lineList3)
                    {
                        var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                        if (product == null || product.Id == Guid.Empty)
                        {
                            isEmptyProduct2 = true;
                            break;
                        }
                    }

                    if (!isEmptyProduct3)
                    {
                        if (DataSource.OrderAcceptanceRecordState != Entities.Enums.OrderAcceptanceRecordStateEnum.SiparisOlusturuldu)
                        {
                            DataSource.OrderAcceptanceRecordState = Entities.Enums.OrderAcceptanceRecordStateEnum.Beklemede;

                            var updatedEntity = ObjectMapper.Map<SelectOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto>(DataSource);

                            await OrderAcceptanceRecordsAppService.UpdatePendingAsync(updatedEntity);

                            await GetListDataSourceAsync();

                            await _grid.Refresh();
                        }
                        else
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIOrderCreatedTitle"], L["UIOrderCreatedMessage3"]);
                        }
                    }
                    else
                    {
                        SpinnerService.Hide();
                        await ModalManager.WarningPopupAsync(L["UILineIncludeEmptyProductTitle"], L["UILineIncludeEmptyProductMessage"]);
                    }


                    await InvokeAsync(StateHasChanged);
                    break;

                case "convertorder":

                    if (args.RowInfo.RowData != null )
                    {

                        DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridConvertToOrderList = DataSource.SelectOrderAcceptanceRecordLines;

                        if (DataSource.OrderAcceptanceRecordState == OrderAcceptanceRecordStateEnum.SiparisFiyatOnayiVerildi)
                        {

                            SelectedToOrderList = new List<SelectOrderAcceptanceRecordLinesDto>();

                            foreach (var item in GridConvertToOrderList)
                            {
                                item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                                item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                                item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                            }

                            ConvertToOrderCrudPopup = true;
                        }
                        else if (DataSource.OrderAcceptanceRecordState == OrderAcceptanceRecordStateEnum.SiparisOlusturuldu)
                        {
                            await ModalManager.WarningPopupAsync(L["UIWarningOrderConvertTitle"], L["UIWarningOrderConvertMessage"]);
                        }
                        else
                        {
                            await ModalManager.WarningPopupAsync(L["UIWarningOrderConvertTitle"], L["UIWarningOrderConvertMessage2"]);
                        }
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                        if (res == true)
                        {
                            await DeleteAsync(args.RowInfo.RowData.Id);
                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                    }
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "mrp":

                    if (args.RowInfo.RowData != null)
                    {

                        SpinnerService.Show();
                        await Task.Delay(100);

                        DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        MRPLinesList.Clear();

                        var LineList = DataSource.SelectOrderAcceptanceRecordLines;

                        SelectMRPsDto mRPsDto = new SelectMRPsDto
                        {

                            Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu"),
                            Date_ = DataSource.Date_,
                            IsMaintenanceMRP = false,
                            ReferanceDate = GetSQLDateAppService.GetDateFromSQL(),
                            MaintenanceMRPID = Guid.Empty,
                            OrderAcceptanceID = DataSource.Id,
                            MaintenanceMRPCode = string.Empty,
                        };

                        MRPDataSource = mRPsDto;

                        var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;
                        var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

                        foreach (var line in LineList)
                        {
                            var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(line.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                            decimal amountofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == line.ProductID).Select(t => t.Amount).Sum();

                            CreateMRPLine(branch, warehouse, line, bomLineList, amountofProduct);

                            foreach (var bomLine in bomLineList)
                            {
                                if ((int)bomLine.SupplyForm == 2)
                                {
                                    var bomLineList2 = (await BillsofMaterialsAppService.GetbyProductIDAsync(bomLine.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                                    decimal amountofProduct2 = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID).Select(t => t.Amount).Sum();

                                    CreateMRPLine(branch, warehouse, line, bomLineList2, amountofProduct2);
                                }
                            }
                        }

                        MRPDataSource.SelectMRPLines = MRPLinesList;



                        SpinnerService.Hide();
                        MRPCrudModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "control":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        GridVirtualLineList.Clear();

                        var productList2 = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                        foreach (var line in DataSource.SelectOrderAcceptanceRecordLines)
                        {
                            if (productList2.Any(t => t.Id == line.ProductID))
                            {
                                VirtualLineModel virtualModel = new VirtualLineModel
                                {
                                    Id = line.Id,
                                    CustomerBarcodeNo = line.CustomerBarcodeNo,
                                    CustomerReferanceNo = line.CustomerReferanceNo,
                                    DefinedUnitPrice = line.DefinedUnitPrice,
                                    Description_ = line.Description_,
                                    IsProductExists = true,
                                    LineAmount = line.LineAmount,
                                    LineNr = line.LineNr,
                                    MinOrderAmount = line.MinOrderAmount,
                                    OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                    OrderAmount = line.OrderAmount,
                                    OrderReferanceNo = line.OrderReferanceNo,
                                    OrderUnitPrice = line.OrderUnitPrice,
                                    ProductCode = line.ProductCode,
                                    ProductID = line.ProductID,
                                    ProductName = line.ProductName,
                                    ProductReferanceNumberID = line.ProductReferanceNumberID,
                                    UnitSetCode = line.UnitSetCode,
                                    UnitSetID = line.UnitSetID,
                                };

                                GridVirtualLineList.Add(virtualModel);
                            }

                            else
                            {
                                VirtualLineModel virtualModel = new VirtualLineModel
                                {
                                    Id = line.Id,
                                    CustomerBarcodeNo = line.CustomerBarcodeNo,
                                    CustomerReferanceNo = line.CustomerReferanceNo,
                                    DefinedUnitPrice = line.DefinedUnitPrice,
                                    Description_ = line.Description_,
                                    IsProductExists = false,
                                    LineAmount = line.LineAmount,
                                    LineNr = line.LineNr,
                                    MinOrderAmount = line.MinOrderAmount,
                                    OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                    OrderAmount = line.OrderAmount,
                                    OrderReferanceNo = line.OrderReferanceNo,
                                    OrderUnitPrice = line.OrderUnitPrice,
                                    ProductCode = line.ProductCode,
                                    ProductID = line.ProductID,
                                    ProductName = line.ProductName,
                                    ProductReferanceNumberID = line.ProductReferanceNumberID,
                                    UnitSetCode = line.UnitSetCode,
                                    UnitSetID = line.UnitSetID,
                                };

                                GridVirtualLineList.Add(virtualModel);
                            }
                        }

                        OrderAcceptanceControlPopup = true;

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "orderlines":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await OrderAcceptanceRecordsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        var RoutesList = (await RoutesAppService.GetListAsync(new ListRoutesParameterDto())).Data.ToList();

                        foreach (var line in DataSource.SelectOrderAcceptanceRecordLines)
                        {
                            bool isRoute = false;
                            bool isBoM = false;
                            bool isApproved = false;

                            var route = RoutesList.Where(t => t.ProductID == line.ProductID).FirstOrDefault();
                            var boM = (await BillsofMaterialsAppService.GetbyCurrentAccountIDAsync(DataSource.CurrentAccountCardID.GetValueOrDefault(), line.ProductID.GetValueOrDefault())).Data;
                            var product = (await ProductsAppService.GetAsync(line.ProductID.GetValueOrDefault())).Data;

                            #region Rota, Reçete ve Onay Kontrolleri

                            if (route != null && route.Id != Guid.Empty)
                            {
                                isRoute = true;
                            }
                            else
                            {
                                isRoute = false;
                            }

                            if (boM != null && boM.Id != Guid.Empty)
                            {
                                isBoM = true;
                            }
                            else
                            {
                                isBoM = false;
                            }
                            if (product != null && product.Id != Guid.Empty)
                            {
                                isApproved = product.Confirmation;
                            }
                            else
                            {
                                isApproved = false;
                            }

                            #endregion

                            OrderLineModel orderLineModel = new OrderLineModel
                            {
                                CustomerBarcodeNo = line.CustomerBarcodeNo,
                                CustomerReferanceNo = line.CustomerReferanceNo,
                                Description_ = line.Description_,
                                IsApproved = isApproved,
                                IsBoM = isBoM,
                                IsRoute = isRoute,
                                OrderAmount = line.OrderAmount,
                                OrderReferanceNo = line.OrderReferanceNo,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                UnitSetCode = line.UnitSetCode,
                                UnitSetID = line.UnitSetID,
                            };

                            GridOrderLineList.Add(orderLineModel);
                        }

                        OrderLineCrudPopup = true;

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                default:
                    break;
            }
        }

        private void CreateMRPLine(SelectBranchesDto branch, SelectWarehousesDto warehouse, SelectOrderAcceptanceRecordLinesDto line, List<SelectBillsofMaterialLinesDto> bomLineList, decimal amountofProduct)
        {
            foreach (var bomline in bomLineList)
            {
                if ((int)bomline.SupplyForm == 1)
                {
                    #region Mamül Reçete satırdaki Temin Şekli Satınalma
                    int calculatedAmount = Convert.ToInt32(bomline.Quantity);


                    SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                    {
                        Amount = calculatedAmount,
                        isCalculated = true,
                        isPurchase = false,
                        ProductID = bomline.ProductID.GetValueOrDefault(),
                        ProductCode = bomline.ProductCode,
                        ProductName = bomline.ProductName,
                        SalesOrderID = Guid.Empty,
                        UnitSetID = bomline.UnitSetID,
                        LineNr = MRPLinesList.Count + 1,
                        UnitSetCode = bomline.UnitSetCode,
                        SupplyDate = GetSQLDateAppService.GetDateFromSQL(),
                        UnitPrice = 0,
                        State_ = string.Empty,
                        ReservedAmount = 0,
                        CurrentAccountCardID = Guid.Empty,
                        CurrentAccountCardCode = string.Empty,
                        CurrentAccountCardName = string.Empty,
                        AmountOfStock = amountofProduct,
                        RequirementAmount = calculatedAmount,
                        SalesOrderLineID = Guid.Empty,
                        SalesOrderFicheNo = string.Empty,
                        BranchID = branch.Id,
                        WarehouseID = warehouse.Id,
                        BranchCode = branch.Code,
                        WarehouseCode = warehouse.Code,
                        OrderAcceptanceID = DataSource.Id,
                        isStockUsage = false,
                        OrderAcceptanceLineID = line.Id,
                    };

                    MRPLinesList.Add(mrpLineModel);
                    #endregion

                }
                //else if(bomline.ProductSupplyType == 2)  yarı mamülün yarı mamülü (yavuz'un fantezisi)
                //{
                //    var bomLineList2 = (BillsofMaterialsAppService.GetbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Result.Data.SelectBillsofMaterialLines.ToList();

                //    decimal amountofProduct2 = ( GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Result.Data.Where(t => t.ProductID == bomline.ProductID).Select(t => t.Amount).Sum();

                //    CreateMRPLine(branch, warehouse, line, bomLineList2, amountofProduct2);
                //}
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<VirtualLineModel> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (DataSource.CurrentAccountCardID == Guid.Empty || DataSource.CurrentAccountCardID == null)
                    {
                        await ModalManager.WarningPopupAsync(L["UICurrentAccountTitle"], L["UICurrentAccountMessage"]);
                    }
                    else
                    {
                        VirtualLineDataSource = new VirtualLineModel();
                        ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
                        VirtualLineCrudPopup = true;
                        VirtualLineDataSource.LineNr = GridVirtualLineList.Count + 1;
                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        VirtualLineDataSource = args.RowInfo.RowData;
                        ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
                        VirtualLineCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupLineMessage"]);

                        if (res == true)
                        {
                            var line = args.RowInfo.RowData;

                            if (line.Id == Guid.Empty)
                            {
                                GridVirtualLineList.Remove(line);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    GridVirtualLineList.Remove(line);
                                    var actualLine = DataSource.SelectOrderAcceptanceRecordLines.Where(t => t.Id == line.Id).FirstOrDefault();
                                    DataSource.SelectOrderAcceptanceRecordLines.Remove(actualLine);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    GridVirtualLineList.Remove(line);
                                }
                            }

                            await _VirtualLineGrid.Refresh();
                            GetTotal();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _VirtualLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnMRPListContextMenuClick(ContextMenuClickEventArgs<SelectMRPLinesDto> args)
        {
            switch (args.Item.Id)
            {


                case "dontcalculate":
                    if (args.RowInfo.RowData != null)
                    {

                        var line = args.RowInfo.RowData;
                        var selectedIndex = MRPLinesList.FindIndex(t => t.SalesOrderID == line.SalesOrderID && t.ProductID == line.ProductID);
                        if (selectedIndex >= 0)
                        {
                            MRPLinesList[selectedIndex].isCalculated = false;
                        }

                        await _MRPLineGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "stockusage":
                    if (args.RowInfo.RowData != null)
                    {

                        var selectedline = args.RowInfo.RowData;
                        var Index = MRPLinesList.FindIndex(t => t.SalesOrderID == selectedline.SalesOrderID && t.ProductID == selectedline.ProductID);
                        if (Index >= 0)
                        {
                            MRPLinesList[Index].isStockUsage = !selectedline.isStockUsage;

                            if (MRPLinesList[Index].isStockUsage)
                            {
                                var stockAmount = MRPLinesList[Index].AmountOfStock;
                                var bomAmount = MRPLinesList[Index].Amount;
                                var purchaseAmount = MRPLinesList[Index].RequirementAmount;
                                var reservedAmount = MRPLinesList[Index].ReservedAmount;

                                if (stockAmount > bomAmount) // stok miktarı > reçeteden gelen toplam ihtiyaç miktarıysa
                                {
                                    MRPLinesList[Index].RequirementAmount = 0;
                                    MRPLinesList[Index].ReservedAmount = bomAmount;
                                    MRPLinesList[Index].AmountOfStock = stockAmount - bomAmount;
                                }
                                else if (stockAmount < bomAmount) // stok miktarı < reçeteden gelen toplam ihtiyaç miktarıysa
                                {
                                    MRPLinesList[Index].AmountOfStock = 0;
                                    MRPLinesList[Index].RequirementAmount = bomAmount - stockAmount;
                                    MRPLinesList[Index].ReservedAmount = stockAmount;
                                }
                                else if (stockAmount == bomAmount) // stok miktarı = reçeteden gelen toplam ihtiyaç miktarıysa
                                {
                                    MRPLinesList[Index].AmountOfStock = 0;
                                    MRPLinesList[Index].RequirementAmount = 0;
                                    MRPLinesList[Index].ReservedAmount = stockAmount;
                                }
                            }

                            else // Stoktan kullanılmayacaksa
                            {
                                MRPLinesList[Index].RequirementAmount = MRPLinesList[Index].Amount;
                                MRPLinesList[Index].ReservedAmount = 0;
                            }
                        }

                        await _MRPLineGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        MRPLineDataSource = args.RowInfo.RowData;
                        MRPLineCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await _MRPLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "supplier":

                    if (args.RowInfo.RowData != null)
                    {

                        MRPLineDataSource = args.RowInfo.RowData;

                        SupplierSelectionList.Clear();

                        var purchasePriceLinesList = (await PurchasePricesAppService.GetSelectLineListAsync(MRPLineDataSource.ProductID.GetValueOrDefault())).Data.ToList();

                        if (purchasePriceLinesList != null && purchasePriceLinesList.Count > 0)
                        {
                            var tempPurchasePriceLines = purchasePriceLinesList.Select(t => t.PurchasePriceID).Distinct();

                            foreach (var purchasePriceID in tempPurchasePriceLines)
                            {
                                var purchasePrice = (await PurchasePricesAppService.GetAsync(purchasePriceID)).Data;

                                if (purchasePrice != null && purchasePriceID != Guid.Empty && purchasePrice.IsApproved == false)
                                {
                                    purchasePriceLinesList = purchasePriceLinesList.Where(t => t.PurchasePriceID != purchasePriceID).ToList();
                                    //Onaylı olmayan fiyat kayıtlarına ait satırları yok etme
                                }
                            }

                            var groupedPurchasePriceLineList = purchasePriceLinesList.GroupBy(t => new { t.CurrencyID, t.CurrentAccountCardID }, (key, group) => new { CurrencyID = key.CurrencyID, CurrentAccountCardID = key.CurrentAccountCardID, Data = group.ToList() });

                            foreach (var item in groupedPurchasePriceLineList)
                            {
                                foreach (var data in item.Data)
                                {
                                    Models.SupplierSelectionGrid supplierSelectionModel = new Models.SupplierSelectionGrid
                                    {
                                        CurrentAccountName = data.CurrentAccountCardName,
                                        CurrentAccountID = data.CurrentAccountCardID,
                                        CurrenyCode = data.CurrencyCode,
                                        CurrenyID = data.CurrencyID,
                                        ProductCode = data.ProductCode,
                                        UnitPrice = data.Price,
                                        SupplyDate = data.SupplyDateDay
                                    };

                                    SupplierSelectionList.Add(supplierSelectionModel);
                                }
                            }

                            SupplierSelectionPopup = true;

                        }

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            VirtualLineCrudPopup = false;
        }

        public void HideMRPLinesPopup()
        {
            MRPLineCrudPopup = false;
        }

        public void HideMRPPopup()
        {
            MRPCrudModalVisible = false;
        }

        public void HideOrderLinePopup()
        {
            OrderLineCrudPopup = false;
            GridOrderLineList.Clear();
        }

        protected async Task OnLineSubmit()
        {

            if (VirtualLineDataSource.Id == Guid.Empty)
            {
                if (GridVirtualLineList.Contains(VirtualLineDataSource))
                {
                    int selectedLineIndex = GridVirtualLineList.FindIndex(t => t.LineNr == VirtualLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        GridVirtualLineList[selectedLineIndex] = VirtualLineDataSource;
                    }
                }
                else
                {
                    GridVirtualLineList.Add(VirtualLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = GridVirtualLineList.FindIndex(t => t.Id == VirtualLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    GridVirtualLineList[selectedLineIndex] = VirtualLineDataSource;
                }
            }
            await _VirtualLineGrid.Refresh();
            HideLinesPopup();
            await InvokeAsync(StateHasChanged);

        }

        protected async Task OnMRPLineSubmit()
        {

            if (MRPLineDataSource.Id == Guid.Empty)
            {
                if (MRPDataSource.SelectMRPLines.Contains(MRPLineDataSource))
                {
                    int selectedLineIndex = MRPDataSource.SelectMRPLines.FindIndex(t => t.LineNr == MRPLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        MRPDataSource.SelectMRPLines[selectedLineIndex] = MRPLineDataSource;
                    }
                }
                else
                {
                    MRPDataSource.SelectMRPLines.Add(MRPLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = MRPDataSource.SelectMRPLines.FindIndex(t => t.Id == MRPLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    MRPDataSource.SelectMRPLines[selectedLineIndex] = MRPLineDataSource;
                }
            }

            MRPLinesList = MRPDataSource.SelectMRPLines;
            await _MRPLineGrid.Refresh();

            HideMRPLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        public async void OnMRPSubmit()
        {
            var createdMRPEntity = ObjectMapper.Map<SelectMRPsDto, CreateMRPsDto>(MRPDataSource);

            await MRPsService.CreateAsync(createdMRPEntity);

            MRPCrudModalVisible = false;

            await InvokeAsync(StateHasChanged);

        }

        protected override Task OnSubmit()
        {
            foreach (var line in GridVirtualLineList)
            {
                if (!DataSource.SelectOrderAcceptanceRecordLines.Any(t => t.ProductCode == line.ProductCode))
                {
                    SelectOrderAcceptanceRecordLinesDto lineModel = new SelectOrderAcceptanceRecordLinesDto
                    {
                        Id = line.Id,
                        CustomerBarcodeNo = line.CustomerBarcodeNo,
                        UnitSetID = line.UnitSetID,
                        DefinedUnitPrice = line.DefinedUnitPrice,
                        Description_ = line.Description_,
                        LineAmount = line.LineAmount,
                        LineNr = line.LineNr,
                        MinOrderAmount = line.MinOrderAmount,
                        UnitSetCode = line.UnitSetCode,
                        ProductReferanceNumberID = line.ProductReferanceNumberID,
                        ProductName = line.ProductName,
                        ProductID = line.ProductID,
                        ProductCode = line.ProductCode,
                        OrderUnitPrice = line.OrderUnitPrice,
                        OrderReferanceNo = line.OrderReferanceNo,
                        OrderAmount = line.OrderAmount,
                        OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                        CustomerReferanceNo = line.CustomerReferanceNo,

                    };

                    DataSource.SelectOrderAcceptanceRecordLines.Add(lineModel);
                }

            }

            return base.OnSubmit();
        }

        private void OrderValueChangeHandler(Syncfusion.Blazor.Inputs.ChangeEventArgs<decimal> args)
        {
            VirtualLineDataSource.LineAmount = VirtualLineDataSource.OrderAmount * VirtualLineDataSource.OrderUnitPrice;
        }

        public async void SupplierDoubleClickHandler(RecordDoubleClickEventArgs<Models.SupplierSelectionGrid> args)
        {
            var selectedSupplier = args.RowData;

            if (selectedSupplier != null)
            {
                MRPLineDataSource.CurrencyID = selectedSupplier.CurrenyID;
                MRPLineDataSource.CurrencyCode = selectedSupplier.CurrenyCode;
                MRPLineDataSource.CurrentAccountCardID = selectedSupplier.CurrentAccountID;
                MRPLineDataSource.CurrentAccountCardName = selectedSupplier.CurrentAccountName;
                MRPLineDataSource.UnitPrice = selectedSupplier.UnitPrice;
                MRPLineDataSource.SupplyDate = MRPLineDataSource.SupplyDate.AddDays(selectedSupplier.SupplyDate);
                HideSupplierSelectionPopup();
                await _MRPLineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public void HideSupplierSelectionPopup()
        {
            SupplierSelectionList.Clear();
            SupplierSelectionPopup = false;
        }

        public void HideOrderAcceptanceControlPopup()
        {
            OrderAcceptanceControlPopup = false;
            GridVirtualLineList.Clear();
        }

        public void ReferanceDateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            if (MRPLinesList != null && MRPLinesList.Count > 0)
            {
                foreach (var line in MRPLinesList)
                {
                    int index = MRPLinesList.IndexOf(line);
                    MRPLinesList[index].SupplyDate = args.Value;
                }

                _MRPLineGrid.Refresh();
            }
        }

        private async Task GetSalesOrdersList()
        {
            SalesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();
        }
        private async Task GetPaymentPlansList()
        {
            PaymentPlansList = (await PaymentPlansAppService.GetListAsync(new ListPaymentPlansParameterDto())).Data.ToList();
        }

        #region Excel'den Aktarım
        //SfGrid<ExpandoObject> Grid;
        public DataTable table = new DataTable();
        private void OnChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                #region Expando Object Örneği

                var path = file.FileInfo.Name;
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                var check = ExcelService.ImportGetPath(path);
                FileStream openFileStream = new FileStream(check, FileMode.OpenOrCreate);
                file.Stream.WriteTo(openFileStream);
                FileStream fileStream = new FileStream(check, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                IWorksheet worksheet = workbook.Worksheets[1];
                table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                GenerateListFromTable(table);

                #endregion

            }
        }
        string[] Columns;
        public List<ExpandoObject> OrderList;
        public async void GenerateListFromTable(DataTable input)
        {
            var list = new List<ExpandoObject>();
            Columns = input.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            foreach (DataRow row in input.Rows)
            {
                System.Dynamic.ExpandoObject e = new System.Dynamic.ExpandoObject();
                foreach (DataColumn col in input.Columns)
                    e.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                list.Add(e);
            }
            OrderList = list;


            foreach (var item in OrderList)
            {
                dynamic row = item;

                #region Row DBNull Kontrolleri

                string productCode = !string.IsNullOrEmpty(Convert.ToString(row.ProductCode)) ? (string)row.ProductCode : string.Empty;
                string customerBarcodeNo = !string.IsNullOrEmpty(Convert.ToString(row.CustomerBarcodeNo)) ? (string)row.CustomerBarcodeNo : string.Empty;
                string customerReferanceNo = !string.IsNullOrEmpty(Convert.ToString(row.CustomerReferanceNo)) ? (string)row.CustomerReferanceNo : string.Empty;
                string orderReferanceNo = !string.IsNullOrEmpty(Convert.ToString(row.OrderReferanceNo)) ? (string)row.OrderReferanceNo : string.Empty;
                decimal lineAmount = !string.IsNullOrEmpty(Convert.ToString(row.LineAmount)) ? Convert.ToDecimal(row.LineAmount) : 0;
                decimal orderAmount = !string.IsNullOrEmpty(Convert.ToString(row.OrderAmount)) ? Convert.ToDecimal(row.OrderAmount) : 0;
                decimal orderUnitPrice = !string.IsNullOrEmpty(Convert.ToString(row.OrderUnitPrice)) ? Convert.ToDecimal(row.OrderUnitPrice) : 0;

                #endregion

                var product = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.Code == productCode).FirstOrDefault();

                if (product != null)
                {
                    //var salesPriceID = (await SalesPricesAppService.GetListAsync(new ListSalesPricesParameterDto())).Data.Where(t => t.StartDate <= DataSource.Date_ && t.EndDate >= DataSource.Date_ && t.CurrentAccountCardID == DataSource.CurrentAccountCardID && t.IsActive == true && t.IsApproved == true).Select(t => t.Id).FirstOrDefault();
                    //var salesPriceLine = (await SalesPricesAppService.GetAsync(salesPriceID)).Data.SelectSalesPriceLines.Where(t => t.ProductCode == product.Code).FirstOrDefault();

                    var definedPrice = (await SalesPricesAppService.GetDefinedProductPriceAsync(product.Id, DataSource.CurrentAccountCardID.GetValueOrDefault(), DataSource.CurrenyID.GetValueOrDefault(), true, DataSource.Date_)).Data;


                    if (definedPrice != null && definedPrice.Id != Guid.Empty)
                    {
                        var productRefNo = ProductReferanceNumbersList.Where(t => t.ProductID == product.Id).FirstOrDefault();

                        VirtualLineModel virtualModel = new VirtualLineModel
                        {
                            CustomerBarcodeNo = customerBarcodeNo,
                            ProductCode = product.Code,
                            ProductName = product.Name,
                            ProductID = product.Id,
                            CustomerReferanceNo = customerReferanceNo,
                            DefinedUnitPrice = definedPrice.Price,
                            Description_ = string.Empty,
                            IsProductExists = true,
                            LineAmount = lineAmount,
                            LineNr = GridVirtualLineList.Count + 1,
                            MinOrderAmount = productRefNo.MinOrderAmount,
                            OrderAmount = orderAmount,
                            OrderReferanceNo = orderReferanceNo,
                            OrderUnitPrice = orderUnitPrice,
                            ProductReferanceNumberID = productRefNo.Id,
                            UnitSetID = product.UnitSetID,
                            UnitSetCode = product.UnitSetCode,
                        };

                        GridVirtualLineList.Add(virtualModel);
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync("UIWarningSalesPriceTitle", "UIWarningSalesPriceMessage");
                    }


                }
                else if (product == null && productCode != string.Empty)
                {
                    VirtualLineModel virtualModel = new VirtualLineModel
                    {
                        CustomerBarcodeNo = customerBarcodeNo,
                        ProductCode = productCode,
                        ProductName = string.Empty,
                        ProductID = Guid.Empty,
                        CustomerReferanceNo = customerReferanceNo,
                        DefinedUnitPrice = 0,
                        Description_ = string.Empty,
                        IsProductExists = false,
                        LineAmount = lineAmount,
                        LineNr = GridVirtualLineList.Count + 1,
                        MinOrderAmount = 0,
                        OrderAmount = orderAmount,
                        OrderReferanceNo = orderReferanceNo,
                        OrderUnitPrice = orderUnitPrice,
                        ProductReferanceNumberID = Guid.Empty,
                        UnitSetID = Guid.Empty,
                        UnitSetCode = string.Empty,
                    };

                    GridVirtualLineList.Add(virtualModel);
                }

            }

            await _VirtualLineGrid.Refresh();
        }

        #endregion

        #region Sipariş Kabul Kontrol Excel'den Aktarım
        //SfGrid<ExpandoObject> Grid;
        public DataTable controltable = new DataTable();
        private void ControlOnChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                #region Expando Object Örneği

                var path = file.FileInfo.Name;
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                var check = ExcelService.ImportGetPath(path);
                FileStream openFileStream = new FileStream(check, FileMode.OpenOrCreate);
                file.Stream.WriteTo(openFileStream);
                FileStream fileStream = new FileStream(check, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                IWorksheet worksheet = workbook.Worksheets[1];
                controltable = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                ControlGenerateListFromTable(controltable);

                #endregion

            }
        }
        string[] ControlColumns;
        public List<ExpandoObject> ControlOrderList;
        public async void ControlGenerateListFromTable(DataTable input)
        {
            var list = new List<ExpandoObject>();
            ControlColumns = input.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            foreach (DataRow row in input.Rows)
            {
                System.Dynamic.ExpandoObject e = new System.Dynamic.ExpandoObject();
                foreach (DataColumn col in input.Columns)
                    e.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                list.Add(e);
            }
            ControlOrderList = list;


            foreach (var item in ControlOrderList)
            {
                dynamic row = item;

                #region Row DBNull Kontrolleri

                string productCode = !string.IsNullOrEmpty(Convert.ToString(row.ProductCode)) ? (string)row.ProductCode : string.Empty;
                string customerBarcodeNo = !string.IsNullOrEmpty(Convert.ToString(row.CustomerBarcodeNo)) ? (string)row.CustomerBarcodeNo : string.Empty;
                string customerReferanceNo = !string.IsNullOrEmpty(Convert.ToString(row.CustomerReferanceNo)) ? (string)row.CustomerReferanceNo : string.Empty;
                string orderReferanceNo = !string.IsNullOrEmpty(Convert.ToString(row.OrderReferanceNo)) ? (string)row.OrderReferanceNo : string.Empty;
                decimal lineAmount = !string.IsNullOrEmpty(Convert.ToString(row.LineAmount)) ? Convert.ToDecimal(row.LineAmount) : 0;
                decimal orderAmount = !string.IsNullOrEmpty(Convert.ToString(row.OrderAmount)) ? Convert.ToDecimal(row.OrderAmount) : 0;
                decimal orderUnitPrice = !string.IsNullOrEmpty(Convert.ToString(row.OrderUnitPrice)) ? Convert.ToDecimal(row.OrderUnitPrice) : 0;

                #endregion

                if (!DataSource.SelectOrderAcceptanceRecordLines.Any(t => t.ProductCode == productCode))
                {
                    var product = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.Code == productCode).FirstOrDefault();

                    if (product != null)
                    {
                        var salesPriceID = (await SalesPricesAppService.GetListAsync(new ListSalesPricesParameterDto())).Data.Where(t => t.StartDate <= DataSource.Date_ && t.EndDate >= DataSource.Date_ && t.CurrentAccountCardID == DataSource.CurrentAccountCardID && t.IsActive == true && t.IsApproved == true).Select(t => t.Id).FirstOrDefault();
                        var salesPriceLine = (await SalesPricesAppService.GetAsync(salesPriceID)).Data.SelectSalesPriceLines.Where(t => t.ProductCode == product.Code).FirstOrDefault();


                        if (salesPriceLine != null && salesPriceLine.Id != Guid.Empty)
                        {
                            var productRefNo = ProductReferanceNumbersList.Where(t => t.ProductID == product.Id).FirstOrDefault();

                            VirtualLineModel virtualModel = new VirtualLineModel
                            {
                                CustomerBarcodeNo = customerBarcodeNo,
                                ProductCode = product.Code,
                                ProductName = product.Name,
                                ProductID = product.Id,
                                CustomerReferanceNo = customerReferanceNo,
                                DefinedUnitPrice = salesPriceLine.Price,
                                Description_ = string.Empty,
                                IsProductExists = true,
                                LineAmount = lineAmount,
                                LineNr = GridVirtualLineList.Count + 1,
                                MinOrderAmount = productRefNo.MinOrderAmount,
                                OrderAmount = orderAmount,
                                OrderReferanceNo = orderReferanceNo,
                                OrderUnitPrice = orderUnitPrice,
                                ProductReferanceNumberID = productRefNo.Id,
                                UnitSetID = product.UnitSetID,
                                UnitSetCode = product.UnitSetCode,
                            };

                            GridVirtualLineList.Add(virtualModel);
                        }
                        else
                        {
                            await ModalManager.WarningPopupAsync(L["UIWarningSalesPriceTitle"], L["UIWarningSalesPriceMessage"]);
                        }


                    }
                    else if (product == null && productCode != string.Empty)
                    {
                        VirtualLineModel virtualModel = new VirtualLineModel
                        {
                            CustomerBarcodeNo = customerBarcodeNo,
                            ProductCode = productCode,
                            ProductName = string.Empty,
                            ProductID = Guid.Empty,
                            CustomerReferanceNo = customerReferanceNo,
                            DefinedUnitPrice = 0,
                            Description_ = string.Empty,
                            IsProductExists = false,
                            LineAmount = lineAmount,
                            LineNr = GridVirtualLineList.Count + 1,
                            MinOrderAmount = 0,
                            OrderAmount = orderAmount,
                            OrderReferanceNo = orderReferanceNo,
                            OrderUnitPrice = orderUnitPrice,
                            ProductReferanceNumberID = Guid.Empty,
                            UnitSetID = Guid.Empty,
                            UnitSetCode = string.Empty,
                        };

                        GridVirtualLineList.Add(virtualModel);
                    }
                }



            }

            await _VirtualLineGrid.Refresh();
        }

        #endregion


        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                VirtualLineDataSource.ProductID = Guid.Empty;
                VirtualLineDataSource.ProductCode = string.Empty;
                VirtualLineDataSource.ProductName = string.Empty;
                VirtualLineDataSource.OrderReferanceNo = string.Empty;
                VirtualLineDataSource.CustomerReferanceNo = string.Empty;
                VirtualLineDataSource.CustomerBarcodeNo = string.Empty;
                VirtualLineDataSource.MinOrderAmount = 0;
                VirtualLineDataSource.UnitSetCode = string.Empty;
                VirtualLineDataSource.UnitSetID = Guid.Empty;
                VirtualLineDataSource.DefinedUnitPrice = 0;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                VirtualLineDataSource.ProductID = selectedProduct.Id;
                VirtualLineDataSource.ProductCode = selectedProduct.Code;
                VirtualLineDataSource.ProductName = selectedProduct.Name;


                if (DataSource.CurrentAccountCardID != Guid.Empty && DataSource.CurrentAccountCardID != null)
                {

                    var productRefNo = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.ProductCode == VirtualLineDataSource.ProductCode && t.CurrentAccountCardID == DataSource.CurrentAccountCardID).FirstOrDefault();

                    if (productRefNo != null)
                    {
                        var tempProductList = ProductsList.Where(t => t.Code == VirtualLineDataSource.ProductCode).ToList();

                        VirtualLineDataSource.OrderReferanceNo = productRefNo.ReferanceNo;
                        VirtualLineDataSource.CustomerReferanceNo = productRefNo.CustomerReferanceNo;
                        VirtualLineDataSource.CustomerBarcodeNo = productRefNo.CustomerBarcodeNo;
                        VirtualLineDataSource.MinOrderAmount = productRefNo.MinOrderAmount;
                        VirtualLineDataSource.UnitSetCode = tempProductList.Select(t => t.UnitSetCode).FirstOrDefault();
                        VirtualLineDataSource.UnitSetID = tempProductList.Select(t => t.UnitSetID).FirstOrDefault();
                        VirtualLineDataSource.ProductID = tempProductList.Select(t => t.Id).FirstOrDefault();
                        VirtualLineDataSource.ProductReferanceNumberID = productRefNo.Id;

                        //var salesPriceID = (await SalesPricesAppService.GetListAsync(new ListSalesPricesParameterDto())).Data.Where(t => t.StartDate <= DataSource.Date_ && t.EndDate >= DataSource.Date_ && t.CurrentAccountCardID == DataSource.CurrentAccountCardID && t.IsActive == true && t.IsApproved == true).Select(t => t.Id).FirstOrDefault();

                        //if (salesPriceID != Guid.Empty)
                        //{
                        //    var salesPriceLine = (await SalesPricesAppService.GetAsync(salesPriceID)).Data.SelectSalesPriceLines.Where(t => t.ProductCode == VirtualLineDataSource.ProductCode).FirstOrDefault();

                        //    if (salesPriceLine != null && salesPriceLine.Id != Guid.Empty)
                        //    {
                        //        VirtualLineDataSource.DefinedUnitPrice = salesPriceLine.Price;
                        //    }

                        //}
                    }
                    var definedPrice = (await SalesPricesAppService.GetDefinedProductPriceAsync(
                        selectedProduct.Id,
                        DataSource.CurrentAccountCardID.GetValueOrDefault(),
                        DataSource.CurrenyID.GetValueOrDefault(),
                        true,
                        DataSource.Date_))
                        .Data;

                    if (definedPrice.Id != Guid.Empty)
                    {
                        VirtualLineDataSource.DefinedUnitPrice = definedPrice.Price;
                    }
                }

                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCodeButtonEdit = new();
        SfTextBox CurrentAccountCardsNameButtonEdit = new();
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCodeButtonClickEvent);
            await CurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task CurrentAccountCardsNameOnCreateIcon()
        {
            var CurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsNameButtonClickEvent);
            await CurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsNameButtonClick } });
        }

        public async void CurrentAccountCardsNameButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
                DataSource.CurrentAccountCardCustomerCode = string.Empty;
                DataSource.CurrenyID = Guid.Empty;
                DataSource.CurrenyCode = string.Empty;

                ProductReferanceNumbersList = new List<ListProductReferanceNumbersDto>();
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccount = args.RowData;

            if (selectedCurrentAccount != null)
            {
                DataSource.CurrentAccountCardID = selectedCurrentAccount.Id;
                DataSource.CurrentAccountCardCode = selectedCurrentAccount.Code;
                DataSource.CurrentAccountCardName = selectedCurrentAccount.Name;
                DataSource.CurrentAccountCardCustomerCode = selectedCurrentAccount.CustomerCode;
                DataSource.CurrenyID = selectedCurrentAccount.CurrencyID;
                DataSource.CurrenyCode = selectedCurrentAccount.Currency;


                ProductReferanceNumbersList = (await ProductReferanceNumbersAppService.GetListAsync(new ListProductReferanceNumbersParameterDto())).Data.Where(t => t.CurrentAccountCardID == selectedCurrentAccount.Id).ToList();


                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("OrderAcceptanceRecordsChildMenu");
            await InvokeAsync(StateHasChanged);
        }

        SfTextBox MRPCodeButtonEdit;

        public async Task MRPCodeOnCreateIcon()
        {
            var MRPCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, MRPCodeButtonClickEvent);
            await MRPCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", MRPCodesButtonClick } });
        }

        public async void MRPCodeButtonClickEvent()
        {
            MRPDataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region MRP Crud ButtonEdits

        #region Depo ButtonEdit (Tüm LineList)

        SfTextBox WarehousesButtonEdit;
        bool SelectWarehousesPopupVisible = false;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        public async Task WarehousesOnCreateIcon()
        {
            var WarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WarehousesButtonClickEvent);
            await WarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WarehousesButtonClick } });
        }

        public async void WarehousesButtonClickEvent()
        {
            SelectWarehousesPopupVisible = true;
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                WarehouseIDButtonEdit = Guid.Empty;
                WarehouseCodeButtonEdit = string.Empty;



                var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

                foreach (var item in MRPLinesList)
                {
                    item.WarehouseID = warehouse.Id;
                    item.WarehouseCode = warehouse.Code;
                }
                await _MRPLineGrid.Refresh();
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                WarehouseIDButtonEdit = selectedWarehouse.Id;
                WarehouseCodeButtonEdit = selectedWarehouse.Code;
                SelectWarehousesPopupVisible = false;

                foreach (var item in MRPLinesList)
                {
                    item.WarehouseID = WarehouseIDButtonEdit;
                    item.WarehouseCode = WarehouseCodeButtonEdit;
                }

                await _MRPLineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit (Tüm LineList)

        SfTextBox BranchesButtonEdit;
        bool SelectBranchesPopupVisible = false;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        public async Task BranchesOnCreateIcon()
        {
            var BranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchesButtonClickEvent);
            await BranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchesButtonClick } });
        }

        public async void BranchesButtonClickEvent()
        {
            SelectBranchesPopupVisible = true;
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                BranchIDButtonEdit = Guid.Empty;
                BranchCodeButtonEdit = string.Empty;

                var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;

                foreach (var item in MRPLinesList)
                {
                    item.BranchID = branch.Id;
                    item.BranchCode = branch.Code;
                }
                await _MRPLineGrid.Refresh();
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                BranchIDButtonEdit = selectedBranch.Id;
                BranchCodeButtonEdit = selectedBranch.Code;

                foreach (var item in MRPLinesList)
                {
                    item.BranchID = BranchIDButtonEdit;
                    item.BranchCode = BranchCodeButtonEdit;
                }

                await _MRPLineGrid.Refresh();
                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Depo ButtonEdit

        SfTextBox LineWarehousesButtonEdit;
        bool SelectLineWarehousesPopupVisible = false;
        List<ListWarehousesDto> LineWarehousesList = new List<ListWarehousesDto>();

        public async Task LineWarehousesOnCreateIcon()
        {
            var LineWarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineWarehousesButtonClickEvent);
            await LineWarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineWarehousesButtonClick } });
        }

        public async void LineWarehousesButtonClickEvent()
        {
            SelectLineWarehousesPopupVisible = true;
            LineWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void LineWarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                MRPLineDataSource.WarehouseID = Guid.Empty;
                MRPLineDataSource.WarehouseCode = string.Empty;
            }

            await Task.CompletedTask;
        }

        public async void LineWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                MRPLineDataSource.WarehouseID = selectedWarehouse.Id;
                MRPLineDataSource.WarehouseCode = selectedWarehouse.Code;
                SelectLineWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit 

        SfTextBox LineBranchesButtonEdit;
        bool SelectLineBranchesPopupVisible = false;
        List<ListBranchesDto> LineBranchesList = new List<ListBranchesDto>();

        public async Task LineBranchesOnCreateIcon()
        {
            var LineBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineBranchesButtonClickEvent);
            await LineBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineBranchesButtonClick } });
        }

        public async void LineBranchesButtonClickEvent()
        {
            SelectLineBranchesPopupVisible = true;
            LineBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void LineBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                MRPLineDataSource.BranchID = Guid.Empty;
                MRPLineDataSource.BranchCode = string.Empty;
            }


            await Task.CompletedTask;
        }

        public async void LineBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                MRPLineDataSource.BranchID = selectedBranch.Id;
                MRPLineDataSource.BranchCode = selectedBranch.Code;
                SelectLineBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #endregion

        #region Ödeme Planı ButtonEdit

        SfTextBox PaymentPlansButtonEdit;
        bool SelectPaymentPlansPopupVisible = false;
        List<ListPaymentPlansDto> PaymentPlansList = new List<ListPaymentPlansDto>();

        public async Task PaymentPlansOnCreateIcon()
        {
            var PaymentPlansButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, PaymentPlansButtonClickEvent);
            await PaymentPlansButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", PaymentPlansButtonClick } });
        }

        public async void PaymentPlansButtonClickEvent()
        {
            SelectPaymentPlansPopupVisible = true;
            await GetPaymentPlansList();
            await InvokeAsync(StateHasChanged);
        }

        public void PaymentPlansOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PaymentPlanID = Guid.Empty;
                DataSource.PaymentPlanName = string.Empty;
            }
        }

        public async void PaymentPlansDoubleClickHandler(RecordDoubleClickEventArgs<ListPaymentPlansDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.PaymentPlanID = selectedUnitSet.Id;
                DataSource.PaymentPlanName = selectedUnitSet.Name;
                SelectPaymentPlansPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }




    public class VirtualLineModel
    {
        public Guid Id { get; set; }
        public bool IsProductExists { get; set; }
        public Guid OrderAcceptanceRecordID { get; set; }
        public int LineNr { get; set; }
        public Guid? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid? ProductReferanceNumberID { get; set; }
        public string OrderReferanceNo { get; set; }
        public string CustomerReferanceNo { get; set; }
        public string CustomerBarcodeNo { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public Guid? UnitSetID { get; set; }
        public string UnitSetCode { get; set; }
        public decimal DefinedUnitPrice { get; set; }
        public decimal OrderUnitPrice { get; set; }
        public decimal LineAmount { get; set; }
        public string Description_ { get; set; }
    }

    public class OrderLineModel
    {
        public Guid? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string OrderReferanceNo { get; set; }
        public string CustomerReferanceNo { get; set; }
        public string CustomerBarcodeNo { get; set; }
        public decimal OrderAmount { get; set; }
        public Guid? UnitSetID { get; set; }
        public string UnitSetCode { get; set; }
        public string Description_ { get; set; }
        public bool IsRoute { get; set; }
        public bool IsBoM { get; set; }
        public bool IsApproved { get; set; }
    }
}
