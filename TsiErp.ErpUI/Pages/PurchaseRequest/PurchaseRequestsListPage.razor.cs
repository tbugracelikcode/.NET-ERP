using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.ErpUI.Helpers;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.TreeGrid;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;
using Syncfusion.Blazor.Inputs;
using Microsoft.AspNetCore.Components.Web;

namespace TsiErp.ErpUI.Pages.PurchaseRequest
{
    public partial class PurchaseRequestsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListProductionOrdersDto> ProductionOrdersComboBox;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        SfComboBox<string, ListProductionOrdersDto> LineProductionOrdersComboBox;
        List<ListProductionOrdersDto> LineProductionOrdersList = new List<ListProductionOrdersDto>();

        #endregion

        private SfGrid<ListPurchaseRequestsDto> _grid;
        private SfGrid<SelectPurchaseRequestLinesDto> _LineGrid;
        private SfGrid<SelectPurchaseRequestLinesDto> _ConvertToOrderGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPurchaseRequestLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ConvertToOrderGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseRequestLinesDto> GridLineList = new List<SelectPurchaseRequestLinesDto>();

        List<SelectPurchaseRequestLinesDto> GridConvertToOrderList = new List<SelectPurchaseRequestLinesDto>();

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
                DataSource.WarehouseName = string.Empty;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.WarehouseID = selectedUnitSet.Id;
                DataSource.WarehouseCode = selectedUnitSet.Code;
                DataSource.WarehouseName = selectedUnitSet.Name;
                SelectWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseRequestsAppService;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateConvertToOrderContextMenuItems();


            await GetCurrentAccountCardsList();
            await GetProductsList();
            await GetProductionOrdersList();
            await LineGetProductionOrdersList();
        }

        #region Satın Almayı Talebe Dönüştürme Modalı İşlemleri

        protected void CreateConvertToOrderContextMenuItems()
        {
            if (ConvertToOrderGridContextMenu.Count() == 0)
            {
                ConvertToOrderGridContextMenu.Add(new ContextMenuItemModel { Text = "Onaylandı", Id = "approve" });
                ConvertToOrderGridContextMenu.Add(new ContextMenuItemModel { Text = "Beklemede", Id = "onhold" });
            }
        }

        public async void OnConvertToOrderContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseRequestLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "approve":


                    var selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

                    foreach (var item in selectedIndexList)
                    {
                        if (GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState != Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma)
                        {
                            GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı;
                        }

                    }

                    DataSource.SelectPurchaseRequestLines = GridConvertToOrderList;

                    #region Talep Durumunu Onaylandı Kaydetme

                    SelectPurchaseRequestsDto result;

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>(DataSource);

                        result = (await UpdateAsync(updateInput)).Data;
                    }

                    #endregion

                    await _ConvertToOrderGrid.Refresh();

                    break;

                case "onhold":

                    selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

                    foreach (var item in selectedIndexList)
                    {
                        if (GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState != Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma)
                        {
                            GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Beklemede;
                        }
                    }

                    DataSource.SelectPurchaseRequestLines = GridConvertToOrderList;

                    #region Teklif Durumunu Beklemede Olarak Kaydetme

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>(DataSource);

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

            List<SelectPurchaseRequestLinesDto> _orderList = new List<SelectPurchaseRequestLinesDto>();
            var selectedRowList = _ConvertToOrderGrid.SelectedRecords;
            var selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

            foreach (var item in selectedRowList)
            {
                _orderList.Add(item);
            }

            var approvedSelected = _orderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı).ToList();

            if (approvedSelected.Count > 0)
            {
                #region Talebi Siparişe Çevirme

                CreatePurchaseOrdersDto createPurchaseOrder = new CreatePurchaseOrdersDto
                {
                    BranchID = DataSource.BranchID,
                    CurrencyID = DataSource.CurrencyID,
                    CurrentAccountCardID = DataSource.CurrentAccountCardID,
                    Date_ = DataSource.Date_,
                    Description_ = DataSource.Description_,
                    ExchangeRate = DataSource.ExchangeRate,
                    GrossAmount = DataSource.GrossAmount,
                    LinkedPurchaseRequestID = DataSource.Id,
                    FicheNo = "DENEME 1",
                    NetAmount = DataSource.NetAmount,
                    PaymentPlanID = DataSource.PaymentPlanID,
                    PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Beklemede,
                    ShippingAdressID = DataSource.ShippingAdressID,
                    SpecialCode = DataSource.SpecialCode,
                    Time_ = DataSource.Time_,
                    TotalDiscountAmount = DataSource.TotalDiscountAmount,
                    TotalVatAmount = DataSource.TotalVatAmount,
                    TotalVatExcludedAmount = DataSource.TotalVatExcludedAmount,
                    WarehouseID = DataSource.WarehouseID,
                    ProductionOrderID = DataSource.ProductionOrderID
                };

                List<SelectPurchaseOrderLinesDto> orderLineList = new List<SelectPurchaseOrderLinesDto>();

                foreach (var item in approvedSelected)
                {
                    SelectPurchaseOrderLinesDto selectPurchaseOrderLine = new SelectPurchaseOrderLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LikedPurchaseRequestLineID = item.Id,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineNr = item.LineNr,
                        LineTotalAmount = item.LineTotalAmount,
                        PaymentPlanID = item.PaymentPlanID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        PurchaseOrderID = Guid.Empty,
                        PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Beklemede,
                        UnitPrice = item.UnitPrice,
                        UnitSetID = item.UnitSetID,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                         LinkedPurchaseRequestID= createPurchaseOrder.LinkedPurchaseRequestID
                    };
                    orderLineList.Add(selectPurchaseOrderLine);
                    item.PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma;
                }

                createPurchaseOrder.SelectPurchaseOrderLinesDto = orderLineList;

                await PurchaseOrdersAppService.ConvertToPurchaseOrderAsync(createPurchaseOrder);

                #endregion

                await ModalManager.MessagePopupAsync("Bilgilendirme", "Seçilen onaylanmış satırlar, siparişe dönüştürüldü.");
            }
            else
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Seçilen tüm satırların durumu beklemededir. Beklemede olan satırlar, siparişe dönüştürülemez.");
            }

            #region Ana Satırın Sipariş Durumunu Belirleme

            int approvedMain = GridConvertToOrderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı).Count();
            int onholdMain = GridConvertToOrderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.Beklemede).Count();
            int orderMain = GridConvertToOrderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma).Count();

            #region Koşullar

            if (orderMain != 0 && orderMain != GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.KismiSatinAlma;
            }
            else if (orderMain == GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.SatinAlma;
            }
            else if (approvedMain == GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.Onaylandı;
            }
            else if (approvedMain != 0 && approvedMain != GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.KismiOnaylandi;
            }
            else if (onholdMain == GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.Beklemede;
            }

            #endregion

            #endregion

            GridConvertToOrderList = DataSource.SelectPurchaseRequestLines;

            #region Teklif Durumunu Sipariş Olarak Kaydetme

            SelectPurchaseRequestsDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>(DataSource);

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
            DataSource = new SelectPurchaseRequestsDto()
            {
                Date_ = DateTime.Today,
                ValidityDate_ = DateTime.Today.AddDays(15)
            };

            DataSource.SelectPurchaseRequestLines = new List<SelectPurchaseRequestLinesDto>();
            GridLineList = DataSource.SelectPurchaseRequestLines;

            ShowEditPage();


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Siparişe Dönüştür", Id = "converttoorder" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPurchaseRequestsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await PurchaseRequestsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseRequestLines;

                    foreach (var item in GridLineList)
                    {
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satın alma talebi, kalıcı olarak silinecektir.");
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "converttoorder":

                    DataSource = (await PurchaseRequestsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridConvertToOrderList = DataSource.SelectPurchaseRequestLines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseRequestLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectPurchaseRequestLinesDto();
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

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectPurchaseRequestLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPurchaseRequestLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPurchaseRequestLines.Remove(line);
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
            LineDataSource.PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Beklemede;

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectPurchaseRequestLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectPurchaseRequestLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPurchaseRequestLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPurchaseRequestLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPurchaseRequestLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPurchaseRequestLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPurchaseRequestLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
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

        #region ComboBox İşlemleri

        #region Cari Hesap Kartları
        public async Task CurrentAccountCardFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await CurrentAccountCardsComboBox.FilterAsync(CurrentAccountCardsList, query);
        }

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        public async Task CurrentAccountCardValueChangeHandler(ChangeEventArgs<string, ListCurrentAccountCardsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.CurrentAccountCardID = args.ItemData.Id;
                DataSource.CurrentAccountCardCode = args.ItemData.Code;
                DataSource.CurrentAccountCardName = args.ItemData.Name;
            }
            else
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Üretim Emirleri
        public async Task ProductionOrderFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductionOrdersComboBox.FilterAsync(ProductionOrdersList, query);
        }

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }
        public async Task ProductionOrderValueChangeHandler(ChangeEventArgs<string, ListProductionOrdersDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ProductionOrderID = args.ItemData.Id;
                DataSource.ProductionOrderFicheNo = args.ItemData.FicheNo;
            }
            else
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

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

        #region Stok Kartları -Teklif Satırları
        public async Task ProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductsComboBox.FilterAsync(ProductsList, query);
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }
        public async Task ProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.ProductID = args.ItemData.Id;
                LineDataSource.ProductCode = args.ItemData.Code;
                LineDataSource.ProductName = args.ItemData.Name;
                LineDataSource.UnitSetID = args.ItemData.UnitSetID;
                LineDataSource.UnitSetCode = args.ItemData.UnitSetCode;
                LineDataSource.VATrate = args.ItemData.SaleVAT;
            }
            else
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
                LineDataSource.VATrate = 0;
            }
            LineCalculate();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetPaymentPlansList()
        {
            PaymentPlansList = (await PaymentPlansAppService.GetListAsync(new ListPaymentPlansParameterDto())).Data.ToList();
        }

        #region Üretim Emirleri - Talep Satırları
        public async Task LineProductionOrderFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineProductionOrdersComboBox.FilterAsync(LineProductionOrdersList, query);
        }

        private async Task LineGetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }
        public async Task LineProductionOrderValueChangeHandler(ChangeEventArgs<string, ListProductionOrdersDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.ProductionOrderID = args.ItemData.Id;
                LineDataSource.ProductionOrderFicheNo = args.ItemData.FicheNo;
            }
            else
            {
                LineDataSource.ProductionOrderID = Guid.Empty;
                LineDataSource.ProductionOrderFicheNo = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #endregion

    }
}
