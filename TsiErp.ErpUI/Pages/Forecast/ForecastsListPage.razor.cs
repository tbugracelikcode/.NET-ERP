using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Forecast.Dtos;
using TsiErp.Entities.Entities.ForecastLine.Dtos;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Forecast
{
    public partial class ForecastsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<Guid?, ListProductsDto> LineProductsComboBox;
        List<ListProductsDto> LineProductsList = new List<ListProductsDto>();

        SfComboBox<Guid?, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        SfComboBox<Guid?, ListBranchesDto> BranchesComboBox;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        SfComboBox<Guid?, ListPeriodsDto> PeriodsComboBox;
        List<ListPeriodsDto> PeriodsList = new List<ListPeriodsDto>();

        #endregion

        private SfGrid<ListForecastsDto> _grid;
        private SfGrid<SelectForecastLinesDto> _LineGrid;

        SelectForecastLinesDto LineDataSource;

        [Inject]
        ModalManager ModalManager { get; set; }
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectForecastLinesDto> GridLineList = new List<SelectForecastLinesDto>();

        private bool LineCrudPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ForecastsAppService;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            await GetCurrentAccountCardsList();
            await GetBranchesList();
            await GetPeriodsList();
            await GetLineProductsList();
        }

        #region Forecast Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectForecastsDto()
            {
                CreationDate_ = DateTime.Today,
                ValidityStartDate = DateTime.Today,
                ValidityEndDate = DateTime.Today

            };

            DataSource.SelectForecastLines = new List<SelectForecastLinesDto>();
            GridLineList = DataSource.SelectForecastLines;

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListForecastsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await ForecastsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectForecastLines;

                    foreach (var item in GridLineList)
                    {
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Name;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz forecast kalıcı olarak silinecektir.");
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectForecastLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectForecastLinesDto();
                    LineCrudPopup = true;
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
                            DataSource.SelectForecastLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectForecastLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectForecastLines.Remove(line);
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
                if (DataSource.SelectForecastLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectForecastLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectForecastLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectForecastLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectForecastLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectForecastLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectForecastLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
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

        public async Task CurrentAccountCardValueChangeHandler(ChangeEventArgs<Guid?, ListCurrentAccountCardsDto> args)
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

        public async Task BranchValueChangeHandler(ChangeEventArgs<Guid?, ListBranchesDto> args)
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

        #region Dönemler
        public async Task PeriodsFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await PeriodsComboBox.FilterAsync(PeriodsList, query);
        }

        private async Task GetPeriodsList()
        {
            PeriodsList = (await PeriodsAppService.GetListAsync(new ListPeriodsParameterDto())).Data.ToList();
        }

        public async Task PeriodValueChangeHandler(ChangeEventArgs<Guid?, ListPeriodsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.PeriodID = args.ItemData.Id;
                DataSource.PeriodCode = args.ItemData.Code;
                DataSource.PeriodName = args.ItemData.Name;
            }
            else
            {
                DataSource.PeriodID = args.ItemData.Id;
                DataSource.PeriodCode = args.ItemData.Code;
                DataSource.PeriodName = args.ItemData.Name;
            }
            await InvokeAsync(StateHasChanged);
        }


        #endregion

        #region Stok Kartları - Forecast Satırları
        public async Task LineProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineProductsComboBox.FilterAsync(LineProductsList, query);
        }

        private async Task GetLineProductsList()
        {
            LineProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }
        public async Task LineProductValueChangeHandler(ChangeEventArgs<Guid?, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.ProductID = args.ItemData.Id;
                LineDataSource.ProductCode = args.ItemData.Code;
                LineDataSource.ProductName = args.ItemData.Name;
            }
            else
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
            }
            LineCalculate();
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
