using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesOrder
{
    public partial class SalesOrdersListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        #endregion

        private SfGrid<ListSalesOrderDto> _grid;
        private SfGrid<SelectSalesOrderLinesDto> _LineGrid;
        private SfGrid<SelectSalesOrderLinesDto> _ProductionOrderGrid;
        private SfGrid<ProductsTreeDto> _BoMLineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectSalesOrderLinesDto LineDataSource;

        SelectBillsofMaterialsDto BoMDataSource;
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
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = SalesOrdersAppService;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateProductionOrderContextMenuItems();
            CreateBoMLineContextMenuItems();

            await GetProductsList();
        }

        #region Reçete Satırları Modalı İşlemleri

        protected void CreateBoMLineContextMenuItems()
        {
            //if (BoMLineGridContextMenu.Count() == 0)
            //{
            //    BoMLineGridContextMenu.Add(new ContextMenuItemModel { Text = "Ürün Ağacı", Id = "productstree" });
            //}
        }

        public async void OnBoMLineContextMenuClick(ContextMenuClickEventArgs<ProductsTreeDto> args)
        {
            //switch (args.Item.Id)
            //{
            //    case "productstree":

            //        break;

            //    default:
            //        break;
            //}
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
            if (ProductionOrderGridContextMenu.Count() == 0)
            {
                ProductionOrderGridContextMenu.Add(new ContextMenuItemModel { Text = "Ürün Ağacı", Id = "productstree" });
            }
        }

        public async void OnCreateProductionOrderContextMenuClick(ContextMenuClickEventArgs<SelectSalesOrderLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "productstree":

                    BoMList = (await BillsofMaterialsAppService.GetListAsync(new ListBillsofMaterialsParameterDto())).Data.Where(t => t.FinishedProductCode == args.RowInfo.RowData.ProductCode).ToList();

                    Guid BoMID = BoMList.Select(t => t.Id).FirstOrDefault();

                    BoMDataSource = (await BillsofMaterialsAppService.GetAsync(BoMID)).Data;

                    GridBoMLineList = BoMDataSource.SelectBillsofMaterialLines;

                    foreach (var item in GridBoMLineList)
                    {
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;

                        ProductsTreeDto _model = new ProductsTreeDto
                        {
                            ProductName = item.ProductName,
                            ProductCode = item.ProductCode,
                            AmountofStock = 30, //deneme
                            AmountofRequierement = Math.Abs(30 - (GridBoMLineList.Count * item.Quantity)),
                            SupplyForm = 1 //deneme
                        };

                        ProductTreeDataSource.Add(_model);
                    }

                    BoMLineCrudPopup = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }
        }

        protected async Task OnCreateProductionOrderBtnClicked()
        {

        }

        public void HideCreateProductionOrderPopup()
        {
            CreateProductionOrderCrudPopup = false;
        }

        #endregion

        #region Teklif Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectSalesOrderDto()
            {
                Date_ = DateTime.Today
            };

            DataSource.SelectSalesOrderLines = new List<SelectSalesOrderLinesDto>();
            GridLineList = DataSource.SelectSalesOrderLines;

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
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Üretim Emri Oluştur", Id = "createproductionorder" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
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
                    DataSource = (await SalesOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectSalesOrderLines;

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
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satış teklifi kalıcı olarak silinecektir.");
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
                    LineDataSource = new SelectSalesOrderLinesDto();
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
    }
}
