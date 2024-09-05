using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.SplitButtons;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Entities.GeneralSystemIdentifications.SalesManagementParameter.Services;
using TsiErp.Business.Entities.SalesPrice.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;


namespace TsiErp.ErpUI.Pages.SalesManagement.SalesOrder
{
    public partial class SalesOrdersListPage : IDisposable
    {
        private SfGrid<SelectSalesOrderLinesDto> _LineGrid;
        private SfGrid<SelectSalesOrderLinesDto> _ProductionOrderGrid;
        private SfGrid<ProductsTreeDto> _BoMLineGrid;

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

        //SelectBillsofMaterialsDto BoMDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductionOrderGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> BoMLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectSalesOrderLinesDto> GridLineList = new List<SelectSalesOrderLinesDto>();

        List<SelectSalesOrderLinesDto> GridProductionOrderList = new List<SelectSalesOrderLinesDto>();

        List<SelectBillsofMaterialLinesDto> GridBoMLineList = new List<SelectBillsofMaterialLinesDto>();

        List<ListBillsofMaterialsDto> BoMList = new List<ListBillsofMaterialsDto>();

        private bool LineCrudPopup = false;

        private bool CreateProductionOrderCrudPopup = false;

        private bool BoMLineCrudPopup = false;

        DateTime MaxDate;

        SfProgressButton ProgressBtn;
        bool HideCreateProductionOrderPopupButtonDisabled = false;
        bool LoadingModalVisibility = false;


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
            CreateProductionOrderContextMenuItems();

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

        protected void CreateProductionOrderContextMenuItems()
        {
            //if (ProductionOrderGridContextMenu.Count() == 0)
            //{

            //    var contextID = contextsList.Where(t => t.MenuName == "ProductOrderContextTree").Select(t => t.Id).FirstOrDefault();
            //    var permission = UserPermissionsList.Where(t => t.MenuId == contextID).Select(t => t.IsUserPermitted).FirstOrDefault();

            //    if (permission)
            //    {
            //        ProductionOrderGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductOrderContextTree"], Id = "productstree" });
            //    }


            //}
        }

        public async void OnCreateProductionOrderContextMenuClick(ContextMenuClickEventArgs<SelectSalesOrderLinesDto> args)
        {
            //switch (args.Item.Id)
            //{
            //    case "productstree":

            //        BoMList = (await BillsofMaterialsAppService.GetListAsync(new ListBillsofMaterialsParameterDto())).Data.Where(t => t.FinishedProductCode == args.RowInfo.RowData.ProductCode).ToList();

            //        Guid BoMID = BoMList.Select(t => t.Id).FirstOrDefault();

            //        BoMDataSource = (await BillsofMaterialsAppService.GetAsync(BoMID)).Data;

            //        GridBoMLineList = BoMDataSource.SelectBillsofMaterialLines;



            //        foreach (var item in GridBoMLineList)
            //        {
            //            item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
            //            item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;

            //            decimal stockAmount = (await ProductsAppService.GetStockAmountAsync(item.ProductID.GetValueOrDefault())).Data.Amount;

            //            ProductsTreeDto _model = new ProductsTreeDto
            //            {
            //                ProductName = item.ProductName,
            //                ProductCode = item.ProductCode,
            //                AmountofStock = stockAmount,
            //                AmountofRequierement = Math.Abs(stockAmount - item.Quantity),
            //                SupplyForm = 1 //deneme
            //            };

            //            ProductTreeDataSource.Add(_model);
            //        }

            //        BoMLineCrudPopup = true;

            //        await InvokeAsync(StateHasChanged);

            //        break;

            //    default:
            //        break;
            //}

            await Task.CompletedTask;

        }


        protected async Task OnCreateProductionOrderBtnClicked()
        {
            //HideCreateProductionOrderPopupButtonDisabled = true;
            //await ProgressBtn.StartAsync();

            SpinnerService.Show();
            await Task.Delay(100);

            foreach (var productionOrder in GridProductionOrderList)
            {
                var productProductionRoute = (await RoutesAppService.GetListAsync(new Entities.Entities.ProductionManagement.Route.Dtos.ListRoutesParameterDto())).Data.Where(t => t.ProductID == productionOrder.ProductID && t.TechnicalApproval == true && t.Approval == true).FirstOrDefault();


                var bomDataSource = (await BillsofMaterialsAppService.GetbyCurrentAccountIDAsync(DataSource.CurrentAccountCardID, productionOrder.ProductID.GetValueOrDefault())).Data;

                var finishedProduct = (await ProductsAppService.GetAsync(productionOrder.ProductID.GetValueOrDefault())).Data;

                var bomLineList = bomDataSource.SelectBillsofMaterialLines;
                    CreateProductionOrdersDto producionOrder = new CreateProductionOrdersDto
                    {
                        OrderID = DataSource.Id,
                        FinishedProductID = productionOrder.ProductID.GetValueOrDefault(),
                        LinkedProductID = Guid.Empty,
                        PlannedQuantity = productionOrder.Quantity,
                        ProducedQuantity = 0,
                        CurrentAccountID = DataSource.CurrentAccountCardID,
                        BOMID = bomDataSource.Id,
                        ConfirmedLoadingDate = DataSource.ConfirmedLoadingDate,
                        Cancel_ = false,
                        UnitSetID = finishedProduct.UnitSetID,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ProductionOrdersChildMenu"),
                        CustomerOrderNo = "",
                        LinkedProductionOrderID = Guid.Empty,
                        OrderLineID = productionOrder.Id,
                        ProductionOrderState = (int)Entities.Enums.ProductionOrderStateEnum.Baslamadi,
                        ProductTreeID = Guid.Empty,
                        ProductTreeLineID = Guid.Empty,
                        PropositionID = productionOrder.LinkedSalesPropositionID.GetValueOrDefault(),
                        PropositionLineID = productionOrder.LikedPropositionLineID.GetValueOrDefault(),
                        Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                        Description_ = "",
                        CreationTime = GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        IsDeleted = false,
                        RouteID = productProductionRoute.Id,
                        BranchID = DataSource.BranchID,
                        WarehouseID = DataSource.WarehouseID
                    };

                    var insertedProductionOrder = (await ProductionOrdersAppService.ConverttoProductionOrder(producionOrder)).Data;
                
            }

            //HideCreateProductionOrderPopupButtonDisabled = false;
            //await ProgressBtn.EndProgressAsync();
            SpinnerService.Hide(); 
            await ModalManager.MessagePopupAsync(L["UIMessageConvertTitle"], L["UIMessageConvertMessage"]);

        }


        public void HideCreateProductionOrderPopup()
        {
            CreateProductionOrderCrudPopup = false;
        }

        #endregion

        #region Teklif Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            var salesManagementParameter = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data;

            DataSource = new SelectSalesOrderDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("SalesOrdersChildMenu"),
                CustomerRequestedDate = GetSQLDateAppService.GetDateFromSQL(),
                BranchID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchID : Guid.Empty,
                WarehouseID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseID : Guid.Empty,
                BranchCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchCode : string.Empty,
                BranchName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchName : string.Empty,
                WarehouseCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseCode : string.Empty,
                WarehouseName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultWarehouseName : string.Empty,

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
                    IsChanged = true;
                    DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectSalesOrderLines;


                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "createproductionorder":

                    DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridProductionOrderList = DataSource.SelectSalesOrderLines;

                        foreach (var item in GridProductionOrderList)
                        {
                            item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                            item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                            item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                        }


                        CreateProductionOrderCrudPopup = true;


                    await InvokeAsync(StateHasChanged);
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
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
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

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

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
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
