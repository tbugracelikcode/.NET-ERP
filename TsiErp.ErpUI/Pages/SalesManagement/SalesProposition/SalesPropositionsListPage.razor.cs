using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.SalesProposition
{
    public partial class SalesPropositionsListPage
    {
        private SfGrid<SelectSalesPropositionLinesDto> _LineGrid;
        private SfGrid<SelectSalesPropositionLinesDto> _ConvertToOrderGrid;

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectSalesPropositionLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ConvertToOrderGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectSalesPropositionLinesDto> GridLineList = new List<SelectSalesPropositionLinesDto>();

        List<SelectSalesPropositionLinesDto> GridConvertToOrderList = new List<SelectSalesPropositionLinesDto>();

        private bool LineCrudPopup = false;

        private bool ConvertToOrderCrudPopup = false;

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

        SfTextBox CurrentAccountCardsCodeButtonEdit;
        SfTextBox CurrentAccountCardsCustomerCodeButtonEdit;
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

        public async Task CurrentAccountCardsCustomerCodeOnCreateIcon()
        {
            var CurrentAccountCardsCustomerCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCustomerCodeButtonClickEvent);
            await CurrentAccountCardsCustomerCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCustomerCodeButtonClick } });
        }

        public async void CurrentAccountCardsCustomerCodeButtonClickEvent()
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
                DataSource.CustomerCode = selectedUnitSet.CustomerCode;
                SelectCurrentAccountCardsPopupVisible = false;
                ShippingAdressEnable = true;
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
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        protected override async void OnInitialized()
        {
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateConvertToOrderContextMenuItems();

            BaseCrudService = SalesPropositionsAppService;
            _L = L;

            futureDateParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.FutureDateParameter;

        }

        #region Satışı Teklife Dönüştürme Modalı İşlemleri

        protected void CreateConvertToOrderContextMenuItems()
        {
            if (ConvertToOrderGridContextMenu.Count() == 0)
            {
                ConvertToOrderGridContextMenu.Add(new ContextMenuItemModel { Text = "Onaylandı", Id = "approve" });
                ConvertToOrderGridContextMenu.Add(new ContextMenuItemModel { Text = "Beklemede", Id = "onhold" });
            }
        }

        public async void OnConvertToOrderContextMenuClick(ContextMenuClickEventArgs<SelectSalesPropositionLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "approve":


                    var selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

                    foreach (var item in selectedIndexList)
                    {
                        if (GridConvertToOrderList[Convert.ToInt32(item)].SalesPropositionLineState != Entities.Enums.SalesPropositionLineStateEnum.Siparis)
                        {
                            GridConvertToOrderList[Convert.ToInt32(item)].SalesPropositionLineState = Entities.Enums.SalesPropositionLineStateEnum.Onaylandı;
                        }

                    }

                    DataSource.SelectSalesPropositionLines = GridConvertToOrderList;

                    #region Teklif Durumunu Onaylandı Kaydetme

                    SelectSalesPropositionsDto result;

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectSalesPropositionsDto, CreateSalesPropositionsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>(DataSource);

                        result = (await UpdateAsync(updateInput)).Data;
                    }

                    #endregion

                    await _ConvertToOrderGrid.Refresh();

                    break;

                case "onhold":

                    selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

                    foreach (var item in selectedIndexList)
                    {
                        if (GridConvertToOrderList[Convert.ToInt32(item)].SalesPropositionLineState != Entities.Enums.SalesPropositionLineStateEnum.Siparis)
                        {
                            GridConvertToOrderList[Convert.ToInt32(item)].SalesPropositionLineState = Entities.Enums.SalesPropositionLineStateEnum.Beklemede;
                        }
                    }

                    DataSource.SelectSalesPropositionLines = GridConvertToOrderList;

                    #region Teklif Durumunu Beklemede Olarak Kaydetme

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectSalesPropositionsDto, CreateSalesPropositionsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>(DataSource);

                        result = (await UpdateAsync(updateInput)).Data;
                    }

                    #endregion

                    await _ConvertToOrderGrid.Refresh();

                    break;

                default:
                    break;
            }
        }

        protected async Task OnConvertToOrderBtnClicked()
        {

            List<SelectSalesPropositionLinesDto> _orderList = new List<SelectSalesPropositionLinesDto>();
            var selectedRowList = _ConvertToOrderGrid.SelectedRecords;
            var selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

            foreach (var item in selectedRowList)
            {
                _orderList.Add(item);
            }

            var approvedSelected = _orderList.Where(t => t.SalesPropositionLineState == Entities.Enums.SalesPropositionLineStateEnum.Onaylandı).ToList();

            if (approvedSelected.Count > 0)
            {
                #region Teklifi Siparişe Çevirme

                CreateSalesOrderDto createSalesOrder = new CreateSalesOrderDto
                {
                    BranchID = DataSource.BranchID,
                    CurrencyID = DataSource.CurrencyID,
                    CurrentAccountCardID = DataSource.CurrentAccountCardID,
                    Date_ = DataSource.Date_,
                    Description_ = DataSource.Description_,
                    ExchangeRate = DataSource.ExchangeRate,
                    GrossAmount = DataSource.GrossAmount,
                    LinkedSalesPropositionID = DataSource.Id,
                    FicheNo = "DENEME 9",
                    NetAmount = DataSource.NetAmount,
                    PaymentPlanID = DataSource.PaymentPlanID,
                    SalesOrderState = (int)Entities.Enums.SalesOrderStateEnum.Beklemede,
                    ShippingAdressID = DataSource.ShippingAdressID,
                    SpecialCode = DataSource.SpecialCode,
                    Time_ = DataSource.Time_,
                    TotalDiscountAmount = DataSource.TotalDiscountAmount,
                    TotalVatAmount = DataSource.TotalVatAmount,
                    TotalVatExcludedAmount = DataSource.TotalVatExcludedAmount,
                    WarehouseID = DataSource.WarehouseID
                };

                List<SelectSalesOrderLinesDto> orderLineList = new List<SelectSalesOrderLinesDto>();

                foreach (var item in approvedSelected)
                {
                    SelectSalesOrderLinesDto selectSalesOrderLine = new SelectSalesOrderLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LikedPropositionLineID = item.Id,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineNr = item.LineNr,
                        LineTotalAmount = item.LineTotalAmount,
                        PaymentPlanID = item.PaymentPlanID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        SalesOrderID = Guid.Empty,
                        SalesOrderLineStateEnum = Entities.Enums.SalesOrderLineStateEnum.Beklemede,
                        UnitPrice = item.UnitPrice,
                        UnitSetID = item.UnitSetID,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        LinkedSalesPropositionID = createSalesOrder.LinkedSalesPropositionID
                    };
                    orderLineList.Add(selectSalesOrderLine);
                    item.SalesPropositionLineState = Entities.Enums.SalesPropositionLineStateEnum.Siparis;
                }

                createSalesOrder.SelectSalesOrderLines = orderLineList;

                await SalesOrdersAppService.ConvertToSalesOrderAsync(createSalesOrder);

                #endregion

                await ModalManager.MessagePopupAsync("Bilgilendirme", "Seçilen onaylanmış satırlar, siparişe dönüştürüldü.");
            }
            else
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Seçilen tüm satırların durumu beklemededir. Beklemede olan satırlar, siparişe dönüştürülemez.");
            }

            #region Ana Satırın Sipariş Durumunu Belirleme

            int approvedMain = GridConvertToOrderList.Where(t => t.SalesPropositionLineState == Entities.Enums.SalesPropositionLineStateEnum.Onaylandı).Count();
            int onholdMain = GridConvertToOrderList.Where(t => t.SalesPropositionLineState == Entities.Enums.SalesPropositionLineStateEnum.Beklemede).Count();
            int orderMain = GridConvertToOrderList.Where(t => t.SalesPropositionLineState == Entities.Enums.SalesPropositionLineStateEnum.Siparis).Count();

            #region Koşullar

            if (orderMain != 0 && orderMain != GridConvertToOrderList.Count())
            {
                DataSource.SalesPropositionState = Entities.Enums.SalesPropositionStateEnum.KismiSiparis;
            }
            else if (orderMain == GridConvertToOrderList.Count())
            {
                DataSource.SalesPropositionState = Entities.Enums.SalesPropositionStateEnum.Siparis;
            }
            else if (approvedMain == GridConvertToOrderList.Count())
            {
                DataSource.SalesPropositionState = Entities.Enums.SalesPropositionStateEnum.Onaylandı;
            }
            else if (approvedMain != 0 && approvedMain != GridConvertToOrderList.Count())
            {
                DataSource.SalesPropositionState = Entities.Enums.SalesPropositionStateEnum.KismiOnaylandi;
            }
            else if (onholdMain == GridConvertToOrderList.Count())
            {
                DataSource.SalesPropositionState = Entities.Enums.SalesPropositionStateEnum.Beklemede;
            }

            #endregion

            #endregion

            GridConvertToOrderList = DataSource.SelectSalesPropositionLines;

            #region Teklif Durumunu Sipariş Olarak Kaydetme

            SelectSalesPropositionsDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectSalesPropositionsDto, CreateSalesPropositionsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            #endregion

            GetTotal();
            await _ConvertToOrderGrid.Refresh();
            await InvokeAsync(StateHasChanged);
        }

        public void HideConvertToOrderPopup()
        {
            ConvertToOrderCrudPopup = false;
        }

        #endregion

        #region Teklif Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectSalesPropositionsDto()
            {
                Date_ = DateTime.Today,
                ValidityDate_ = DateTime.Today.AddDays(15)
            };

            DataSource.SelectSalesPropositionLines = new List<SelectSalesPropositionLinesDto>();
            GridLineList = DataSource.SelectSalesPropositionLines;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextConverttoOrder"], Id = "converttoorder" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListSalesPropositionsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await SalesPropositionsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectSalesPropositionLines;

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

                case "converttoorder":

                    DataSource = (await SalesPropositionsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridConvertToOrderList = DataSource.SelectSalesPropositionLines;

                    foreach (var item in GridConvertToOrderList)
                    {
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    ConvertToOrderCrudPopup = true;
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectSalesPropositionLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectSalesPropositionLinesDto();
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
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectSalesPropositionLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectSalesPropositionLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectSalesPropositionLines.Remove(line);
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

        protected override async Task OnSubmit()
        {
            SelectSalesPropositionsDto result;

            foreach (var item in DataSource.SelectSalesPropositionLines)
            {
                item.SalesPropositionLineState = Entities.Enums.SalesPropositionLineStateEnum.Beklemede;
            }


            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectSalesPropositionsDto, CreateSalesPropositionsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>(DataSource);

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
                    if (DataSource.SelectSalesPropositionLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectSalesPropositionLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectSalesPropositionLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectSalesPropositionLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectSalesPropositionLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectSalesPropositionLines[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectSalesPropositionLines;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }
          
        }

        public override async void LineCalculate()
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
                }
            }
            else
            {
                LineDataSource.DiscountRate = 0;
            }

            LineDataSource.LineTotalAmount = LineDataSource.LineAmount + LineDataSource.VATamount;

            await InvokeAsync(StateHasChanged);
        }

        public override void GetTotal()
        {
            DataSource.GrossAmount = GridLineList.Sum(x => x.LineAmount) + GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalDiscountAmount = GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalVatExcludedAmount = DataSource.GrossAmount - DataSource.TotalDiscountAmount;
            DataSource.TotalVatAmount = GridLineList.Sum(x => x.VATamount);
            DataSource.NetAmount = GridLineList.Sum(x => x.LineTotalAmount);
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



    }
}
