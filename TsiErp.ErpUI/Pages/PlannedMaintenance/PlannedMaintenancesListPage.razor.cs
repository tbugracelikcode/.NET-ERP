using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlannedMaintenance
{
    public partial class PlannedMaintenancesListPage
    {
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        private SfGrid<ListPlannedMaintenancesDto> _grid;
        private SfGrid<SelectPlannedMaintenanceLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPlannedMaintenanceLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPlannedMaintenanceLinesDto> GridLineList = new List<SelectPlannedMaintenanceLinesDto>();

        private bool LineCrudPopup = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = PlannedMaintenancesAppService;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
        }

        #region Planlı Bakımlar Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPlannedMaintenancesDto()
            {
                StartDate = DateTime.Today,
                PlannedDate = DateTime.Today,
                CompletionDate = DateTime.Today
            };

            DataSource.SelectPlannedMaintenanceLines = new List<SelectPlannedMaintenanceLinesDto>();
            GridLineList = DataSource.SelectPlannedMaintenanceLines;

            ShowEditPage();

            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPlannedMaintenancesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await PlannedMaintenancesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPlannedMaintenanceLines;

                    foreach (var item in GridLineList)
                    {
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz planlı bakım, kalıcı olarak silinecektir.");
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPlannedMaintenanceLinesDto> args)
        {
            switch (args.Item.Id)
            {
             
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
                            DataSource.SelectPlannedMaintenanceLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPlannedMaintenanceLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPlannedMaintenanceLines.Remove(line);
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
                if (DataSource.SelectPlannedMaintenanceLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectPlannedMaintenanceLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPlannedMaintenanceLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPlannedMaintenanceLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPlannedMaintenanceLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPlannedMaintenanceLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPlannedMaintenanceLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region İş İstasyonu ButtonEdit 

        SfTextBox StationsButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();
        public async Task StationsOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsButtonClickEvent);
            await StationsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.StationID = Guid.Empty;
                DataSource.StationCode = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedStation = args.RowData;

            if (selectedStation != null)
            {
                DataSource.StationID = selectedStation.Id;
                DataSource.StationCode = selectedStation.Code;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);

                if (DataSource.PeriodID != null && DataSource.StationID != null)
                {
                    var instructionDataSource = (await MaintenanceInstructionsAppService.GetbyPeriodStationAsync(DataSource.StationID, DataSource.PeriodID)).Data;
                    var instructionGridLineList = instructionDataSource.SelectMaintenanceInstructionLines;

                    await GetProductsList();
                    await GetUnitSetsList();

                    foreach (var instructionline in instructionGridLineList)
                    {
                        SelectPlannedMaintenanceLinesDto plannedMaintenanceLine = new SelectPlannedMaintenanceLinesDto
                        {
                            Amount = instructionline.Amount,
                            InstructionDescription = instructionline.InstructionDescription,
                            LineNr = instructionline.LineNr,
                            PlannedMaintenanceID = DataSource.Id,
                            ProductCode = ProductsList.Where(t=>t.Id == instructionline.ProductID).Select(t=>t.Code).FirstOrDefault(),
                            ProductName = ProductsList.Where(t => t.Id == instructionline.ProductID).Select(t => t.Name).FirstOrDefault(),
                            ProductID = instructionline.ProductID,
                            UnitSetCode = UnitSetsList.Where(t => t.Id == instructionline.UnitSetID).Select(t => t.Code).FirstOrDefault(),
                            UnitSetID = instructionline.UnitSetID
                        };

                        GridLineList.Add(plannedMaintenanceLine);
                        await _LineGrid.Refresh();
                    }

                }
            }
        }

        #endregion

        #region Bakım Periyodu ButtonEdit 

        SfTextBox MaintenancePeriodsButtonEdit;
        bool SelectMaintenancePeriodsPopupVisible = false;
        List<ListMaintenancePeriodsDto> MaintenancePeriodsList = new List<ListMaintenancePeriodsDto>();
        public async Task MaintenancePeriodsOnCreateIcon()
        {
            var MaintenancePeriodsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, MaintenancePeriodsButtonClickEvent);
            await MaintenancePeriodsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", MaintenancePeriodsButtonClick } });
        }

        public async void MaintenancePeriodsButtonClickEvent()
        {
            SelectMaintenancePeriodsPopupVisible = true;
            await GetMaintenancePeriodsList();
            await InvokeAsync(StateHasChanged);
        }

        public void MaintenancePeriodsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PeriodID = Guid.Empty;
                DataSource.PeriodName = string.Empty;
            }
        }

        public async void MaintenancePeriodsDoubleClickHandler(RecordDoubleClickEventArgs<ListMaintenancePeriodsDto> args)
        {
            var selectedMaintenancePeriod = args.RowData;

            if (selectedMaintenancePeriod != null)
            {
                DataSource.PeriodID = selectedMaintenancePeriod.Id;
                DataSource.PeriodName = selectedMaintenancePeriod.Name;
                DataSource.PeriodTime = selectedMaintenancePeriod.PeriodTime;
                SelectMaintenancePeriodsPopupVisible = false;
                await InvokeAsync(StateHasChanged);

                if (DataSource.PeriodID != null && DataSource.StationID != null)
                {
                    var instructionDataSource = (await MaintenanceInstructionsAppService.GetbyPeriodStationAsync(DataSource.StationID, DataSource.PeriodID)).Data;
                    var instructionGridLineList = instructionDataSource.SelectMaintenanceInstructionLines;

                    await GetProductsList();
                    await GetUnitSetsList();

                    foreach (var instructionline in instructionGridLineList)
                    {
                        SelectPlannedMaintenanceLinesDto plannedMaintenanceLine = new SelectPlannedMaintenanceLinesDto
                        {
                            Amount = instructionline.Amount,
                            InstructionDescription = instructionline.InstructionDescription,
                            LineNr = instructionline.LineNr,
                            PlannedMaintenanceID = DataSource.Id,
                            ProductCode = ProductsList.Where(t => t.Id == instructionline.ProductID).Select(t => t.Code).FirstOrDefault(),
                            ProductName = ProductsList.Where(t => t.Id == instructionline.ProductID).Select(t => t.Name).FirstOrDefault(),
                            ProductID = instructionline.ProductID,
                            UnitSetCode = UnitSetsList.Where(t => t.Id == instructionline.UnitSetID).Select(t => t.Code).FirstOrDefault(),
                            UnitSetID = instructionline.UnitSetID
                        };

                        GridLineList.Add(plannedMaintenanceLine);
                        await _LineGrid.Refresh();

                    }

                }
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetMaintenancePeriodsList()
        {
            MaintenancePeriodsList = (await MaintenancePeriodsAppService.GetListAsync(new ListMaintenancePeriodsParameterDto())).Data.ToList();
        }

        #endregion

        #region Stok Kartı ve Birim Seti ButtonEdit Kodları

        //#region Stok Kartı ButtonEdit 

        //SfTextBox ProductsButtonEdit;
        //bool SelectproductsPopupVisible = false;
        //List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        //public async Task ProductsOnCreateIcon()
        //{
        //    var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsButtonClickEvent);
        //    await ProductsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        //}

        //public async void ProductsButtonClickEvent()
        //{
        //    SelectproductsPopupVisible = true;
        //    await GetProductsList();
        //    await InvokeAsync(StateHasChanged);
        //}

        //public void ProductsOnValueChange(ChangedEventArgs args)
        //{
        //    if (args.Value == null)
        //    {
        //        LineDataSource.ProductID = Guid.Empty;
        //        LineDataSource.ProductCode = string.Empty;
        //        LineDataSource.ProductName = string.Empty;
        //    }
        //}

        //public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        //{
        //    var selectedProduct = args.RowData;

        //    if (selectedProduct != null)
        //    {
        //        LineDataSource.ProductID = selectedProduct.Id;
        //        LineDataSource.ProductCode = selectedProduct.Code;
        //        LineDataSource.ProductName = selectedProduct.Name;
        //        SelectproductsPopupVisible = false;
        //        await InvokeAsync(StateHasChanged);
        //    }
        //}

        //#endregion

        //#region Birim Seti ButtonEdit 

        //SfTextBox UnitSetsButtonEdit;
        //bool SelectUnitSetsPopupVisible = false;
        //List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();
        //public async Task UnitSetsOnCreateIcon()
        //{
        //    var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
        //    await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        //}

        //public async void UnitSetsButtonClickEvent()
        //{
        //    SelectUnitSetsPopupVisible = true;
        //    await GetUnitSetsList();
        //    await InvokeAsync(StateHasChanged);
        //}

        //public void UnitSetsOnValueChange(ChangedEventArgs args)
        //{
        //    if (args.Value == null)
        //    {
        //        LineDataSource.UnitSetID = Guid.Empty;
        //        LineDataSource.UnitSetCode = string.Empty;
        //    }
        //}

        //public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        //{
        //    var selectedUnitSet = args.RowData;

        //    if (selectedUnitSet != null)
        //    {
        //        LineDataSource.UnitSetID = selectedUnitSet.Id;
        //        LineDataSource.UnitSetCode = selectedUnitSet.Code;
        //        SelectUnitSetsPopupVisible = false;
        //        await InvokeAsync(StateHasChanged);
        //    }
        //}

        //#endregion

        #endregion
    }
}
