using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.SalesInvoices
{
    public partial class SalesInvoicesListPage
    {
        private SfGrid<SelectSalesInvoiceLinesDto> _LineGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }



        SelectSalesInvoiceLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectSalesInvoiceLinesDto> GridLineList = new List<SelectSalesInvoiceLinesDto>();

        private bool LineCrudPopup = false;

        DateTime MaxDate;

        #region ButtonEdit İşlemleri


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

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = SalesInvoicesAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "SalesInvoicesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            await GetProductsList();

            futureDateParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.FutureDateParameter;

            MaxDate = !futureDateParameter ? GetSQLDateAppService.GetDateFromSQL() : new DateTime(9999, 12, 31);
        }



        #region Fatura Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            var salesManagementParameter = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data;

            DataSource = new SelectSalesInvoiceDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesInvoicesChildMenu"),
                CustomerRequestedDate = GetSQLDateAppService.GetDateFromSQL().Date,
                BranchID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchID : Guid.Empty,
                WarehouseID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseID : Guid.Empty,
                BranchCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchCode : string.Empty,
                BranchName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchName : string.Empty,
                WarehouseCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseCode : string.Empty,
                WarehouseName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseName : string.Empty,

            };

            DataSource.SelectSalesInvoiceLines = new List<SelectSalesInvoiceLinesDto>();
            GridLineList = DataSource.SelectSalesInvoiceLines;


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
                            case "SalesInvoiceLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceLineContextAdd"], Id = "new" }); break;
                            case "SalesInvoiceLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceLineContextChange"], Id = "changed" }); break;
                            case "SalesInvoiceLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceLineContextDelete"], Id = "delete" }); break;
                            case "SalesInvoiceLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceLineContextRefresh"], Id = "refresh" }); break;
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
                            case "SalesInvoiceContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceContextAdd"], Id = "new" }); break;
                            case "SalesInvoiceContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceContextChange"], Id = "changed" }); break;
                            case "SalesInvoiceContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceContextDelete"], Id = "delete" }); break;
                            case "SalesInvoiceContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["SalesInvoiceContextRefresh"], Id = "refresh" }); break;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListSalesInvoiceDto> args)
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
                        DataSource = (await SalesInvoicesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        GridLineList = DataSource.SelectSalesInvoiceLines;


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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectSalesInvoiceLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (DataSource.PricingCurrency == PricingCurrencyEnum.LocalCurrency || DataSource.PricingCurrency == PricingCurrencyEnum.TransactionCurrency)
                    {
                        LineDataSource = new SelectSalesInvoiceLinesDto();
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
                                DataSource.SelectSalesInvoiceLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    DataSource.SelectSalesInvoiceLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    DataSource.SelectSalesInvoiceLines.Remove(line);
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
                    if (DataSource.SelectSalesInvoiceLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectSalesInvoiceLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectSalesInvoiceLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectSalesInvoiceLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectSalesInvoiceLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectSalesInvoiceLines[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectSalesInvoiceLines;
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

        protected override async Task OnSubmit()
        {
            var now = GetSQLDateAppService.GetDateFromSQL();

            List<SelectStockFicheLinesDto> StockFicheLineList = new List<SelectStockFicheLinesDto>();

            #region Sales Invoices OnSubmit
            SelectSalesInvoiceDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectSalesInvoiceDto, CreateSalesInvoiceDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectSalesInvoiceDto, UpdateSalesInvoiceDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
            #endregion

            #region Stok Çıkış Fiş Create

            DataSource = (await SalesInvoicesAppService.GetAsync(DataSource.Id)).Data;

            SelectStockFichesDto stockFicheCreated = (await StockFichesAppService.GetbySalesInvoiceAsync(DataSource.Id)).Data;

            if (stockFicheCreated == null || stockFicheCreated.Id == Guid.Empty)
            {
                if (DataSource.SelectSalesInvoiceLines.Count > 0)
                {
                    foreach (var line in DataSource.SelectSalesInvoiceLines)
                    {
                        SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            Date_ = now.Date,
                            FicheType = StockFicheTypeEnum.StokCikisFisi,
                            InputOutputCode = 1,
                            LineAmount = line.LineAmount,
                            LineDescription = string.Empty,
                            LineNr = StockFicheLineList.Count + 1,
                            MRPID = Guid.Empty,
                            MRPLineID = Guid.Empty,
                            PartyNo = string.Empty,
                            ProductCode = line.ProductCode,
                            ProductID = line.ProductID,
                            ProductionDateReferance = string.Empty,
                            ProductionOrderFicheNo = string.Empty,
                            ProductionOrderID = Guid.Empty,
                            ProductName = line.ProductName,
                            PurchaseInvoiceID = Guid.Empty,
                            PurchaseInvoiceLineID = Guid.Empty,
                            PurchaseOrderID = Guid.Empty,
                            PurchaseOrderLineID = Guid.Empty,
                            SalesInvoiceID = DataSource.Id,
                            SalesOrderID = DataSource.SalesOrderID,
                            SalesOrderLineID = line.SalesOrderLineID,
                            Quantity = line.Quantity,
                            SalesInvoiceLineID = line.Id,
                            TransactionExchangeLineAmount = line.TransactionExchangeLineAmount,
                            TransactionExchangeUnitPrice = line.TransactionExchangeUnitPrice,
                            UnitOutputCost = 0,
                            UnitPrice = line.UnitPrice,
                            UnitSetID = line.UnitSetID,
                            UnitSetCode = line.UnitSetCode,

                        };

                        StockFicheLineList.Add(stockFicheLineModel);
                    }
                }

                CreateStockFichesDto createStockFicheModel = new CreateStockFichesDto
                {
                    BranchID = DataSource.BranchID,
                    CurrencyID = DataSource.CurrencyID,
                    SalesOrderID = DataSource.SalesOrderID,
                    Date_ = now.Date,
                    Description_ = string.Empty,
                    ExchangeRate = DataSource.ExchangeRate,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                    InputOutputCode = 1,
                    NetAmount = DataSource.NetAmount,
                    ProductionDateReferance = string.Empty,
                    FicheType = 51,
                    ProductionOrderID = Guid.Empty,
                    PurchaseInvoiceID = Guid.Empty,
                    SalesInvoiceID = DataSource.Id,
                    PurchaseOrderID = Guid.Empty,
                    PurchaseRequestID = Guid.Empty,
                    Time_ = now.TimeOfDay,
                    WarehouseID = DataSource.WarehouseID,
                    TransactionExchangeCurrencyID = DataSource.TransactionExchangeCurrencyID,
                    SpecialCode = DataSource.SpecialCode,
                };

                createStockFicheModel.SelectStockFicheLines = StockFicheLineList;

                await StockFichesAppService.CreateAsync(createStockFicheModel);
            }
            else
            {
                if (DataSource.SelectSalesInvoiceLines.Count > 0)
                {
                    foreach (var line in DataSource.SelectSalesInvoiceLines)
                    {
                        if (line.Id != Guid.Empty)
                        {
                            var stockLine = stockFicheCreated.SelectStockFicheLines.Where(t => t.SalesInvoiceLineID == line.Id).FirstOrDefault();

                            if (stockLine != null) // Line Update
                            {
                                SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                                {
                                    Date_ = stockLine.Date_,
                                    FicheType = StockFicheTypeEnum.StokCikisFisi,
                                    InputOutputCode = 1,
                                    LineAmount = line.LineAmount,
                                    LineDescription = stockLine.LineDescription,
                                    LineNr = stockLine.LineNr,
                                    MRPID = Guid.Empty,
                                    MRPLineID = Guid.Empty,
                                    PartyNo = string.Empty,
                                    ProductCode = line.ProductCode,
                                    ProductID = line.ProductID,
                                    ProductionDateReferance = string.Empty,
                                    ProductionOrderFicheNo = string.Empty,
                                    ProductionOrderID = Guid.Empty,
                                    ProductName = line.ProductName,
                                    PurchaseInvoiceID = Guid.Empty,
                                    PurchaseInvoiceLineID = Guid.Empty,
                                    PurchaseOrderID = Guid.Empty,
                                    PurchaseOrderLineID = Guid.Empty,
                                    SalesInvoiceID = DataSource.Id,
                                    SalesOrderID = DataSource.SalesOrderID,
                                    SalesOrderLineID = line.SalesOrderLineID,
                                    Quantity = line.Quantity,
                                    SalesInvoiceLineID = line.Id,
                                    TransactionExchangeLineAmount = line.TransactionExchangeLineAmount,
                                    TransactionExchangeUnitPrice = line.TransactionExchangeUnitPrice,
                                    UnitOutputCost = 0,
                                    UnitPrice = line.UnitPrice,
                                    UnitSetID = line.UnitSetID,
                                    UnitSetCode = line.UnitSetCode,
                                    Id = stockLine.Id,
                                    CreationTime = stockLine.CreationTime,
                                    CreatorId = stockLine.CreatorId,
                                    DataOpenStatus = stockLine.DataOpenStatus,
                                    DataOpenStatusUserId = stockLine.DataOpenStatusUserId,
                                    DeleterId = stockLine.DeleterId,
                                    DeletionTime = stockLine.DeletionTime,
                                    IsDeleted = stockLine.IsDeleted,
                                    StockFicheID = stockLine.StockFicheID,
                                    LastModificationTime = now,
                                    LastModifierId = LoginedUserService.UserId,
                                    PurchaseOrderFicheNo = stockLine.PurchaseOrderFicheNo
                                };

                                int stockLineIndex = stockFicheCreated.SelectStockFicheLines.IndexOf(stockLine);

                                stockFicheCreated.SelectStockFicheLines[stockLineIndex] = stockFicheLineModel;
                            }
                        }

                        else
                        {
                            SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                            {
                                Date_ = now.Date,
                                FicheType = StockFicheTypeEnum.StokCikisFisi,
                                InputOutputCode = 1,
                                LineAmount = line.LineAmount,
                                LineDescription = string.Empty,
                                LineNr = stockFicheCreated.SelectStockFicheLines.Count + 1,
                                MRPID = Guid.Empty,
                                MRPLineID = Guid.Empty,
                                PartyNo = string.Empty,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductionDateReferance = string.Empty,
                                ProductionOrderFicheNo = string.Empty,
                                ProductionOrderID = Guid.Empty,
                                ProductName = line.ProductName,
                                PurchaseInvoiceID = Guid.Empty,
                                PurchaseInvoiceLineID = Guid.Empty,
                                PurchaseOrderID = Guid.Empty,
                                PurchaseOrderLineID = Guid.Empty,
                                SalesInvoiceID = DataSource.Id,
                                SalesOrderID = DataSource.SalesOrderID,
                                SalesOrderLineID = line.SalesOrderLineID,
                                Quantity = line.Quantity,
                                SalesInvoiceLineID = line.Id,
                                TransactionExchangeLineAmount = line.TransactionExchangeLineAmount,
                                TransactionExchangeUnitPrice = line.TransactionExchangeUnitPrice,
                                UnitOutputCost = 0,
                                UnitPrice = line.UnitPrice,
                                UnitSetID = line.UnitSetID,
                                UnitSetCode = line.UnitSetCode,

                            };

                            stockFicheCreated.SelectStockFicheLines.Add(stockFicheLineModel);
                        }
                    }
                }

                stockFicheCreated.BranchID = DataSource.BranchID;
                stockFicheCreated.CurrencyID = DataSource.CurrencyID;
                stockFicheCreated.ExchangeRate = DataSource.ExchangeRate;
                stockFicheCreated.NetAmount = DataSource.NetAmount;
                stockFicheCreated.WarehouseID = DataSource.WarehouseID;
                stockFicheCreated.TransactionExchangeCurrencyID = DataSource.TransactionExchangeCurrencyID;
                stockFicheCreated.SpecialCode = DataSource.SpecialCode;

                UpdateStockFichesDto updatedStockFicheEntity = ObjectMapper.Map<SelectStockFichesDto, UpdateStockFichesDto>(stockFicheCreated);

                await StockFichesAppService.UpdateAsync(updatedStockFicheEntity);


            }




            #endregion
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesInvoicesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Fiyatlandırma Dövizi Enum Combobox

        public IEnumerable<SelectSalesInvoiceDto> PricingCurrencyList = GetEnumPricingCurrencyNames<PricingCurrencyEnum>();

        public static List<SelectSalesInvoiceDto> GetEnumPricingCurrencyNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PricingCurrencyEnum>()
                       .Select(x => new SelectSalesInvoiceDto
                       {
                           PricingCurrency = x,
                           PricingCurrencyName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private async void PricingCurrencyValueChangeHandler(ChangeEventArgs<PricingCurrencyEnum, SelectSalesInvoiceDto> args)
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
