using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MaintenanceInstruction
{
    public partial class MaintenanceInstructionsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListUnitSetsDto> LineUnitSetsComboBox;
        List<ListUnitSetsDto> LineUnitSetsList = new List<ListUnitSetsDto>();

        SfComboBox<Guid?, ListProductsDto> LineProductsComboBox;
        List<ListProductsDto> LineProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListStationsDto> StationsComboBox;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        SfComboBox<string, ListMaintenancePeriodsDto> MaintenancePeriodsComboBox;
        List<ListMaintenancePeriodsDto> MaintenancePeriodsList = new List<ListMaintenancePeriodsDto>();

        #endregion

        private SfGrid<ListMaintenanceInstructionsDto> _grid;
        private SfGrid<SelectMaintenanceInstructionLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectMaintenanceInstructionLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMaintenanceInstructionLinesDto> GridLineList = new List<SelectMaintenanceInstructionLinesDto>();

        private bool LineCrudPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = MaintenanceInstructionsAppService;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            await GetLineProductsList();
            await GetLineUnitSetsList();
            await GetStationsList();
            await GetMaintenancePeriodsList();
        }

        #region Bakım Talimatları Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenanceInstructionsDto(){};

            DataSource.SelectMaintenanceInstructionLines = new List<SelectMaintenanceInstructionLinesDto>();
            GridLineList = DataSource.SelectMaintenanceInstructionLines;

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMaintenanceInstructionsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await MaintenanceInstructionsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMaintenanceInstructionLines;

                    foreach (var item in GridLineList)
                    {
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectMaintenanceInstructionLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectMaintenanceInstructionLinesDto();
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
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectMaintenanceInstructionLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectMaintenanceInstructionLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectMaintenanceInstructionLines.Remove(line);
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
                if (DataSource.SelectMaintenanceInstructionLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectMaintenanceInstructionLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectMaintenanceInstructionLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectMaintenanceInstructionLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectMaintenanceInstructionLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectMaintenanceInstructionLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectMaintenanceInstructionLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Stok Kartları - Bakım Talimatları Satırları
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
            }
            else
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
            }
            LineCalculate();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Birim Setleri - Bakım Talimatları Satırları
        public async Task LineUnitSetFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineUnitSetsComboBox.FilterAsync(LineUnitSetsList, query);
        }

        private async Task GetLineUnitSetsList()
        {
            LineUnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        public async Task LineUnitSetValueChangeHandler(ChangeEventArgs<string, ListUnitSetsDto> args)
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

        #region İş İstasyonları

        public async Task StationFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await StationsComboBox.FilterAsync(StationsList, query);
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        public async Task StationValueChangeHandler(ChangeEventArgs<string, ListStationsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.StationID = args.ItemData.Id;
                DataSource.StationCode = args.ItemData.Code;
            }
            else
            {
                DataSource.StationID = Guid.Empty;
                DataSource.StationCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Bakım Periyotları

        public async Task MaintenancePeriodFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await MaintenancePeriodsComboBox.FilterAsync(MaintenancePeriodsList, query);
        }

        private async Task GetMaintenancePeriodsList()
        {
            MaintenancePeriodsList = (await MaintenancePeriodsAppService.GetListAsync(new ListMaintenancePeriodsParameterDto())).Data.ToList();
        }

        public async Task MaintenancePeriodValueChangeHandler(ChangeEventArgs<string, ListMaintenancePeriodsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.PeriodID = args.ItemData.Id;
                DataSource.PeriodName = args.ItemData.Name;
                DataSource.PeriodTime = args.ItemData.PeriodTime;
            }
            else
            {
                DataSource.PeriodID = Guid.Empty;
                DataSource.PeriodName = string.Empty;
                DataSource.PeriodTime = 0;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion


    }
}
