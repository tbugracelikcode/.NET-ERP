using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MaintenanceManagement.MaintenanceInstruction
{
    public partial class MaintenanceInstructionsListPage
    {
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
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

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
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                LineDataSource.ProductID = selectedProduct.Id;
                LineDataSource.ProductCode = selectedProduct.Code;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Bakım Periyodu Button Edit

        SfTextBox MaintenancePeriodButtonEdit;
        bool SelectMaintenancePeriodPopupVisible = false;
        List<ListMaintenancePeriodsDto> MaintenancePeriodsList = new List<ListMaintenancePeriodsDto>();
        public async Task MaintenancePeriodOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, MaintenancePeriodButtonClickEvent);
            await MaintenancePeriodButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void MaintenancePeriodButtonClickEvent()
        {
            SelectMaintenancePeriodPopupVisible = true;
            await GetMaintenancePeriodsList();
            await InvokeAsync(StateHasChanged);
        }
       
        public void MaintenancePeriodsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PeriodID = Guid.Empty;
                DataSource.PeriodName = string.Empty;
                DataSource.PeriodTime = 0;
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
                SelectMaintenancePeriodPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Birim Seti Button Edit

        SfTextBox UnitSetButtonEdit;
        bool SelectUnitSetPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();
        public async Task UnitSetOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetButtonClickEvent);
            await UnitSetButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void UnitSetButtonClickEvent()
        {
            SelectUnitSetPopupVisible = true;
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
                LineDataSource.UnitSetCode = selectedUnitSet.Code;
                SelectUnitSetPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İş İstasyonu Button Edit

        SfTextBox StationButtonEdit;
        bool SelectStationPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();
        public async Task StationOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationButtonClickEvent);
            await StationButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void StationButtonClickEvent()
        {
            SelectStationPopupVisible = true;
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
                SelectStationPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Bakım Talimatları Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenanceInstructionsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("MainInstructionsChildMenu")
            };

            DataSource.SelectMaintenanceInstructionLines = new List<SelectMaintenanceInstructionLinesDto>();
            GridLineList = DataSource.SelectMaintenanceInstructionLines;

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceInstructionLineContextAdd"]    , Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceInstructionLineContextChange"] , Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceInstructionLineContextDelete"] , Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceInstructionLineContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text =  L["MaintenanceInstructionContextAdd"]    , Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text =  L["MaintenanceInstructionContextChange"] , Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text =  L["MaintenanceInstructionContextDelete"] , Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenanceInstructionContextRefresh"], Id = "refresh" });
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMaintenanceInstructionsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await MaintenanceInstructionsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMaintenanceInstructionLines;


                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalMessageBase"]);
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

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalLineMessageBase"]);

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
            if(LineDataSource.Amount == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase"]);
            }
            else
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
           
        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetMaintenancePeriodsList()
        {
            MaintenancePeriodsList = (await MaintenancePeriodsAppService.GetListAsync(new ListMaintenancePeriodsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MainInstructionsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
