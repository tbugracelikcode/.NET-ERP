using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesProposition
{
    public partial class SalesPropositionsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListPaymentPlansDto> PaymentPlansComboBox;
        List<ListPaymentPlansDto> PaymentPlansList = new List<ListPaymentPlansDto>();

        SfComboBox<string, ListUnitSetsDto> UnitSetsComboBox;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        SfComboBox<string, ListBranchesDto> BranchesComboBox;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        SfComboBox<string, ListWarehousesDto> WarehousesComboBox;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        SfComboBox<string, ListCurrenciesDto> CurrenciesComboBox;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        #endregion

        private SfGrid<ListSalesPropositionsDto> _grid;
        private SfGrid<SelectSalesPropositionLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectSalesPropositionLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectSalesPropositionLinesDto> GridLineList = new List<SelectSalesPropositionLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = SalesPropositionsAppService;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            await GetCurrentAccountCardsList();
            await GetBranchesList();
            await GetWarehousesList();
            await GetCurrenciesList();
            await GetProductsList();
            await GetUnitSetsList();
            await GetPaymentPlansList();
        }


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
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
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
                    DataSource = (await SalesPropositionsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectSalesPropositionLines;

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

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

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

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if(DataSource.SelectSalesPropositionLines.Contains(LineDataSource))
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

        #region Şubeler
        public async Task BranchFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await BranchesComboBox.FilterAsync(BranchesList, query);
        }

        private async Task GetBranchesList()
        {
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        public async Task BranchValueChangeHandler(ChangeEventArgs<string, ListBranchesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.BranchID = args.ItemData.Id;
                DataSource.BranchCode = args.ItemData.Code;
            }
            else
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }


        #endregion

        #region Depolar
        public async Task WarehouseFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await WarehousesComboBox.FilterAsync(WarehousesList, query);
        }

        private async Task GetWarehousesList()
        {
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        public async Task WarehouseValueChangeHandler(ChangeEventArgs<string, ListWarehousesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.WarehouseID = args.ItemData.Id;
                DataSource.WarehouseCode = args.ItemData.Code;
            }
            else
            {
                DataSource.WarehouseID = Guid.Empty;
                DataSource.WarehouseCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Para Birimleri
        public async Task CurrencyFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await CurrenciesComboBox.FilterAsync(CurrenciesList, query);
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }
        public async Task CurrencyValueChangeHandler(ChangeEventArgs<string, ListCurrenciesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.CurrencyID = args.ItemData.Id;
                DataSource.CurrencyCode = args.ItemData.Code;
            }
            else
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

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

        #region Birim Setleri -Teklif Satırları
        public async Task UnitSetFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await UnitSetsComboBox.FilterAsync(UnitSetsList, query);
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        public async Task UnitSetValueChangeHandler(ChangeEventArgs<string, ListUnitSetsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.UnitSetID = args.ItemData.Id;
                LineDataSource.UnitSetCode = args.ItemData.Code;
            }
            else
            {
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Ödeme Planları - Teklif Satırları
        public async Task PaymentPlanFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await PaymentPlansComboBox.FilterAsync(PaymentPlansList, query);
        }

        private async Task GetPaymentPlansList()
        {
            PaymentPlansList = (await PaymentPlansAppService.GetListAsync(new ListPaymentPlansParameterDto())).Data.ToList();
        }

        public async Task PaymentPlanValueChangeHandler(ChangeEventArgs<string, ListPaymentPlansDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.PaymentPlanID = args.ItemData.Id;
                DataSource.PaymentPlanName = args.ItemData.Name;


                foreach (var item in DataSource.SelectSalesPropositionLines)
                {
                    if (item.PaymentPlanID == Guid.Empty)
                    {
                        item.PaymentPlanID = DataSource.PaymentPlanID;
                        item.PaymentPlanName = DataSource.PaymentPlanName;
                    }
                }
            }
            else
            {
                DataSource.PaymentPlanID = Guid.Empty;
                DataSource.PaymentPlanName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
