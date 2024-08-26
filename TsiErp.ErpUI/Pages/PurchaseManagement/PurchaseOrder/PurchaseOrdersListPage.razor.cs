using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
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
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PurchaseManagement.PurchaseOrder
{
    public partial class PurchaseOrdersListPage : IDisposable
    {
        private SfGrid<SelectPurchaseOrderLinesDto> _LineGrid;
        private SfGrid<CreateStockReceiptFishes> _CreateStockFishesGrid;
        private SfGrid<CreateStockReceiptFishes> _CancelOrderGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPurchaseOrderLinesDto LineDataSource;
        CreateStockReceiptFishes CreateStockReceiptFishesDataSource;
        CreateStockReceiptFishes CancelOrderDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> CreateStockFishesGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseOrderLinesDto> GridLineList = new List<SelectPurchaseOrderLinesDto>();

        List<CreateStockReceiptFishes> CreateStockFishesList = new List<CreateStockReceiptFishes>();

        List<CreateStockReceiptFishes> CancelOrderList = new List<CreateStockReceiptFishes>();

        private bool LineCrudPopup = false;
        private bool CreateStockFishesCrudPopup = false;
        private bool CancelOrderCrudPopup = false;

        DateTime MaxDate;

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

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
                DataSource.BranchName = string.Empty;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.BranchID = selectedUnitSet.Id;
                DataSource.BranchCode = selectedUnitSet.Code;
                DataSource.BranchName = selectedUnitSet.Name;
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

        SfTextBox CurrentAccountCardsCodeButtonEdit;
        SfTextBox CurrentAccountCardsNameButtonEdit;
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
                DataSource.TransactionExchangeCurrencyID = selectedUnitSet.CurrencyID;
                DataSource.TransactionExchangeCurrencyCode = selectedUnitSet.Currency;
                SelectCurrentAccountCardsPopupVisible = false;
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
                LineDataSource.SupplierReferenceNo = string.Empty;
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
                LineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                LineDataSource.UnitSetID = selectedProduct.UnitSetID;
                LineDataSource.VATrate = selectedProduct.SaleVAT;

                if (DataSource.CurrentAccountCardID != Guid.Empty && DataSource.CurrentAccountCardID != null)
                {
                    //    var lastApprovedPriceID = (await PurchasePricesAppService.GetListAsync(new ListPurchasePricesParameterDto())).Data.Where(t => t.IsApproved == true && t.CurrentAccountCardID == DataSource.CurrentAccountCardID).Last().Id;

                    //    if(lastApprovedPriceID != Guid.Empty)
                    //    {
                    //        LineDataSource.UnitPrice = (await PurchasePricesAppService.GetAsync(lastApprovedPriceID)).Data.SelectPurchasePriceLines.Where(t => t.ProductID == selectedProduct.Id).Select(t => t.Price).FirstOrDefault();

                    //    }

                    string supplierReferanceNumber = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(selectedProduct.Id, DataSource.CurrentAccountCardID.GetValueOrDefault());

                    if (!string.IsNullOrEmpty(supplierReferanceNumber))
                    {
                        LineDataSource.SupplierReferenceNo = supplierReferanceNumber;
                    }
                    else
                    {
                        LineDataSource.SupplierReferenceNo = "-";
                    }
                }
                SelectProductsPopupVisible = false;

                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Üretim Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersPopupVisible = true;
            await GetProductionOrdersList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                DataSource.ProductionOrderID = selectedProductionOrder.Id;
                DataSource.ProductionOrderFicheNo = selectedProductionOrder.FicheNo;
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Üretim Emri ButtonEdit - Satır

        SfTextBox LineProductionOrdersButtonEdit;
        bool SelectProductionOrdersLinePopupVisible = false;
        List<ListProductionOrdersDto> LineProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task LineProductionOrdersOnCreateIcon()
        {
            var LineProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineProductionOrdersButtonClickEvent);
            await LineProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineProductionOrdersButtonClick } });
        }

        public async void LineProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersLinePopupVisible = true;
            await GetLineProductionOrdersList();
            await InvokeAsync(StateHasChanged);
        }

        public void LineProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.ProductionOrderID = Guid.Empty;
                LineDataSource.ProductionOrderFicheNo = string.Empty;
            }
        }

        public async void LineProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedLineProductionOrder = args.RowData;

            if (selectedLineProductionOrder != null)
            {
                LineDataSource.ProductionOrderID = selectedLineProductionOrder.Id;
                LineDataSource.ProductionOrderFicheNo = selectedLineProductionOrder.FicheNo;
                SelectProductionOrdersLinePopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Sipariş İptal Modalı İşlemleri

        public async void OnCancelOrderContextMenuClick(ContextMenuClickEventArgs<CreateStockReceiptFishes> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    CancelOrderDataSource = args.RowInfo.RowData;

                    if (CancelOrderDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                    {
                        int selectedIndex = CancelOrderList.IndexOf(CancelOrderDataSource);

                        CancelOrderList[selectedIndex].SelectedLine = true;

                        await _CancelOrderGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "multiselect":

                    CancelOrderDataSource = args.RowInfo.RowData;
                    if (CancelOrderDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                    {
                        if (_CancelOrderGrid.SelectedRecords.Count > 0)
                        {
                            foreach (var selectedRow in _CancelOrderGrid.SelectedRecords)
                            {
                                if (selectedRow.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                                {

                                    int selectedRowIndex = CancelOrderList.IndexOf(selectedRow);
                                    CancelOrderList[selectedRowIndex].SelectedLine = true;
                                }
                            }
                        }


                        await _CancelOrderGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "removeall":

                    CancelOrderDataSource = args.RowInfo.RowData;
                    if (CancelOrderDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                    {
                        foreach (var line in CancelOrderList)
                        {
                            if (line.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                            {
                                int lineIndex = CancelOrderList.IndexOf(line);
                                CancelOrderList[lineIndex].SelectedLine = false;
                            }
                        }



                        await _CancelOrderGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;


            }
        }

        public async void CancelOrderButtonClicked()
        {
            var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationCancelOrderMessage"]);

            if (res == true)
            {
                if (DataSource.PurchaseOrderState != Entities.Enums.PurchaseOrderStateEnum.Iptal)
                {
                    if (DataSource.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Tamamlandi || DataSource.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.KismiTamamlandi)
                    {
                        var stockFicheIDs = (await StockFichesAppService.GetListbyPurchaseOrderAsync(DataSource.Id)).Data.Select(t=>t.Id).ToList();

                        foreach (var stockFicheID in stockFicheIDs)
                        {
                            await StockFichesAppService.DeleteAsync(stockFicheID);
                        }
                    }

                    foreach (var line in DataSource.SelectPurchaseOrderLinesDto)
                    {
                        int lineIndex = DataSource.SelectPurchaseOrderLinesDto.IndexOf(line);
                        DataSource.SelectPurchaseOrderLinesDto[lineIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Iptal;
                        DataSource.SelectPurchaseOrderLinesDto[lineIndex].PurchaseOrderLineWayBillStatusEnum =  PurchaseOrderLineWayBillStatusEnum.Beklemede;
                    }

                    DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Iptal;
                    DataSource.PurchaseOrderWayBillStatusEnum =  PurchaseOrderWayBillStatusEnum.Beklemede;
                    DataSource.PriceApprovalState =  PurchaseOrderPriceApprovalStateEnum.Beklemede;
                    var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);
                    await PurchaseOrdersAppService.UpdateCancelOrderAsync(updateInput);

                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                }
            }

            await GetListDataSourceAsync();
            HideCancelOrderPopup();
            await _grid.Refresh();
            await InvokeAsync(StateHasChanged);
        }

        public async void HideCancelOrderPopup()
        {

            CancelOrderList.Clear();
            CancelOrderCrudPopup = false;
            await InvokeAsync(StateHasChanged);

        }

        #endregion

        #region Stok Giriş Fişi Oluşturma Modalı İşlemleri

        public class CreateStockReceiptFishes
        {
            public Entities.Enums.PurchaseOrderLineStateEnum PurchaseStateLine { get; set; }
            public bool SelectedLine { get; set; }
            public Guid? ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
            public Guid? UnitSetID { get; set; }
            public string UnitSetCode { get; set; }
            public Guid? LineID { get; set; }
        }

        protected void CreateStockReceiptFishesContextMenuItems()
        {
            if (CreateStockFishesGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "StockReceiptFichesContextSelect":
                                CreateStockFishesGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockReceiptFichesContextSelect"], Id = "select" }); break;
                            case "StockReceiptFichesContextMultiSelect":
                                CreateStockFishesGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockReceiptFichesContextMultiSelect"], Id = "multiselect" }); break;
                            case "StockReceiptFichesContextRemoveAll":
                                CreateStockFishesGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockReceiptFichesContextRemoveAll"], Id = "removeall" }); break;
                            default: break;
                        }
                    }
                }
            }
        }


        public async void OnCreateStockReceiptFishesContextMenuClick(ContextMenuClickEventArgs<CreateStockReceiptFishes> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    CreateStockReceiptFishesDataSource = args.RowInfo.RowData;

                    if (CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                    {
                        int selectedIndex = CreateStockFishesList.IndexOf(CreateStockReceiptFishesDataSource);

                        CreateStockFishesList[selectedIndex].SelectedLine = true;

                        await _CreateStockFishesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "multiselect":

                    CreateStockReceiptFishesDataSource = args.RowInfo.RowData;
                    if (CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                    {
                        if (_CreateStockFishesGrid.SelectedRecords.Count > 0)
                        {
                            foreach (var selectedRow in _CreateStockFishesGrid.SelectedRecords)
                            {
                                if (selectedRow.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || selectedRow.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                                {

                                    int selectedRowIndex = CreateStockFishesList.IndexOf(selectedRow);
                                    CreateStockFishesList[selectedRowIndex].SelectedLine = true;
                                }
                            }
                        }


                        await _CreateStockFishesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "removeall":

                    CreateStockReceiptFishesDataSource = args.RowInfo.RowData;
                    if (CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                    {
                        foreach (var line in CreateStockFishesList)
                        {
                            if (line.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || line.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                            {
                                int lineIndex = CreateStockFishesList.IndexOf(line);
                                CreateStockFishesList[lineIndex].SelectedLine = false;
                            }
                        }



                        await _CreateStockFishesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;


            }
        }

        public async void CreateStockFishesButtonClicked()
        {
            List<SelectStockFicheLinesDto> stockFicheLineList = new List<SelectStockFicheLinesDto>();

            foreach (var item in CreateStockFishesList)
            {
                if (item.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || item.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                {
                    if (item.SelectedLine)
                    {
                        SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            FicheType = Entities.Enums.StockFicheTypeEnum.StokGirisFisi,
                            LineAmount = DataSource.SelectPurchaseOrderLinesDto.Where(t => t.Id == item.LineID).Select(t => t.LineTotalAmount).FirstOrDefault(),
                            LineNr = stockFicheLineList.Count + 1,
                            LineDescription = string.Empty,
                            ProductID = item.ProductID,
                            ProductCode = item.ProductCode,
                            ProductName = item.ProductName,
                            PurchaseOrderID = DataSource.Id,
                            PurchaseOrderFicheNo = DataSource.FicheNo,
                            PurchaseOrderLineID = item.LineID,
                            Quantity = item.Quantity,
                            StockFicheID = Guid.Empty,
                            UnitPrice = DataSource.SelectPurchaseOrderLinesDto.Where(t => t.Id == item.LineID).Select(t => t.UnitPrice).FirstOrDefault(),
                            UnitSetCode = item.UnitSetCode,
                            UnitSetID = item.UnitSetID,

                        };
                        stockFicheLineList.Add(stockFicheLineModel);
                        var line = DataSource.SelectPurchaseOrderLinesDto.Where(t => t.Id == item.LineID).FirstOrDefault();
                        int datasourcelineIndex = DataSource.SelectPurchaseOrderLinesDto.IndexOf(line);
                        DataSource.SelectPurchaseOrderLinesDto[datasourcelineIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi;
                    }
                }
            }

            CreateStockFichesDto stockFichesModel = new CreateStockFichesDto
            {
                BranchID = DataSource.BranchID.GetValueOrDefault(),
                CurrencyID = DataSource.CurrencyID.GetValueOrDefault(),
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                Description_ = string.Empty,
                ExchangeRate = DataSource.ExchangeRate,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                FicheType = 50,
                InputOutputCode = 0,
                NetAmount = DataSource.NetAmount,
                ProductionOrderID = DataSource.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderID = DataSource.Id,
                SpecialCode = DataSource.SpecialCode,
                WarehouseID = DataSource.WarehouseID.GetValueOrDefault(),
                Time_ = null,
            };

            stockFichesModel.SelectStockFicheLines = stockFicheLineList;

            await StockFichesAppService.CreateAsync(stockFichesModel);

            if (CreateStockFishesList.Where(t => t.SelectedLine == false).Count() == 0)
            {
                DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Tamamlandi;
            }
            else
            {
                DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.KismiTamamlandi;
            }

            var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);
            await PurchaseOrdersAppService.UpdateOrderCreateStockFichesAsync(updateInput);

            await ModalManager.MessagePopupAsync(L["UIInformationStockFichesCreatedTitle"], L["UIInformationStockFichesCreatedMessage"]);

            HideCreateStockFichesPopup();

            await InvokeAsync(StateHasChanged);
        }

        public void HideCreateStockFichesPopup()
        {

            CreateStockFishesList.Clear();
            CreateStockFishesCrudPopup = false;

        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = PurchaseOrdersAppService;
            _L = L;


            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PurchaseOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateStockReceiptFishesContextMenuItems();
            futureDateParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.FutureDateParameter;
            MaxDate = !futureDateParameter ? GetSQLDateAppService.GetDateFromSQL() : new DateTime(9999, 12, 31);
        }

        #region Fiyatlandırma Dövizi Enum Combobox

        public IEnumerable<SelectPurchaseOrdersDto> PricingCurrencyList = GetEnumPricingCurrencyNames<PricingCurrencyEnum>();

        public static List<SelectPurchaseOrdersDto> GetEnumPricingCurrencyNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PricingCurrencyEnum>()
                       .Select(x => new SelectPurchaseOrdersDto
                       {
                           PricingCurrency = x,
                           PricingCurrencyName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private async void PricingCurrencyValueChangeHandler(ChangeEventArgs<PricingCurrencyEnum, SelectPurchaseOrdersDto> args)
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

        #region Sipariş Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            var purchaseManagementParameter = (await PurchaseManagementParametersAppService.GetPurchaseManagementParametersAsync()).Data;
            DataSource = new SelectPurchaseOrdersDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersChildMenu"),
                PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Beklemede,
                PriceApprovalState = Entities.Enums.PurchaseOrderPriceApprovalStateEnum.Beklemede,
                PurchaseOrderWayBillStatusEnum = Entities.Enums.PurchaseOrderWayBillStatusEnum.Beklemede,
                BranchID = purchaseManagementParameter != null && purchaseManagementParameter.Id != Guid.Empty ? purchaseManagementParameter.DefaultBranchID : Guid.Empty,
                WarehouseID = purchaseManagementParameter != null && purchaseManagementParameter.Id != Guid.Empty ? purchaseManagementParameter.DefaultWarehouseID : Guid.Empty,
            };

            DataSource.SelectPurchaseOrderLinesDto = new List<SelectPurchaseOrderLinesDto>();
            GridLineList = DataSource.SelectPurchaseOrderLinesDto;


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
                            case "PurchaseOrderLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextAdd"], Id = "new" }); break;
                            case "PurchaseOrderLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextChange"], Id = "changed" }); break;
                            case "PurchaseOrderLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextDelete"], Id = "delete" }); break;
                            case "PurchaseOrderLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PurchaseOrderContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextAdd"], Id = "new" }); break;
                            case "PurchaseOrderContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextChange"], Id = "changed" }); break;
                            case "PurchaseOrderContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextDelete"], Id = "delete" }); break;
                            case "PurchaseOrderContextApproveOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextApproveOrder"], Id = "approveorder" }); break;
                            case "PurchaseOrderContextCreateStockFiches":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextCreateStockFiches"], Id = "createstockfiches" }); break;
                            case "PurchaseOrderContextCancelOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextCancelOrder"], Id = "cancelorder" }); break;
                            case "PurchaseOrderContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextRefresh"], Id = "refresh" }); break;
                            case "PurchaseOrderContextPriceApproval":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextPriceApproval"], Id = "priceApproval" }); break;
                            case "PurchaseOrderContextWayBillApproval":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextWayBillApproval"], Id = "waybillapproval" }); break;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPurchaseOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "approveorder":
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    if (DataSource.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Beklemede)
                    {
                        foreach (var line in GridLineList.ToList())
                        {
                            if (line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Beklemede)
                            {
                                int lineIndex = GridLineList.IndexOf(line);
                                line.PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Onaylandı;
                                GridLineList[lineIndex] = line;
                            }
                        }

                        DataSource.SelectPurchaseOrderLinesDto = GridLineList;
                        DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Onaylandı;
                        var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);
                        await PurchaseOrdersAppService.UpdateApproveOrderAsync(updateInput);
                        await ModalManager.MessagePopupAsync(L["UIInformationTitle"], L["UIInformationApproveOrder"]);

                    }

                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "createstockfiches":

                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    if (DataSource.PurchaseOrderState != Entities.Enums.PurchaseOrderStateEnum.Onaylandı)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningStockFichesTitle"], L["UIWarningStockFichesMessage"]);
                    }
                    else
                    {

                        if(DataSource.PurchaseOrderWayBillStatusEnum == PurchaseOrderWayBillStatusEnum.Beklemede)
                        {
                            await ModalManager.WarningPopupAsync(L["UIWarningStockFichesTitle"], L["UIWarningStockFichesWayBillMessage"]);
                        }
                        else
                        {
                            bool selectedLine = false;

                            foreach (var line in GridLineList)
                            {
                                if(line.PurchaseOrderLineWayBillStatusEnum != PurchaseOrderLineWayBillStatusEnum.Beklemede)
                                {
                                    if (line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                                    {
                                        selectedLine = true;
                                    }
                                    CreateStockReceiptFishes createStockReceiptFichesModel = new CreateStockReceiptFishes
                                    {
                                        PurchaseStateLine = line.PurchaseOrderLineStateEnum,
                                        ProductCode = line.ProductCode,
                                        ProductID = line.ProductID,
                                        ProductName = line.ProductName,
                                        Quantity = line.Quantity,
                                        UnitSetCode = line.UnitSetCode,
                                        UnitSetID = line.UnitSetID,
                                        SelectedLine = selectedLine,
                                        LineID = line.Id,
                                    };

                                    CreateStockFishesList.Add(createStockReceiptFichesModel);
                                }
                              
                            }

                            CreateStockFishesCrudPopup = true;
                        }
                        
                    }


                    await InvokeAsync(StateHasChanged);
                    break;

                case "cancelorder":
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    if (DataSource.PurchaseOrderState != Entities.Enums.PurchaseOrderStateEnum.Iptal)
                    {
                        bool selectedLine = true;

                        foreach (var line in GridLineList)
                        {
                            if (line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                            {
                                selectedLine = false;
                            }
                            CreateStockReceiptFishes createStockReceiptFichesModel = new CreateStockReceiptFishes
                            {
                                PurchaseStateLine = line.PurchaseOrderLineStateEnum,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                Quantity = line.Quantity,
                                UnitSetCode = line.UnitSetCode,
                                UnitSetID = line.UnitSetID,
                                SelectedLine = selectedLine,
                                LineID = line.Id,
                            };

                            CancelOrderList.Add(createStockReceiptFichesModel);
                        }

                        CancelOrderCrudPopup = true;
                    }


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

                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "priceApproval":
                    var order = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (order.PurchaseOrderState == PurchaseOrderStateEnum.Onaylandı)
                    {
                        var resConfirm = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIPriceStatusApprovalMessage"]);

                        if (resConfirm == true)
                        {
                            order.PriceApprovalState = Entities.Enums.PurchaseOrderPriceApprovalStateEnum.Onaylandi;
                            var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(order);
                            await PurchaseOrdersAppService.UpdateApproveBillAsync(updateInput);
                        }

                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningStateTitle"], L["UIWarningStateMessage"]);
                    }

                   
                    await InvokeAsync(StateHasChanged);
                    break;

                case "waybillapproval":

                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if(DataSource.PurchaseOrderState == PurchaseOrderStateEnum.Onaylandı)
                    {
                        var resWayBillConfirm = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIWayBillStatusApprovalMessage"]);

                        if (resWayBillConfirm == true)
                        {

                            DataSource.PurchaseOrderWayBillStatusEnum = PurchaseOrderWayBillStatusEnum.Onaylandi;

                            var updatedEntity = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);

                            await PurchaseOrdersAppService.UpdateApproveWayBillAsync(updatedEntity);
                        }

                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningStateTitle"], L["UIWarningStateWayBillMessage"]);
                    }
                    

                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseOrderLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectPurchaseOrderLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.PaymentPlanID = DataSource.PaymentPlanID;
                    LineDataSource.PaymentPlanName = DataSource.PaymentPlanName;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
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
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectPurchaseOrderLinesDto.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPurchaseOrderLinesDto.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPurchaseOrderLinesDto.Remove(line);
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
                    if (DataSource.SelectPurchaseOrderLinesDto.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectPurchaseOrderLinesDto.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectPurchaseOrderLinesDto[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectPurchaseOrderLinesDto.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectPurchaseOrderLinesDto.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPurchaseOrderLinesDto[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectPurchaseOrderLinesDto;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        public void LineQuantityReserved()
        {
            LineDataSource.PurchaseReservedQuantity = LineDataSource.Quantity - LineDataSource.WaitingQuantity;
        }

        public void LineQuantityWaiting()
        {
            LineDataSource.WaitingQuantity = LineDataSource.Quantity - LineDataSource.PurchaseReservedQuantity;
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
            SelectPurchaseOrdersDto result;

            if (DataSource.Id == Guid.Empty)
            {
                foreach (var item in DataSource.SelectPurchaseOrderLinesDto)
                {
                    int itemIndex = DataSource.SelectPurchaseOrderLinesDto.IndexOf(item);
                    DataSource.SelectPurchaseOrderLinesDto[itemIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Beklemede;
                }

                var createInput = ObjectMapper.Map<SelectPurchaseOrdersDto, CreatePurchaseOrdersDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);

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
        }


        #endregion

        #region GetList Metotları

        private async Task GetLineProductionOrdersList()
        {
            LineProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
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

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

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

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
