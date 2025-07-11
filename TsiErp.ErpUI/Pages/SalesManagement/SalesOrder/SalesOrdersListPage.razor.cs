﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.SplitButtons;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Entities.PurchaseManagement.PurchaseInvoice.Services;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.PurchaseManagement.PurchaseOrder.PurchaseOrdersListPage;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using DevExpress.Blazor.Internal;
using Syncfusion.Blazor.Navigations;
using TsiErp.Business.Entities.OrderAcceptanceRecord.Services;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;


namespace TsiErp.ErpUI.Pages.SalesManagement.SalesOrder
{
    public partial class SalesOrdersListPage : IDisposable
    {
        private SfGrid<SelectSalesOrderLinesDto> _LineGrid;
        private SfGrid<SelectSalesOrderLinesDto> _ProductionOrderGrid;
        private SfGrid<ProductsTreeDto> _BoMLineGrid;
        private SfGrid<CreateSalesInvoices> _CreateSalesInvoicesGrid;

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }

        SelectSalesOrderLinesDto LineDataSource;
        CreateSalesInvoices CreateSalesInvoicesDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> OrderLinesWithSemiProductsGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> CreateSalesInvoicesGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> BoMLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectSalesOrderLinesDto> GridLineList = new List<SelectSalesOrderLinesDto>();

        List<SelectSalesOrderLinesDto> GridProductionOrderList = new List<SelectSalesOrderLinesDto>();

        List<SelectBillsofMaterialLinesDto> GridBoMLineList = new List<SelectBillsofMaterialLinesDto>();

        List<ListBillsofMaterialsDto> BoMList = new List<ListBillsofMaterialsDto>();

        List<CreateSalesInvoices> CreateSalesInvoicesList = new List<CreateSalesInvoices>();

        private bool LineCrudPopup = false;

        private bool CreateProductionOrderCrudPopup = false;

        private bool BoMLineCrudPopup = false;

        private bool CreateSalesInvoicesCrudPopup = false;

        DateTime MaxDate;

        SfProgressButton ProgressBtn;
        bool HideCreateProductionOrderPopupButtonDisabled = false;
        bool LoadingModalVisibility = false;

        #region ButtonEdit Metotları

        #region Birim Setleri ButtonEdit

        SfTextBox UnitSetsButtonEdit;
        bool SelectUnitSetsPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        public async Task UnitSetsOnCreateIcon()
        {
            var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
            await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        }

        public async void UnitSetsButtonClickEvent()
        {
            SelectUnitSetsPopupVisible = true;
            await GetUnitSetsList();
            await InvokeAsync(StateHasChanged);
        }

        public void UnitSetsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                LineDataSource.UnitSetID = selectedUnitSet.Id;
                LineDataSource.UnitSetCode = selectedUnitSet.Name;
                SelectUnitSetsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Para Birimleri ButtonEdit

        SfTextBox CurrenciesButtonEdit;
        bool SelectCurrencyPopupVisible = false;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        public async Task CurrenciesOnCreateIcon()
        {
            var CurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrenciesButtonClickEvent);
            await CurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrenciesButtonClick } });
        }

        public async void CurrenciesButtonClickEvent()
        {
            SelectCurrencyPopupVisible = true;
            await GetCurrenciesList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrencyID = selectedUnitSet.Id;
                DataSource.CurrencyCode = selectedUnitSet.Name;
                SelectCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İşlem Dövizi Para Birimleri ButtonEdit

        SfTextBox TransactionExchangeCurrenciesButtonEdit;
        bool SelectTransactionExchangeCurrencyPopupVisible = false;
        List<ListCurrenciesDto> TransactionExchangeCurrenciesList = new List<ListCurrenciesDto>();

        public async Task TransactionExchangeCurrenciesOnCreateIcon()
        {
            var TransactionExchangeCurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TransactionExchangeCurrenciesButtonClickEvent);
            await TransactionExchangeCurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TransactionExchangeCurrenciesButtonClick } });
        }

        public async void TransactionExchangeCurrenciesButtonClickEvent()
        {
            SelectCurrencyPopupVisible = true;
            TransactionExchangeCurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void TransactionExchangeCurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TransactionExchangeCurrencyID = Guid.Empty;
                DataSource.TransactionExchangeCurrencyCode = string.Empty;
                DataSource.ExchangeRate = 0;
            }
            else
            {
                decimal exRate = 1;

                var exchangeRateType = LoginedUserService.PurchaseRequestExchangeRateType;

                var date = new DateTime(DataSource.Date_.Year, DataSource.Date_.Month, DataSource.Date_.Day);

                var exchangeRate = (await ExchangeRatesService.GetListAsync(new ListExchangeRatesParameterDto())).Data.Where(t => t.Date == date && t.CurrencyId == DataSource.TransactionExchangeCurrencyID).FirstOrDefault();

                if (exchangeRate != null)
                {
                    switch (exchangeRateType)
                    {
                        case 0:
                            exRate = exchangeRate.BuyingRate;
                            break;
                        case 1:
                            exRate = exchangeRate.SaleRate;
                            break;
                        case 2:
                            exRate = exchangeRate.EffectiveBuyingRate;
                            break;
                        case 3:
                            exRate = exchangeRate.EffectiveSaleRate;
                            break;
                        default:
                            exRate = 1;
                            break;
                    }
                }

                DataSource.ExchangeRate = exRate;

            }
        }

        public async void TransactionExchangeCurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                DataSource.TransactionExchangeCurrencyID = selectedCurrency.Id;
                DataSource.TransactionExchangeCurrencyCode = selectedCurrency.Name;
                SelectTransactionExchangeCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit

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
            await GetBranchesList();
            await InvokeAsync(StateHasChanged);
        }

        public void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchCode = string.Empty;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.BranchID = selectedUnitSet.Id;
                DataSource.BranchCode = selectedUnitSet.Code;
                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
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

        #region Depo ButtonEdit

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
            await GetWarehousesList();
            await InvokeAsync(StateHasChanged);
        }

        public void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WarehouseID = Guid.Empty;
                DataSource.WarehouseCode = string.Empty;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.WarehouseID = selectedUnitSet.Id;
                DataSource.WarehouseCode = selectedUnitSet.Code;
                SelectWarehousesPopupVisible = false;
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
            await GetCurrentAccountCardsList();
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
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
                DataSource.CustomerCode = string.Empty;
                ShippingAdressEnable = false;
                DataSource.ShippingAdressCode = string.Empty;
                DataSource.ShippingAdressID = Guid.Empty;
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrentAccountCardID = selectedUnitSet.Id;
                DataSource.CurrentAccountCardCode = selectedUnitSet.Code;
                DataSource.CurrentAccountCardName = selectedUnitSet.Name;
                DataSource.TransactionExchangeCurrencyCode = selectedUnitSet.Currency;
                DataSource.TransactionExchangeCurrencyID = selectedUnitSet.CurrencyID;
                DataSource.CustomerCode = selectedUnitSet.CustomerCode;
                SelectCurrentAccountCardsPopupVisible = false;
                DataSource.ShippingAdressCode = selectedUnitSet.ShippingAddress;
                DataSource.CurrencyID = selectedUnitSet.CurrencyID.GetValueOrDefault();
                DataSource.CurrencyCode = selectedUnitSet.Currency;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Sevkiyat Adresi ButtonEdit

        SfTextBox ShippingAdressesButtonEdit;
        bool ShippingAdressEnable = false;
        bool SelectShippingAdressesPopupVisible = false;
        List<ListShippingAdressesDto> ShippingAdressesList = new List<ListShippingAdressesDto>();

        public async Task ShippingAdressesOnCreateIcon()
        {
            var ShippingAdressesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ShippingAdressesButtonClickEvent);
            await ShippingAdressesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ShippingAdressesButtonClick } });
        }

        public async void ShippingAdressesButtonClickEvent()
        {
            SelectShippingAdressesPopupVisible = true;
            ShippingAdressesList = (await ShippingAdressesAppService.GetListAsync(new ListShippingAdressesParameterDto())).Data.Where(t => t.CustomerCardCode == DataSource.CurrentAccountCardCode && t.CustomerCardName == DataSource.CurrentAccountCardName).ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void ShippingAdressesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ShippingAdressID = Guid.Empty;
                DataSource.ShippingAdressCode = string.Empty;
            }
        }

        public async void ShippingAdressesDoubleClickHandler(RecordDoubleClickEventArgs<ListShippingAdressesDto> args)
        {
            var selectedShippingAdress = args.RowData;

            if (selectedShippingAdress != null)
            {
                DataSource.ShippingAdressID = selectedShippingAdress.Id;
                DataSource.ShippingAdressCode = selectedShippingAdress.Code;
                SelectShippingAdressesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
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
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
                LineDataSource.VATrate = 0;
                LineDataSource.ProductGroupID = Guid.Empty;
                LineDataSource.ProductGroupName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                LineDataSource.ProductID = selectedProduct.Id;
                LineDataSource.ProductCode = selectedProduct.Code;
                LineDataSource.ProductName = selectedProduct.Name;
                LineDataSource.UnitSetID = selectedProduct.UnitSetID;
                LineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                LineDataSource.VATrate = selectedProduct.SaleVAT;
                LineDataSource.ProductGroupID = selectedProduct.ProductGrpID;
                LineDataSource.ProductGroupName = selectedProduct.ProductGrp;


                if (DataSource.CurrentAccountCardID != Guid.Empty && DataSource.CurrentAccountCardID != null)
                {

                    var definedPrice = (await SalesPricesAppService.GetDefinedProductPriceAsync(selectedProduct.Id, DataSource.CurrentAccountCardID, DataSource.CurrencyID, true, DataSource.Date_)).Data;

                    if (definedPrice.Id != Guid.Empty)
                    {
                        LineDataSource.UnitPrice = definedPrice.Price;
                    }
                }
                SelectProductsPopupVisible = false;

                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = SalesOrdersAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "SalesOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            OrderLinesWithSemiProductContextMenuItems();
            CreateSalesInvoicesContextMenuItems();

            await GetProductsList();

            futureDateParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.FutureDateParameter;

            MaxDate = !futureDateParameter ? GetSQLDateAppService.GetDateFromSQL() : new DateTime(9999, 12, 31);
        }

        #region Reçete Satırları Modalı İşlemleri


        public async void OnBoMLineContextMenuClick(ContextMenuClickEventArgs<ProductsTreeDto> args)
        {
            //switch (args.Item.Id)
            //{
            //    case "productstree":

            //        break;

            //    default:
            //        break;
            //}

            await Task.CompletedTask;
        }


        public void HideBoMLinePopup()
        {
            CreateProductionOrderCrudPopup = false;
        }

        #endregion

        #region Üretim Emri Oluşturma Modalı İşlemleri


        List<ProductsTreeDto> ProductTreeDataSource = new List<ProductsTreeDto>();

        public class ProductsTreeDto
        {
            public string ProductName { get; set; }
            public string ProductCode { get; set; }
            public int SupplyForm { get; set; }
            public decimal AmountofStock { get; set; }
            public decimal AmountofRequierement { get; set; }
        }

        SfGrid<PlannedProductionOrdersDto> _PlannedProductionOrdersGrid;

        List<PlannedProductionOrdersDto> PlannedProductionOrdersList = new List<PlannedProductionOrdersDto>();
        public class PlannedProductionOrdersDto
        {
            public Guid OrderLineID { get; set; }
            public bool isStockUsage { get; set; }
            public Guid ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal StockUsage { get; set; }
            public decimal PlannedQuantity { get; set; }
            public DateTime LoadingDate { get; set; }
            public Guid LinkedProductID { get; set; }
            public string LinkedProductCode { get; set; }
            public string LinkedProductName { get; set; }
            public Guid BomID { get; set; }
            public Guid RouteID { get; set; }
            public Guid UnitSetID { get; set; }
            public Guid TechnicalDrawingID { get; set; }
            public ProductTypeEnum ProductType { get; set; }
            public Guid ProductGroupID { get; set; }
            public int LineNr { get; set; }

        }


        SfGrid<OrderLinesWithSemiProductsDto> _OrderLinesWithSemiProductsGrid;

        List<OrderLinesWithSemiProductsDto> OrderLinesWithSemiProductsList = new List<OrderLinesWithSemiProductsDto>();

        public class OrderLinesWithSemiProductsDto
        {
            public Guid OrderLineID { get; set; }
            public bool isStockUsage { get; set; }
            public DateTime LoadingDate { get; set; }
            public int LineNr { get; set; }
            public bool isBoM { get; set; }
            public bool isRoute { get; set; }
            public bool isTechnicalDrawing { get; set; }
            public bool isStandart { get; set; }
            public Guid ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal StockQuantity { get; set; }
            public decimal TotalReservedQuantity { get; set; }
            public decimal AvailableStock { get; set; }
            public decimal ProductionOrderQuantity { get; set; }
            public decimal ProductionQuantity { get; set; }
            public decimal StockUsage { get; set; }
            public Guid LinkedProductID { get; set; }
            public string LinkedProductCode { get; set; }
            public string LinkedProductName { get; set; }
            public Guid BomID { get; set; }
            public Guid RouteID { get; set; }
            public Guid UnitSetID { get; set; }
            public Guid TechnicalDrawingID { get; set; }
            public ProductTypeEnum ProductType { get; set; }
            public SalesOrderLineStateEnum SalesOrderLineState { get; set; }
            public Guid ProductGroupID { get; set; }
        }

        protected void OrderLinesWithSemiProductContextMenuItems()
        {
            if (OrderLinesWithSemiProductsGridContextMenu.Count() == 0)
            {
                OrderLinesWithSemiProductsGridContextMenu.Add(new ContextMenuItemModel { Text = L["OrderLinesWithSemiProductsGridContextStockUsage"], Id = "stockusage" });
            }
        }

        public async void OnOrderLinesWithSemiProductsContextMenuClick(ContextMenuClickEventArgs<OrderLinesWithSemiProductsDto> args)
        {
            switch (args.Item.Id)
            {
                case "stockusage":
                    var line = args.RowInfo.RowData;

                    if (!line.isStockUsage)
                    {
                        line.isStockUsage = true;

                        if (line.ProductionOrderQuantity < line.StockQuantity)
                        {
                            line.StockQuantity = line.StockQuantity - line.ProductionOrderQuantity;
                            line.TotalReservedQuantity += line.ProductionOrderQuantity;
                            line.StockUsage = line.ProductionOrderQuantity;
                            line.AvailableStock -= line.ProductionOrderQuantity;
                            line.ProductionQuantity -= line.ProductionOrderQuantity;
                        }
                        else if (line.ProductionOrderQuantity > line.StockQuantity)
                        {

                            line.ProductionQuantity = line.ProductionOrderQuantity - line.StockQuantity;
                            line.TotalReservedQuantity += line.StockQuantity;
                            line.StockUsage = line.StockQuantity;
                            line.StockQuantity = 0;
                            line.AvailableStock = 0;
                        }
                        else if (line.ProductionOrderQuantity == line.StockQuantity)
                        {
                            line.ProductionQuantity = line.ProductionOrderQuantity;
                            line.TotalReservedQuantity += line.ProductionOrderQuantity;
                            line.StockUsage = line.ProductionOrderQuantity;
                            line.StockQuantity = 0;
                            line.AvailableStock = 0;

                        }

                        int lineIndex = OrderLinesWithSemiProductsList.IndexOf(line);

                        OrderLinesWithSemiProductsList[lineIndex] = line;

                        var otherList = OrderLinesWithSemiProductsList.Where(t => t.ProductID == line.ProductID && t.LineNr != line.LineNr).ToList();

                        foreach (var item in otherList)
                        {
                            item.StockQuantity = 0;
                            item.AvailableStock = 0;
                        }

                        await _OrderLinesWithSemiProductsGrid.Refresh();
                    }

                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }

            await Task.CompletedTask;

        }


        protected async Task OnCreateProductionOrderBtnClicked()
        {
            SpinnerService.Show();
            await Task.Delay(100);

            var now = GetSQLDateAppService.GetDateFromSQL();

            foreach (var mmPlannedProductionOrder in PlannedProductionOrdersList.Where(t => t.ProductType == ProductTypeEnum.MM).ToList())
            {
                SelectProductionOrdersDto mmProductionOrderModel = new SelectProductionOrdersDto
                {
                    OrderID = DataSource.Id,
                    FinishedProductID = mmPlannedProductionOrder.ProductID,
                    LinkedProductID = mmPlannedProductionOrder.LinkedProductID,
                    PlannedQuantity = mmPlannedProductionOrder.PlannedQuantity,
                    ProducedQuantity = 0,
                    CurrentAccountID = DataSource.CurrentAccountCardID,
                    CurrentAccountCode = DataSource.CurrentAccountCardCode,
                    CustomerOrderNo = DataSource.CustomerOrderNr,
                    CurrentAccountName = DataSource.CurrentAccountCardName,
                    BOMID = mmPlannedProductionOrder.BomID,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ProductionOrdersChildMenu"),
                    Description_ = string.Empty,
                    LinkedProductCode = string.Empty,
                    LinkedProductName = string.Empty,
                    LinkedProductionOrderID = Guid.Empty,
                    LinkedProductionOrderFicheNo = string.Empty,
                    BranchID = DataSource.BranchID,
                    BranchCode = DataSource.BranchCode,
                    Cancel_ = false,
                    Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                    ConfirmedLoadingDate = DataSource.ConfirmedLoadingDate,
                    FinishedProductCode = mmPlannedProductionOrder.ProductCode,
                    FinishedProductName = mmPlannedProductionOrder.ProductName,
                    WarehouseID = DataSource.WarehouseID,
                    WarehouseCode = DataSource.WarehouseCode,
                    UnitSetID = mmPlannedProductionOrder.UnitSetID,
                    RouteID = mmPlannedProductionOrder.RouteID,
                    OrderLineID = mmPlannedProductionOrder.OrderLineID,
                    ProductTreeID = Guid.Empty,
                    ProductionOrderState = Entities.Enums.ProductionOrderStateEnum.Baslamadi,
                    ProductTreeLineID = Guid.Empty,
                    PropositionID = Guid.Empty,
                    PropositionLineID = Guid.Empty,
                    TechnicalDrawingID = mmPlannedProductionOrder.TechnicalDrawingID,
                    TechnicalDrawingUpdateDate_ = null,
                    TechnicalDrawingUpdateDescription_ = string.Empty,
                    CreationTime = now,
                    ProductGroupID = mmPlannedProductionOrder.ProductGroupID,
                    CustomerRequestedDate = DataSource.CustomerRequestedDate.GetValueOrDefault(),
                    isReserve = false,

                };

                var mmConvertedInput = ObjectMapper.Map<SelectProductionOrdersDto, CreateProductionOrdersDto>(mmProductionOrderModel);

                var insertedMMProductionOrder = (await ProductionOrdersAppService.ConverttoProductionOrder(mmConvertedInput)).Data;

                #region MM Stock Reserved Fiche
                if (mmPlannedProductionOrder.isStockUsage)
                {
                    decimal transactionExchangeLineAmount = 0;
                    decimal lineTotalAmount = 0;
                    decimal transactionExchangeUnitPrice = 0;
                    decimal unitPrice = 0;

                    var selectedLine = DataSource.SelectSalesOrderLines.Where(t => t.ProductID == mmPlannedProductionOrder.ProductID).FirstOrDefault();

                    if (selectedLine != null)
                    {
                        transactionExchangeLineAmount = selectedLine.TransactionExchangeLineAmount;
                        lineTotalAmount = selectedLine.LineTotalAmount;
                        transactionExchangeUnitPrice = selectedLine.TransactionExchangeUnitPrice;
                        unitPrice = selectedLine.UnitPrice;
                    }
                    else
                    {
                        var salesPriceLineDataSource = (await SalesPricesAppService.GetDefinedProductPriceAsync(mmPlannedProductionOrder.ProductID, DataSource.CurrentAccountCardID, DataSource.CurrencyID, true, now.Date)).Data;

                        if (salesPriceLineDataSource != null && salesPriceLineDataSource.Id != Guid.Empty)
                        {
                            unitPrice = salesPriceLineDataSource.Price;
                        }
                    }

                    SelectStockFichesDto StockFicheDataSource = new SelectStockFichesDto
                    {
                        ProductionOrderID = insertedMMProductionOrder.Id,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                        Description_ = string.Empty,
                        FicheType = StockFicheTypeEnum.StokRezerveFisi,
                        InputOutputCode = 1,
                        NetAmount = DataSource.NetAmount,
                        ExchangeRate = DataSource.ExchangeRate,
                        BranchID = DataSource.BranchID,
                        CurrencyID = DataSource.CurrencyID,
                        Date_ = now.Date,
                        ProductionDateReferenceID = Guid.Empty,
                        PurchaseOrderID = Guid.Empty,
                        PurchaseRequestID = Guid.Empty,
                        SpecialCode = DataSource.SpecialCode,
                        WarehouseID = DataSource.WarehouseID,
                        TransactionExchangeCurrencyID = DataSource.TransactionExchangeCurrencyID,
                        Time_ = now.TimeOfDay,
                    };

                    StockFicheDataSource.SelectStockFicheLines = new List<SelectStockFicheLinesDto>();


                    SelectStockFicheLinesDto StockFicheLineModel = new SelectStockFicheLinesDto
                    {
                        Date_ = now.Date,
                        FicheType = StockFicheTypeEnum.StokRezerveFisi,
                        InputOutputCode = 1,
                        LineDescription = string.Empty,
                        LineAmount = lineTotalAmount,
                        LineNr = StockFicheDataSource.SelectStockFicheLines.Count + 1,
                        ProductID = mmPlannedProductionOrder.ProductID,
                        MRPID = Guid.Empty,
                        MRPLineID = Guid.Empty,
                        PartyNo = string.Empty,
                        ProductionDateReferenceID = Guid.Empty,
                        ProductionOrderID = insertedMMProductionOrder.Id,
                        PurchaseOrderID = Guid.Empty,
                        PurchaseOrderLineID = Guid.Empty,
                        Quantity = mmPlannedProductionOrder.StockUsage,
                        TransactionExchangeLineAmount = transactionExchangeLineAmount,
                        TransactionExchangeUnitPrice = transactionExchangeUnitPrice,
                        UnitPrice = unitPrice,
                        UnitSetID = mmPlannedProductionOrder.UnitSetID,
                        UnitOutputCost = 0
                    };

                    StockFicheDataSource.SelectStockFicheLines.Add(StockFicheLineModel);

                    var createStockFicheInput = ObjectMapper.Map<SelectStockFichesDto, CreateStockFichesDto>(StockFicheDataSource);

                    await StockFichesAppService.CreateAsync(createStockFicheInput);
                }
                #endregion

                //var mmPlannedList = PlannedProductionOrdersList.Where(t => t.ProductType == ProductTypeEnum.MM).ToList();


                //var nextMM = mmPlannedList[mmPlannedList.IndexOf(mmPlannedProductionOrder) + 1];

                //int firstMMIndex = PlannedProductionOrdersList.IndexOf(mmPlannedProductionOrder);

                //int secondMMIndex = PlannedProductionOrdersList.IndexOf(nextMM);

                //var ymProductionOrderList =

                var ymProductionOrderList = PlannedProductionOrdersList.Where(t => t.LinkedProductID == mmPlannedProductionOrder.ProductID && t.LineNr == mmPlannedProductionOrder.LineNr).ToList();

                foreach (var ymPlannedProductionOrder in ymProductionOrderList)
                {
                    SelectProductionOrdersDto ymProductionOrderModel = new SelectProductionOrdersDto
                    {
                        OrderID = DataSource.Id,
                        FinishedProductID = ymPlannedProductionOrder.ProductID,
                        LinkedProductID = ymPlannedProductionOrder.LinkedProductID,
                        PlannedQuantity = ymPlannedProductionOrder.PlannedQuantity,
                        ProducedQuantity = 0,
                        CustomerOrderNo = DataSource.CustomerOrderNr,
                        CurrentAccountID = DataSource.CurrentAccountCardID,
                        CurrentAccountCode = DataSource.CurrentAccountCardCode,
                        CurrentAccountName = DataSource.CurrentAccountCardName,
                        BOMID = ymPlannedProductionOrder.BomID,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ProductionOrdersChildMenu"),
                        Description_ = string.Empty,
                        LinkedProductCode = mmPlannedProductionOrder.ProductCode,
                        LinkedProductName = mmPlannedProductionOrder.ProductName,
                        LinkedProductionOrderID = insertedMMProductionOrder.Id,
                        LinkedProductionOrderFicheNo = insertedMMProductionOrder.FicheNo,
                        BranchID = DataSource.BranchID,
                        BranchCode = DataSource.BranchCode,
                        Cancel_ = false,
                        Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                        ConfirmedLoadingDate = DataSource.ConfirmedLoadingDate,
                        FinishedProductCode = ymPlannedProductionOrder.ProductCode,
                        FinishedProductName = ymPlannedProductionOrder.ProductName,
                        WarehouseID = DataSource.WarehouseID,
                        WarehouseCode = DataSource.WarehouseCode,
                        UnitSetID = ymPlannedProductionOrder.UnitSetID,
                        RouteID = ymPlannedProductionOrder.RouteID,
                        OrderLineID = ymPlannedProductionOrder.OrderLineID,
                        ProductTreeID = Guid.Empty,
                        ProductionOrderState = Entities.Enums.ProductionOrderStateEnum.Baslamadi,
                        ProductTreeLineID = Guid.Empty,
                        PropositionID = Guid.Empty,
                        PropositionLineID = Guid.Empty,
                        TechnicalDrawingID = ymPlannedProductionOrder.TechnicalDrawingID,
                        TechnicalDrawingUpdateDate_ = null,
                        TechnicalDrawingUpdateDescription_ = string.Empty,
                        ProductGroupID = ymPlannedProductionOrder.ProductGroupID,
                        CustomerRequestedDate = DataSource.CustomerRequestedDate.GetValueOrDefault(),
                        isReserve = false
                    };

                    var ymConvertedInput = ObjectMapper.Map<SelectProductionOrdersDto, CreateProductionOrdersDto>(ymProductionOrderModel);

                    var insertedYMProductionOrder = (await ProductionOrdersAppService.ConverttoProductionOrder(ymConvertedInput)).Data;

                    #region YM Stock Reserved Fiche
                    if (ymPlannedProductionOrder.isStockUsage)
                    {

                        decimal transactionExchangeLineAmount = 0;
                        decimal lineTotalAmount = 0;
                        decimal transactionExchangeUnitPrice = 0;
                        decimal unitPrice = 0;

                        var selectedLine = DataSource.SelectSalesOrderLines.Where(t => t.ProductID == ymPlannedProductionOrder.ProductID).FirstOrDefault();

                        if (selectedLine != null)
                        {
                            transactionExchangeLineAmount = selectedLine.TransactionExchangeLineAmount;
                            lineTotalAmount = selectedLine.LineTotalAmount;
                            transactionExchangeUnitPrice = selectedLine.TransactionExchangeUnitPrice;
                            unitPrice = selectedLine.UnitPrice;
                        }
                        else
                        {
                            var salesPriceLineDataSource = (await SalesPricesAppService.GetDefinedProductPriceAsync(ymPlannedProductionOrder.ProductID, DataSource.CurrentAccountCardID, DataSource.CurrencyID, true, now.Date)).Data;

                            if (salesPriceLineDataSource != null && salesPriceLineDataSource.Id != Guid.Empty)
                            {
                                unitPrice = salesPriceLineDataSource.Price;
                            }
                        }

                        SelectStockFichesDto StockFicheDataSource = new SelectStockFichesDto
                        {
                            ProductionOrderID = insertedYMProductionOrder.Id,
                            FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                            Description_ = string.Empty,
                            FicheType = StockFicheTypeEnum.StokRezerveFisi,
                            InputOutputCode = 0,
                            NetAmount = DataSource.NetAmount,
                            ExchangeRate = DataSource.ExchangeRate,
                            BranchID = DataSource.BranchID,
                            CurrencyID = DataSource.CurrencyID,
                            Date_ = now.Date,
                            ProductionDateReferenceID = Guid.Empty,
                            PurchaseOrderID = Guid.Empty,
                            PurchaseRequestID = Guid.Empty,
                            SpecialCode = DataSource.SpecialCode,
                            WarehouseID = DataSource.WarehouseID,
                            TransactionExchangeCurrencyID = DataSource.TransactionExchangeCurrencyID,
                            Time_ = now.TimeOfDay,

                        };

                        StockFicheDataSource.SelectStockFicheLines = new List<SelectStockFicheLinesDto>();


                        SelectStockFicheLinesDto StockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            Date_ = now.Date,
                            FicheType = StockFicheTypeEnum.StokRezerveFisi,
                            InputOutputCode = 0,
                            LineDescription = string.Empty,
                            LineAmount = lineTotalAmount,
                            LineNr = StockFicheDataSource.SelectStockFicheLines.Count + 1,
                            ProductID = ymPlannedProductionOrder.ProductID,
                            MRPID = Guid.Empty,
                            MRPLineID = Guid.Empty,
                            PartyNo = string.Empty,
                            ProductionDateReferenceID = Guid.Empty,
                            ProductionOrderID = insertedYMProductionOrder.Id,
                            PurchaseOrderID = Guid.Empty,
                            PurchaseOrderLineID = Guid.Empty,
                            Quantity = ymPlannedProductionOrder.StockUsage,
                            TransactionExchangeLineAmount = transactionExchangeLineAmount,
                            TransactionExchangeUnitPrice = transactionExchangeUnitPrice,
                            UnitPrice = unitPrice,
                            UnitSetID = ymPlannedProductionOrder.UnitSetID,
                            UnitOutputCost = 0
                        };

                        StockFicheDataSource.SelectStockFicheLines.Add(StockFicheLineModel);

                        var createStockFicheInput = ObjectMapper.Map<SelectStockFichesDto, CreateStockFichesDto>(StockFicheDataSource);

                        await StockFichesAppService.CreateAsync(createStockFicheInput);
                    }
                    #endregion
                }
            }

            SpinnerService.Hide();

            await ModalManager.MessagePopupAsync(L["UIMessageCreateProdOrderTitle"], L["UIMessageCreateProdOrderMessage"]);

            CreateProductionOrderCrudPopup = true;

        }

        public void HideCreateProductionOrderPopup()
        {
            CreateProductionOrderCrudPopup = false;
            InvokeAsync(StateHasChanged);
        }

        public async void CreateProductionOrderListClicked()
        {

            if (OrderLinesWithSemiProductsList.Any(t => !t.isRoute))
            {
                await ModalManager.MessagePopupAsync(L["UIWarningCreateProdOrderTitle"], L["UIWarningCreateProdOrderMessage"]);
                return;
            }

            if (OrderLinesWithSemiProductsList.Any(t => !t.isBoM))
            {
                await ModalManager.MessagePopupAsync(L["UIWarningCreateProdOrderTitle"], L["UIWarningCreateProdOrderMessage2"]);
                return;
            }

            if (OrderLinesWithSemiProductsList.Any(t => !t.isTechnicalDrawing))
            {
                await ModalManager.MessagePopupAsync(L["UIWarningCreateProdOrderTitle"], L["UIWarningCreateProdOrderMessage3"]);
                return;
            }

            PlannedProductionOrdersList.Clear();


            foreach (var item in OrderLinesWithSemiProductsList)
            {
                if (DataSource.isStandart == true)
                {

                    if (item.isStandart)
                    {
                        PlannedProductionOrdersDto plannedProductionOrdersModel = new PlannedProductionOrdersDto
                        {
                            isStockUsage = item.isStockUsage,
                            LinkedProductCode = item.LinkedProductCode,
                            LinkedProductID = item.LinkedProductID,
                            LinkedProductName = item.LinkedProductName,
                            LoadingDate = item.LoadingDate,
                            PlannedQuantity = item.ProductionQuantity,
                            ProductCode = item.ProductCode,
                            ProductID = item.ProductID,
                            ProductName = item.ProductName,
                            StockUsage = item.StockUsage,
                            OrderLineID = item.OrderLineID,
                            BomID = item.BomID,
                            RouteID = item.RouteID,
                            UnitSetID = item.UnitSetID,
                            TechnicalDrawingID = item.TechnicalDrawingID,
                            ProductType = item.ProductType,
                            ProductGroupID = item.ProductGroupID,
                            LineNr = item.LineNr
                        };

                        PlannedProductionOrdersList.Add(plannedProductionOrdersModel);
                    }


                }
                else
                {

                    PlannedProductionOrdersDto plannedProductionOrdersModel = new PlannedProductionOrdersDto
                    {
                        isStockUsage = item.isStockUsage,
                        LinkedProductCode = item.LinkedProductCode,
                        LinkedProductID = item.LinkedProductID,
                        LinkedProductName = item.LinkedProductName,
                        LoadingDate = item.LoadingDate,
                        PlannedQuantity = item.ProductionQuantity,
                        ProductCode = item.ProductCode,
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        StockUsage = item.StockUsage,
                        OrderLineID = item.OrderLineID,
                        BomID = item.BomID,
                        RouteID = item.RouteID,
                        UnitSetID = item.UnitSetID,
                        TechnicalDrawingID = item.TechnicalDrawingID,
                        ProductType = item.ProductType,
                        ProductGroupID = item.ProductGroupID,
                        LineNr = item.LineNr
                    };

                    PlannedProductionOrdersList.Add(plannedProductionOrdersModel);

                }

            }

            //PlannedProductionOrdersList.Clear();
            await _PlannedProductionOrdersGrid.Refresh();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Teklif Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            var salesManagementParameter = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data;

            DataSource = new SelectSalesOrderDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesOrdersChildMenu"),
                CustomerRequestedDate = GetSQLDateAppService.GetDateFromSQL().Date,
                BranchID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchID : Guid.Empty,
                WarehouseID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseID : Guid.Empty,
                BranchCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchCode : string.Empty,
                BranchName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchName : string.Empty,
                WarehouseCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseCode : string.Empty,
                WarehouseName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseName : string.Empty,
                SalesOrderState = Entities.Enums.SalesOrderStateEnum.Beklemede,

            };

            DataSource.SelectSalesOrderLines = new List<SelectSalesOrderLinesDto>();
            GridLineList = DataSource.SelectSalesOrderLines;


            var localCurrency = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.Where(t => t.IsLocalCurrency == true).FirstOrDefault();

            if (localCurrency != null && localCurrency.Id != Guid.Empty)
            {
                DataSource.CurrencyID = localCurrency.Id;
                DataSource.CurrencyCode = localCurrency.Code;
            }

            foreach (var item in PricingCurrencyList)
            {
                item.PricingCurrencyName = L[item.PricingCurrencyName];
            }

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
                            case "SalesOrderLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderLineContextAdd"], Id = "new" }); break;
                            case "SalesOrderLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderLineContextChange"], Id = "changed" }); break;
                            case "SalesOrderLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderLineContextDelete"], Id = "delete" }); break;
                            case "SalesOrderLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderLineContextRefresh"], Id = "refresh" }); break;
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
                            case "SalesOrderContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextAdd"], Id = "new" }); break;
                            case "SalesOrderContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextChange"], Id = "changed" }); break;
                            case "SalesOrderContextProdOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextProdOrder"], Id = "createproductionorder" }); break;
                            case "SalesOrderContextStatus":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "SalesOrderContextOrderApproval":
                                                subMenus.Add(new MenuItem { Text = L["SalesOrderContextOrderApproval"], Id = "orderapproval" }); break;
                                            case "SalesOrderContextPending":
                                                subMenus.Add(new MenuItem { Text = L["SalesOrderContextPending"], Id = "pending" }); break;
                                            case "SalesOrderContextAvailableProduction":
                                                subMenus.Add(new MenuItem { Text = L["SalesOrderContextAvailableProduction"], Id = "availableproduction" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextStatus"], Id = "status", Items = subMenus }); break;

                            case "SalesOrderContextSalesInvoice":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextSalesInvoice"], Id = "createsalesinvoice" }); break;
                            case "SalesOrderContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextDelete"], Id = "delete" }); break;
                            case "SalesOrderContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesOrderContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
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

                    #region Fiyatlandırma Dövizi

                    if (DataSource.PricingCurrency == PricingCurrencyEnum.LocalCurrency)
                    {
                        UnitPriceEnabled = true;
                        DiscountAmountEnabled = true;
                        LineAmountEnabled = true;
                        LineTotalAmountEnabled = true;

                        TransactionExchangeUnitPriceEnabled = false;
                        TransactionExchangeDiscountAmountEnabled = false;
                        TransactionExchangeLineAmountEnabled = false;
                        TransactionExchangeLineTotalAmountEnabled = false;
                    }
                    else if (DataSource.PricingCurrency == PricingCurrencyEnum.TransactionCurrency)
                    {
                        UnitPriceEnabled = false;
                        DiscountAmountEnabled = false;
                        LineAmountEnabled = false;
                        LineTotalAmountEnabled = false;

                        TransactionExchangeUnitPriceEnabled = true;
                        TransactionExchangeDiscountAmountEnabled = true;
                        TransactionExchangeLineAmountEnabled = true;
                        TransactionExchangeLineTotalAmountEnabled = true;
                    }
                    else
                    {
                        UnitPriceEnabled = false;
                        DiscountAmountEnabled = false;
                        LineAmountEnabled = false;
                        LineTotalAmountEnabled = false;

                        TransactionExchangeUnitPriceEnabled = false;
                        TransactionExchangeDiscountAmountEnabled = false;
                        TransactionExchangeLineAmountEnabled = false;
                        TransactionExchangeLineTotalAmountEnabled = false;
                    }

                    #endregion

                    foreach (var item in PricingCurrencyList)
                    {
                        item.PricingCurrencyName = L[item.PricingCurrencyName];
                    }

                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListSalesOrderDto> args)
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
                        DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineList = DataSource.SelectSalesOrderLines;


                        ShowEditPage();
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

                case "createproductionorder":
                    if (args.RowInfo.RowData != null)
                    {

                        SpinnerService.Show();
                        await Task.Delay(100);

                        DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        if (DataSource.SalesOrderState == SalesOrderStateEnum.UretimeVerilebilir)
                        {
                            OrderLinesWithSemiProductsList.Clear();

                            foreach (var mmItem in DataSource.SelectSalesOrderLines)
                            {
                                bool isbom = false;
                                bool isroute = false;
                                bool istechnicaldrawing = false;
                                bool isstandart = false;

                                var mmItemRoute = (await RoutesAppService.GetbyProductIDAsync(mmItem.ProductID.GetValueOrDefault())).Data;

                                if (mmItemRoute != null && mmItemRoute.Id != Guid.Empty)
                                {
                                    isroute = true;
                                }


                                var mmItemTechnicalDrawing = (await TechnicalDrawingsAppService.GetbyProductIDAsync(mmItem.ProductID.GetValueOrDefault())).Data;

                                if (mmItemTechnicalDrawing != null && mmItemTechnicalDrawing.Id != Guid.Empty)
                                {
                                    istechnicaldrawing = true;
                                }

                                var mmItemBom = (await BillsofMaterialsAppService.GetbyProductIDAsync(mmItem.ProductID.GetValueOrDefault())).Data;

                                if (mmItemBom != null && mmItemBom.Id != Guid.Empty)
                                {
                                    isbom = true;
                                }

                                var mmItemStandart = (await ProductsAppService.GetAsync(mmItem.ProductID.GetValueOrDefault())).Data;

                                if (mmItemStandart != null && mmItemStandart.Id != Guid.Empty)
                                {
                                    isstandart = mmItemStandart.isStandart;

                                }

                                decimal stockQuantity = 0;
                                decimal totalReserved = 0;
                                decimal availableStock = 0;

                                var grandTotalListofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == mmItem.ProductID).ToList();

                                if (grandTotalListofProduct != null && grandTotalListofProduct.Count > 0)
                                {
                                    stockQuantity = grandTotalListofProduct.Sum(t => t.Amount);
                                    totalReserved = grandTotalListofProduct.Sum(t => t.TotalReserved);
                                    availableStock = stockQuantity - totalReserved;
                                }

                                var mmTechnicalDrawing = (await TechnicalDrawingsAppService.GetSelectListAsync(mmItem.ProductID.GetValueOrDefault())).Data.Where(t => t.CustomerApproval == true && t.IsApproved == true).LastOrDefault();

                                OrderLinesWithSemiProductsDto orderLinesWithSemiProductsModel = new OrderLinesWithSemiProductsDto
                                {
                                    ProductCode = mmItem.ProductCode,
                                    ProductID = mmItem.ProductID.GetValueOrDefault(),
                                    ProductName = mmItem.ProductName,
                                    OrderLineID = mmItem.Id,
                                    isBoM = isbom,
                                    isStockUsage = false,
                                    isRoute = isroute,
                                    isTechnicalDrawing = istechnicaldrawing,
                                    isStandart = isstandart,
                                    LinkedProductID = Guid.Empty,
                                    LinkedProductCode = "-",
                                    LinkedProductName = "-",
                                    LoadingDate = DataSource.ConfirmedLoadingDate.GetValueOrDefault(),
                                    LineNr = OrderLinesWithSemiProductsList.Count + 1,
                                    SalesOrderLineState = mmItem.SalesOrderLineStateEnum,
                                    StockUsage = 0,
                                    ProductionOrderQuantity = mmItem.Quantity,
                                    ProductionQuantity = mmItem.Quantity,
                                    AvailableStock = availableStock,
                                    StockQuantity = stockQuantity,
                                    TotalReservedQuantity = totalReserved,
                                    BomID = mmItemBom.Id,
                                    RouteID = mmItemRoute.Id,
                                    UnitSetID = mmItem.UnitSetID.GetValueOrDefault(),
                                    TechnicalDrawingID = mmItemTechnicalDrawing.Id,
                                    ProductType = ProductTypeEnum.MM,
                                    ProductGroupID = mmItemBom.FinishedProductGroupID.GetValueOrDefault()
                                };

                                OrderLinesWithSemiProductsList.Add(orderLinesWithSemiProductsModel);

                                var ymList = mmItemBom.SelectBillsofMaterialLines.Where(t => t.SupplyForm == ProductSupplyFormEnum.Üretim).ToList();

                                if (ymList != null && ymList.Count > 0)
                                {
                                    foreach (var ymItem in ymList)
                                    {
                                        bool lineisbom = false;
                                        bool lineistechnicaldrawing = false;
                                        bool lineisroute = false;
                                        bool lineisstandart = false;

                                        decimal lineQuantity = mmItem.Quantity * ymItem.Quantity;

                                        var linebomDataSource = (await BillsofMaterialsAppService.GetbyProductIDAsync(ymItem.ProductID.GetValueOrDefault())).Data;

                                        if (linebomDataSource != null && linebomDataSource.Id != Guid.Empty)
                                        {
                                            lineisbom = true;
                                        }

                                        var linetechnicaldrawingDataSource = (await TechnicalDrawingsAppService.GetbyProductIDAsync(ymItem.ProductID.GetValueOrDefault())).Data;

                                        if (linetechnicaldrawingDataSource != null && linetechnicaldrawingDataSource.Id != Guid.Empty)
                                        {
                                            lineistechnicaldrawing = true;
                                        }

                                        var linestandartDataSource = (await ProductsAppService.GetbyProductIDAsync(ymItem.ProductID.GetValueOrDefault())).Data;

                                        if (linestandartDataSource != null && linestandartDataSource.Id != Guid.Empty)
                                        {
                                            lineisstandart = linestandartDataSource.isStandart;
                                        }

                                        var linerouteDataSource = (await RoutesAppService.GetbyProductIDAsync(ymItem.ProductID.GetValueOrDefault())).Data;

                                        if (linerouteDataSource != null && linerouteDataSource.Id != Guid.Empty)
                                        {
                                            lineisroute = true;
                                        }

                                        decimal linestockQuantity = 0;
                                        decimal linetotalReserved = 0;
                                        decimal lineavailableStock = 0;

                                        var linegrandTotalListofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == ymItem.ProductID).ToList();

                                        if (linegrandTotalListofProduct != null && linegrandTotalListofProduct.Count > 0)
                                        {
                                            linestockQuantity = linegrandTotalListofProduct.Sum(t => t.Amount);
                                            linetotalReserved = linegrandTotalListofProduct.Sum(t => t.TotalReserved);
                                            lineavailableStock = linestockQuantity - linetotalReserved;
                                        }


                                        var ymTechnicalDrawing = (await TechnicalDrawingsAppService.GetSelectListAsync(ymItem.ProductID.GetValueOrDefault())).Data.Where(t => t.CustomerApproval == true && t.IsApproved == true).LastOrDefault();

                                        OrderLinesWithSemiProductsDto lineorderLinesWithSemiProductsModel = new OrderLinesWithSemiProductsDto
                                        {
                                            ProductCode = ymItem.ProductCode,
                                            isStockUsage = false,
                                            ProductID = ymItem.ProductID.GetValueOrDefault(),
                                            ProductName = ymItem.ProductName,
                                            isBoM = lineisbom,
                                            isRoute = lineisroute,
                                            isTechnicalDrawing = lineistechnicaldrawing,
                                            isStandart = lineisstandart,
                                            OrderLineID = Guid.Empty,
                                            LinkedProductID = ymItem.FinishedProductID.GetValueOrDefault(),
                                            LinkedProductCode = ymItem.FinishedProductCode,
                                            //LinkedProductName = bomLine.FinishedProducName,
                                            LoadingDate = DataSource.ConfirmedLoadingDate.GetValueOrDefault(),
                                            LineNr = orderLinesWithSemiProductsModel.LineNr,
                                            SalesOrderLineState = mmItem.SalesOrderLineStateEnum,
                                            StockUsage = 0,
                                            ProductionOrderQuantity = lineQuantity,
                                            ProductionQuantity = lineQuantity,
                                            AvailableStock = lineavailableStock,
                                            StockQuantity = linestockQuantity,
                                            TotalReservedQuantity = linetotalReserved,
                                            BomID = linebomDataSource.Id,
                                            RouteID = linerouteDataSource.Id,
                                            UnitSetID = ymItem.UnitSetID.GetValueOrDefault(),
                                            TechnicalDrawingID = linetechnicaldrawingDataSource.Id,
                                            ProductType = ProductTypeEnum.YM,
                                            ProductGroupID = linebomDataSource.FinishedProductGroupID.GetValueOrDefault()
                                        };

                                        OrderLinesWithSemiProductsList.Add(lineorderLinesWithSemiProductsModel);

                                    }

                                    CreateProductionOrderCrudPopup = true;
                                }

                            }

                        }
                        else
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageBase3"]);
                        }
                    }

                    SpinnerService.Hide();

                    await InvokeAsync(StateHasChanged);

                    break;

                case "orderapproval":

                    if (args.RowInfo.RowData != null)
                    {
                        SpinnerService.Show();
                        await Task.Delay(100);

                        DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        if (DataSource.SalesOrderState != SalesOrderStateEnum.Onaylandı)
                        {
                            SpinnerService.Hide();

                            DataSource.SalesOrderState = Entities.Enums.SalesOrderStateEnum.Onaylandı;
                            var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(DataSource);
                            await SalesOrdersAppService.UpdateOrderApprovalAsync(updateInput);


                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                        }
                        else
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIWarningStateTitle"], L["UIWarningStateMessage"]);
                        }


                        await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "pending":
                    {
                        SpinnerService.Show();
                        await Task.Delay(100);

                        DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        if (DataSource.SalesOrderState != SalesOrderStateEnum.Beklemede)
                        {
                            SpinnerService.Hide();

                            DataSource.SalesOrderState = Entities.Enums.SalesOrderStateEnum.Beklemede;
                            var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(DataSource);
                            await SalesOrdersAppService.UpdatePendingAsync(updateInput);


                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                        }
                        else
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIWarningStateTitle"], L["UIWarningStateMessage2"]);
                        }

                    }

                    await InvokeAsync(StateHasChanged);
                    break;


                case "availableproduction":
                    {
                        SpinnerService.Show();
                        await Task.Delay(100);

                        DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        SpinnerService.Hide();

                        DataSource.SalesOrderState = Entities.Enums.SalesOrderStateEnum.UretimeVerilebilir;
                        var updateInput = ObjectMapper.Map<SelectSalesOrderDto, UpdateSalesOrderDto>(DataSource);
                        await SalesOrdersAppService.UpdateGiveProductionAsync(updateInput);


                        await GetListDataSourceAsync();
                        await _grid.Refresh();


                    }

                    await InvokeAsync(StateHasChanged);
                    break;


                case "createsalesinvoice":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineList = DataSource.SelectSalesOrderLines;

                        if (DataSource.SalesOrderState != Entities.Enums.SalesOrderStateEnum.Onaylandı)
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIWarningStockFichesTitle"], L["UIWarningStockFichesMessage"]);
                        }
                        else
                        {
                            if (DataSource.SalesOrderState == Entities.Enums.SalesOrderStateEnum.Onaylandı)
                            {

                                bool selectedLine = false;

                                foreach (var line in GridLineList)
                                {

                                    CreateSalesInvoices createSalesInvoiceModel = new CreateSalesInvoices
                                    {
                                        ProductCode = line.ProductCode,
                                        ProductID = line.ProductID,
                                        ProductName = line.ProductName,
                                        Quantity = line.Quantity,
                                        UnitSetCode = line.UnitSetCode,
                                        UnitSetID = line.UnitSetID,
                                        SelectedLine = selectedLine,
                                        LineID = line.Id,
                                        BranchCode = line.BranchCode,
                                        BranchID = line.BranchID,
                                        BranchName = line.BranchName,
                                        CurrentAccountCardCode = line.CurrentAccountCardCode,
                                        CurrentAccountCardID = line.CurrentAccountCardID,
                                        CurrentAccountCardName = line.CurrentAccountCardName,
                                        Date_ = line.Date_,
                                        DiscountAmount = line.DiscountAmount,
                                        DiscountRate = line.DiscountRate,
                                        ExchangeRate = line.ExchangeRate,
                                        LineAmount = line.LineAmount,
                                        LineDescription = string.Empty,
                                        LineNr = CreateSalesInvoicesList.Count + 1,
                                        LineTotalAmount = line.LineTotalAmount,
                                        PaymentPlanID = line.PaymentPlanID,
                                        PaymentPlanName = line.PaymentPlanName,
                                        TransactionExchangeDiscountAmount = line.TransactionExchangeDiscountAmount,
                                        TransactionExchangeLineAmount = line.TransactionExchangeLineAmount,
                                        TransactionExchangeLineTotalAmount = line.TransactionExchangeLineTotalAmount,
                                        TransactionExchangeUnitPrice = line.TransactionExchangeUnitPrice,
                                        TransactionExchangeVATamount = line.TransactionExchangeVATamount,
                                        UnitPrice = line.UnitPrice,
                                        VATamount = line.VATamount,
                                        VATrate = line.VATrate,
                                        WarehouseCode = line.WarehouseCode,
                                        WarehouseID = line.WarehouseID,
                                        WarehouseName = line.WarehouseName,
                                        WorkOrderCreationDate = line.WorkOrderCreationDate,
                                        LikedPropositionLineID = line.LikedPropositionLineID,
                                        LinkedSalesPropositionID = line.LinkedSalesPropositionID,
                                        OrderAcceptanceRecordID = line.OrderAcceptanceRecordID,
                                        OrderAcceptanceRecordLineID = line.OrderAcceptanceRecordLineID,
                                        ProductGroupID = line.ProductGroupID,
                                        ProductGroupName = line.ProductGroupName,
                                        PurchaseSupplyDate = line.PurchaseSupplyDate,
                                    };

                                    CreateSalesInvoicesList.Add(createSalesInvoiceModel);
                                }



                                SpinnerService.Hide();
                                CreateSalesInvoicesCrudPopup = true;




                            }
                        }


                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectSalesOrderLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (DataSource.PricingCurrency == PricingCurrencyEnum.LocalCurrency || DataSource.PricingCurrency == PricingCurrencyEnum.TransactionCurrency)
                    {
                        LineDataSource = new SelectSalesOrderLinesDto();
                        LineCrudPopup = true;
                        LineDataSource.PaymentPlanID = DataSource.PaymentPlanID;
                        LineDataSource.PaymentPlanName = DataSource.PaymentPlanName;
                        LineDataSource.LineNr = GridLineList.Count + 1;
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningTitleBase"], L["UIWarningMessageBase"]);
                    }
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {
                        LineDataSource = args.RowInfo.RowData;
                        LineCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                        if (res == true)
                        {
                            //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                            var line = args.RowInfo.RowData;

                            if (line.Id == Guid.Empty)
                            {
                                DataSource.SelectSalesOrderLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    DataSource.SelectSalesOrderLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    DataSource.SelectSalesOrderLines.Remove(line);
                                }
                            }

                            await _LineGrid.Refresh();
                            GetTotal();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.UnitSetID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase1"]);
            }
            else if (LineDataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase2"]);
            }
            else if (LineDataSource.Quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase3"]);
            }
            else
            {
                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectSalesOrderLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectSalesOrderLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectSalesOrderLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectSalesOrderLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectSalesOrderLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectSalesOrderLines[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectSalesOrderLines;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        public override async void LineCalculate()
        {
            if (DataSource.PricingCurrency == PricingCurrencyEnum.LocalCurrency)
            {
                LineDataSource.LineAmount = (LineDataSource.Quantity * LineDataSource.UnitPrice) - LineDataSource.DiscountAmount;
                LineDataSource.VATamount = (LineDataSource.LineAmount * LineDataSource.VATrate) / 100;

                if (LineDataSource.DiscountAmount > 0)
                {
                    if (LineDataSource.Quantity > 0 && LineDataSource.UnitPrice > 0)
                    {
                        LineDataSource.DiscountRate = (LineDataSource.DiscountAmount / (LineDataSource.Quantity * LineDataSource.UnitPrice)) * 100;
                    }

                    if (LineDataSource.Quantity == 0 || LineDataSource.UnitPrice == 0)
                    {
                        LineDataSource.DiscountRate = 0;
                        LineDataSource.DiscountAmount = 0;
                        LineDataSource.TransactionExchangeDiscountAmount = 0;
                    }
                }
                else
                {
                    LineDataSource.DiscountRate = 0;
                }

                LineDataSource.LineTotalAmount = LineDataSource.LineAmount + LineDataSource.VATamount;


                LineDataSource.TransactionExchangeUnitPrice = 0;
                LineDataSource.TransactionExchangeVATamount = 0;
                LineDataSource.TransactionExchangeDiscountAmount = 0;
                LineDataSource.TransactionExchangeLineAmount = 0;
                LineDataSource.TransactionExchangeLineTotalAmount = 0;
            }
            else
            {
                LineDataSource.TransactionExchangeLineAmount = (LineDataSource.Quantity * LineDataSource.TransactionExchangeUnitPrice) - LineDataSource.TransactionExchangeDiscountAmount;
                LineDataSource.TransactionExchangeVATamount = (LineDataSource.TransactionExchangeLineAmount * LineDataSource.VATrate) / 100;

                if (LineDataSource.TransactionExchangeDiscountAmount > 0)
                {
                    if (LineDataSource.Quantity > 0 && LineDataSource.TransactionExchangeUnitPrice > 0)
                    {
                        LineDataSource.DiscountRate = (LineDataSource.TransactionExchangeDiscountAmount / (LineDataSource.Quantity * LineDataSource.TransactionExchangeUnitPrice)) * 100;
                    }

                    if (LineDataSource.Quantity == 0 || LineDataSource.TransactionExchangeUnitPrice == 0)
                    {
                        LineDataSource.DiscountRate = 0;
                        LineDataSource.TransactionExchangeDiscountAmount = 0;
                        LineDataSource.TransactionExchangeDiscountAmount = 0;
                    }
                }
                else
                {
                    LineDataSource.DiscountRate = 0;
                }

                LineDataSource.TransactionExchangeLineTotalAmount = LineDataSource.TransactionExchangeLineAmount + LineDataSource.TransactionExchangeVATamount;


                LineDataSource.UnitPrice = LineDataSource.TransactionExchangeUnitPrice * DataSource.ExchangeRate;
                LineDataSource.VATamount = LineDataSource.TransactionExchangeVATamount * DataSource.ExchangeRate;
                LineDataSource.DiscountAmount = LineDataSource.TransactionExchangeDiscountAmount * DataSource.ExchangeRate;
                LineDataSource.LineAmount = LineDataSource.TransactionExchangeLineAmount * DataSource.ExchangeRate;
                LineDataSource.LineTotalAmount = LineDataSource.TransactionExchangeLineTotalAmount * DataSource.ExchangeRate;

            }

            await InvokeAsync(StateHasChanged);
        }

        public override void GetTotal()
        {
            DataSource.GrossAmount = GridLineList.Sum(x => x.LineAmount) + GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalDiscountAmount = GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalVatExcludedAmount = DataSource.GrossAmount - DataSource.TotalDiscountAmount;
            DataSource.TotalVatAmount = GridLineList.Sum(x => x.VATamount);
            DataSource.NetAmount = GridLineList.Sum(x => x.LineTotalAmount);

            DataSource.TransactionExchangeGrossAmount = GridLineList.Sum(x => x.TransactionExchangeLineAmount) + GridLineList.Sum(x => x.TransactionExchangeDiscountAmount);
            DataSource.TransactionExchangeTotalDiscountAmount = GridLineList.Sum(x => x.TransactionExchangeDiscountAmount);
            DataSource.TransactionExchangeTotalVatExcludedAmount = DataSource.TransactionExchangeGrossAmount - DataSource.TransactionExchangeTotalDiscountAmount;
            DataSource.TransactionExchangeTotalVatAmount = GridLineList.Sum(x => x.TransactionExchangeVATamount);
            DataSource.TransactionExchangeNetAmount = GridLineList.Sum(x => x.TransactionExchangeLineTotalAmount);
        }
        #endregion

        #region Fatura Oluşturma Modalı İşlemleri

        public class CreateSalesInvoices
        {
            public bool SelectedLine { get; set; }
            public Guid? LineID { get; set; }
            public Guid? ProductGroupID { get; set; }
            public string ProductGroupName { get; set; }
            public Guid? OrderAcceptanceRecordID { get; set; }
            public Guid? OrderAcceptanceRecordLineID { get; set; }
            public decimal TransactionExchangeUnitPrice { get; set; }
            public decimal TransactionExchangeVATamount { get; set; }
            public decimal TransactionExchangeLineAmount { get; set; }
            public decimal TransactionExchangeLineTotalAmount { get; set; }
            public decimal TransactionExchangeDiscountAmount { get; set; }
            public int LineNr { get; set; }
            public Guid? ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public Guid? UnitSetID { get; set; }
            public string UnitSetCode { get; set; }
            public Guid? LikedPropositionLineID { get; set; }
            public Guid? LinkedSalesPropositionID { get; set; }
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal DiscountRate { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal LineAmount { get; set; }
            public decimal LineTotalAmount { get; set; }
            public string LineDescription { get; set; }
            public int VATrate { get; set; }
            public decimal VATamount { get; set; }
            public decimal ExchangeRate { get; set; }
            public Guid? PaymentPlanID { get; set; }
            public string PaymentPlanName { get; set; }
            public DateTime? WorkOrderCreationDate { get; set; }
            public DateTime Date_ { get; set; }
            public Guid BranchID { get; set; }
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
            public Guid WarehouseID { get; set; }
            public string WarehouseCode { get; set; }
            public string WarehouseName { get; set; }
            public Guid CurrentAccountCardID { get; set; }
            public string CurrentAccountCardCode { get; set; }
            public string CurrentAccountCardName { get; set; }
            public DateTime? PurchaseSupplyDate { get; set; }
        }

        protected void CreateSalesInvoicesContextMenuItems()
        {
            if (CreateSalesInvoicesGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "SalesInvoicesContextSelect":
                                CreateSalesInvoicesGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoicesContextSelect"], Id = "select" }); break;
                            case "SalesInvoicesContextMultiSelect":
                                CreateSalesInvoicesGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoicesContextMultiSelect"], Id = "multiselect" }); break;
                            case "SalesInvoicesContextRemoveAll":
                                CreateSalesInvoicesGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoicesContextRemoveAll"], Id = "removeall" }); break;
                            default: break;
                        }
                    }
                }
            }
        }


        public async void OnCreateSalesInvoicesContextMenuClick(ContextMenuClickEventArgs<CreateSalesInvoices> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    if (args.RowInfo.RowData != null)
                    {

                        CreateSalesInvoicesDataSource = args.RowInfo.RowData;

                        int selectedIndex = CreateSalesInvoicesList.IndexOf(CreateSalesInvoicesDataSource);

                        CreateSalesInvoicesList[selectedIndex].SelectedLine = true;

                        await _CreateSalesInvoicesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);

                    }

                    break;

                case "multiselect":

                    if (args.RowInfo.RowData != null)
                    {

                        CreateSalesInvoicesDataSource = args.RowInfo.RowData;


                        foreach (var selectedRow in CreateSalesInvoicesList)
                        {
                            int selectedRowIndex = CreateSalesInvoicesList.IndexOf(selectedRow);
                            CreateSalesInvoicesList[selectedRowIndex].SelectedLine = true;

                        }

                        await _CreateSalesInvoicesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);

                    }
                    break;

                case "removeall":

                    if (args.RowInfo.RowData != null)
                    {

                        CreateSalesInvoicesDataSource = args.RowInfo.RowData;

                        foreach (var line in CreateSalesInvoicesList)
                        {
                            int lineIndex = CreateSalesInvoicesList.IndexOf(line);
                            CreateSalesInvoicesList[lineIndex].SelectedLine = false;
                        }

                        await _CreateSalesInvoicesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);

                    }
                    break;


            }
        }

        public async void CreateSalesInvoicesButtonClicked()
        {
            List<SelectSalesInvoiceLinesDto> SalesInvoiceLinesList = new List<SelectSalesInvoiceLinesDto>();

            List<SelectStockFicheLinesDto> StockFicheLineList = new List<SelectStockFicheLinesDto>();

            var now = GetSQLDateAppService.GetDateFromSQL();

            foreach (var item in CreateSalesInvoicesList)
            {

                if (item.SelectedLine)
                {
                    SelectSalesInvoiceLinesDto SalesInvoiceLineModel = new SelectSalesInvoiceLinesDto
                    {
                        LineAmount = DataSource.SelectSalesOrderLines.Where(t => t.Id == item.LineID).Select(t => t.LineTotalAmount).FirstOrDefault(),
                        LineNr = SalesInvoiceLinesList.Count + 1,
                        LineDescription = string.Empty,
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        SalesOrderLineID = item.LineID,
                        Quantity = item.Quantity,
                        UnitPrice = DataSource.SelectSalesOrderLines.Where(t => t.Id == item.LineID).Select(t => t.UnitPrice).FirstOrDefault(),
                        UnitSetCode = item.UnitSetCode,
                        UnitSetID = item.UnitSetID,
                        WorkOrderCreationDate = item.WorkOrderCreationDate,
                        WarehouseName = item.WarehouseName,
                        WarehouseID = item.WarehouseID,
                        WarehouseCode = item.WarehouseCode,
                        VATrate = item.VATrate,
                        VATamount = item.VATamount,
                        TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                        BranchCode = item.BranchCode,
                        BranchID = item.BranchID,
                        BranchName = item.BranchName,
                        CurrentAccountCardCode = item.CurrentAccountCardCode,
                        CurrentAccountCardID = item.CurrentAccountCardID,
                        CurrentAccountCardName = item.CurrentAccountCardName,
                        Date_ = item.Date_,
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        PaymentPlanName = item.PaymentPlanName,
                        PaymentPlanID = item.PaymentPlanID,
                        LineTotalAmount = item.LineTotalAmount,
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                        TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                        ExchangeRate = item.ExchangeRate,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
                        ProductGroupName = item.ProductGroupName,
                        ProductGroupID = item.ProductGroupID,
                        OrderAcceptanceRecordLineID = item.OrderAcceptanceRecordLineID,
                        OrderAcceptanceRecordID = item.OrderAcceptanceRecordID,
                        LinkedSalesPropositionID = item.LinkedSalesPropositionID,
                        LikedPropositionLineID = item.LikedPropositionLineID,
                    };
                    SalesInvoiceLinesList.Add(SalesInvoiceLineModel);

                }

            }
            if (CreateSalesInvoicesList.Count == 0)
            {
                await ModalManager.MessagePopupAsync(L["UIInformationStockFichesCreatedTitle"], L["UIInformationStockFichesCreatedMessage2"]);
            }
            else
            {
                SpinnerService.Show();
                await Task.Delay(100);

                CreateSalesInvoiceDto SalesInvoicesModel = new CreateSalesInvoiceDto
                {
                    BranchID = DataSource.BranchID,
                    CurrencyID = DataSource.CurrencyID,
                    Date_ = GetSQLDateAppService.GetDateFromSQL(),
                    Description_ = string.Empty,
                    ExchangeRate = DataSource.ExchangeRate,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesInvoicesChildMenu"),
                    NetAmount = DataSource.NetAmount,
                    CurrentAccountCardID = DataSource.CurrentAccountCardID,
                    GrossAmount = DataSource.GrossAmount,
                    PaymentPlanID = DataSource.PaymentPlanID,
                    SalesOrderID = DataSource.Id,
                    PricingCurrency = (int)DataSource.PricingCurrency,
                    ShippingAdressID = DataSource.ShippingAdressID.GetValueOrDefault(),
                    TotalDiscountAmount = DataSource.TotalDiscountAmount,
                    TransactionExchangeGrossAmount = DataSource.TransactionExchangeGrossAmount,
                    TransactionExchangeCurrencyID = DataSource.TransactionExchangeCurrencyID.GetValueOrDefault(),
                    TotalVatAmount = DataSource.TotalVatAmount,
                    TransactionExchangeTotalVatAmount = DataSource.TransactionExchangeTotalVatAmount,
                    TotalVatExcludedAmount = DataSource.TotalVatExcludedAmount,
                    TransactionExchangeNetAmount = DataSource.TransactionExchangeNetAmount,
                    WorkOrderCreationDate = DataSource.WorkOrderCreationDate,
                    TransactionExchangeTotalVatExcludedAmount = DataSource.TransactionExchangeTotalVatExcludedAmount,
                    TransactionExchangeTotalDiscountAmount = DataSource.TransactionExchangeTotalDiscountAmount,
                    SpecialCode = DataSource.SpecialCode,
                    WarehouseID = DataSource.WarehouseID,
                    Time_ = null,
                    ConfirmedLoadingDate = DataSource.ConfirmedLoadingDate,
                    CustomerOrderNr = DataSource.CustomerOrderNr,
                    CustomerRequestedDate = DataSource.CustomerRequestedDate,
                    isStandart = DataSource.isStandart,
                    OrderAcceptanceRecordID = DataSource.OrderAcceptanceRecordID.GetValueOrDefault(),
                    LinkedSalesPropositionID = DataSource.LinkedSalesPropositionID,
                };

                SalesInvoicesModel.SelectSalesInvoiceLines = SalesInvoiceLinesList;

                var invoiceResult = await SalesInvoicesAppService.CreateAsync(SalesInvoicesModel);

                SelectSalesInvoiceDto salesInvoice = (await SalesInvoicesAppService.GetAsync(invoiceResult.Data.Id)).Data;

                if (salesInvoice != null && salesInvoice.Id != Guid.Empty && salesInvoice.SelectSalesInvoiceLines.Count > 0)
                {
                    foreach (var invoiceLine in salesInvoice.SelectSalesInvoiceLines)
                    {
                        SelectStockFicheLinesDto StockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            InputOutputCode = 1,
                            LineAmount = invoiceLine.LineAmount,
                            LineDescription = string.Empty,
                            LineNr = StockFicheLineList.Count + 1,
                            MRPID = Guid.Empty,
                            MRPLineID = Guid.Empty,
                            FicheType = StockFicheTypeEnum.StokCikisFisi,
                            SalesOrderLineID = invoiceLine.SalesOrderLineID,
                            PartyNo = string.Empty,
                            ProductCode = invoiceLine.ProductCode,
                            ProductID = invoiceLine.ProductID,
                            ProductionDateReferenceID = Guid.Empty,
                            ProductionOrderFicheNo = string.Empty,
                            ProductionOrderID = Guid.Empty,
                            ProductName = invoiceLine.ProductName,
                            SalesOrderID = DataSource.Id,
                            PurchaseInvoiceID = Guid.Empty,
                            PurchaseInvoiceLineID = Guid.Empty,
                            PurchaseOrderFicheNo = string.Empty,
                            PurchaseOrderID = Guid.Empty,
                            PurchaseOrderLineID = Guid.Empty,
                            Quantity = invoiceLine.Quantity,
                            SalesInvoiceID = invoiceLine.SalesInvoiceID,
                            SalesInvoiceLineID = invoiceLine.Id,
                            Date_ = now.Date,
                            TransactionExchangeLineAmount = invoiceLine.TransactionExchangeLineAmount,
                            TransactionExchangeUnitPrice = invoiceLine.TransactionExchangeUnitPrice,
                            UnitOutputCost = 0,
                            UnitPrice = invoiceLine.UnitPrice,
                            UnitSetCode = invoiceLine.UnitSetCode,
                            UnitSetID = invoiceLine.UnitSetID,
                        };

                        StockFicheLineList.Add(StockFicheLineModel);
                    }

                    CreateStockFichesDto stockFicheModel = new CreateStockFichesDto
                    {
                        BranchID = DataSource.BranchID,
                        CurrencyID = DataSource.CurrencyID,
                        SalesOrderID = DataSource.Id,
                        Date_ = now.Date,
                        Description_ = string.Empty,
                        ExchangeRate = DataSource.ExchangeRate,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                        FicheType = 51,
                        InputOutputCode = 1,
                        NetAmount = DataSource.NetAmount,
                        ProductionDateReferenceID = Guid.Empty,
                        ProductionOrderID = Guid.Empty,
                        PurchaseInvoiceID = Guid.Empty,
                        PurchaseOrderID = Guid.Empty,
                        SalesInvoiceID = salesInvoice.Id,
                        PurchaseRequestID = Guid.Empty,
                        SpecialCode = DataSource.SpecialCode,
                        Time_ = now.TimeOfDay,
                        TransactionExchangeCurrencyID = DataSource.TransactionExchangeCurrencyID,
                        WarehouseID = DataSource.WarehouseID,
                    };

                    stockFicheModel.SelectStockFicheLines = StockFicheLineList;

                    await StockFichesAppService.CreateAsync(stockFicheModel);
                }

                SpinnerService.Hide();
                await ModalManager.MessagePopupAsync(L["UIInformationStockFichesCreatedTitle"], L["UIInformationStockFichesCreatedMessage"]);
            }

            HideCreateSalesInvoicesPopup();

            await InvokeAsync(StateHasChanged);
        }

        public void HideCreateSalesInvoicesPopup()
        {

            CreateSalesInvoicesList.Clear();
            CreateSalesInvoicesCrudPopup = false;

        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }
        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        private async Task GetBranchesList()
        {
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        private async Task GetWarehousesList()
        {
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetPaymentPlansList()
        {
            PaymentPlansList = (await PaymentPlansAppService.GetListAsync(new ListPaymentPlansParameterDto())).Data.ToList();
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesOrdersChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Fiyatlandırma Dövizi Enum Combobox

        public IEnumerable<SelectSalesOrderDto> PricingCurrencyList = GetEnumPricingCurrencyNames<PricingCurrencyEnum>();

        public static List<SelectSalesOrderDto> GetEnumPricingCurrencyNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PricingCurrencyEnum>()
                       .Select(x => new SelectSalesOrderDto
                       {
                           PricingCurrency = x,
                           PricingCurrencyName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private async void PricingCurrencyValueChangeHandler(ChangeEventArgs<PricingCurrencyEnum, SelectSalesOrderDto> args)
        {
            if (args != null)
            {
                if (args.Value == PricingCurrencyEnum.LocalCurrency)
                {
                    UnitPriceEnabled = true;
                    DiscountAmountEnabled = true;
                    LineAmountEnabled = true;
                    LineTotalAmountEnabled = true;

                    TransactionExchangeUnitPriceEnabled = false;
                    TransactionExchangeDiscountAmountEnabled = false;
                    TransactionExchangeLineAmountEnabled = false;
                    TransactionExchangeLineTotalAmountEnabled = false;
                }
                else if (args.Value == PricingCurrencyEnum.TransactionCurrency)
                {
                    UnitPriceEnabled = false;
                    DiscountAmountEnabled = false;
                    LineAmountEnabled = false;
                    LineTotalAmountEnabled = false;

                    TransactionExchangeUnitPriceEnabled = true;
                    TransactionExchangeDiscountAmountEnabled = true;
                    TransactionExchangeLineAmountEnabled = true;
                    TransactionExchangeLineTotalAmountEnabled = true;
                }
                else
                {
                    UnitPriceEnabled = false;
                    DiscountAmountEnabled = false;
                    LineAmountEnabled = false;
                    LineTotalAmountEnabled = false;

                    TransactionExchangeUnitPriceEnabled = false;
                    TransactionExchangeDiscountAmountEnabled = false;
                    TransactionExchangeLineAmountEnabled = false;
                    TransactionExchangeLineTotalAmountEnabled = false;
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        private bool UnitPriceEnabled = false;
        private bool DiscountAmountEnabled = false;
        private bool LineAmountEnabled = false;
        private bool LineTotalAmountEnabled = false;

        private bool TransactionExchangeUnitPriceEnabled = false;
        private bool TransactionExchangeDiscountAmountEnabled = false;
        private bool TransactionExchangeLineAmountEnabled = false;
        private bool TransactionExchangeLineTotalAmountEnabled = false;

        #endregion

        public async void DateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            decimal exRate = 1;

            var exchangeRateType = LoginedUserService.PurchaseRequestExchangeRateType;

            var date = new DateTime(args.Value.Year, args.Value.Month, args.Value.Day);

            var exchangeRate = (await ExchangeRatesService.GetListAsync(new ListExchangeRatesParameterDto())).Data.Where(t => t.Date == date && t.CurrencyId == DataSource.TransactionExchangeCurrencyID).FirstOrDefault();

            if (exchangeRate != null)
            {
                switch (exchangeRateType)
                {
                    case 0:
                        exRate = exchangeRate.BuyingRate;
                        break;
                    case 1:
                        exRate = exchangeRate.SaleRate;
                        break;
                    case 2:
                        exRate = exchangeRate.EffectiveBuyingRate;
                        break;
                    case 3:
                        exRate = exchangeRate.EffectiveSaleRate;
                        break;
                    default:
                        exRate = 1;
                        break;
                }
            }

            DataSource.Date_ = args.Value;
            DataSource.ExchangeRate = exRate;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
