using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionTracking
{
    public partial class ProductionTrackingsListPage
    {
        private SfGrid<SelectProductionTrackingHaltLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectProductionTrackingHaltLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectProductionTrackingHaltLinesDto> GridLineList = new List<SelectProductionTrackingHaltLinesDto>();

        private bool LineCrudPopup = false;

        private DateTime? _date = DateTime.Today;

        private SfDatePicker<DateTime?> _endDatePicker;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ProductionTrackingsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Vardiya ButtonEdit

        SfTextBox ShiftsButtonEdit;
        bool SelectShiftsPopupVisible = false;
        List<ListShiftsDto> ShiftsList = new List<ListShiftsDto>();

        public async Task ShiftsCodeOnCreateIcon()
        {
            var ShiftsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ShiftsButtonClickEvent);
            await ShiftsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ShiftsCodeButtonClick } });
        }

        public async void ShiftsButtonClickEvent()
        {
            SelectShiftsPopupVisible = true;
            await GetShiftsList();
            await InvokeAsync(StateHasChanged);
        }


        public void ShiftsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ShiftID = Guid.Empty;
                DataSource.ShiftCode = string.Empty;
            }
        }

        public async void ShiftsDoubleClickHandler(RecordDoubleClickEventArgs<ListShiftsDto> args)
        {
            var selectedShift = args.RowData;

            if (selectedShift != null)
            {
                DataSource.ShiftID = selectedShift.Id;
                DataSource.ShiftCode = selectedShift.Code;
                SelectShiftsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsButtonClickEvent);
            await StationsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsCodeButtonClick } });
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
            }
        }
        #endregion

        #region Operatör ButtonEdit

        SfTextBox EmployeesButtonEdit;
        bool SelectEmployeesPopupVisible = false;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        public async Task EmployeesCodeOnCreateIcon()
        {
            var EmployeesCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeesButtonClickEvent);
            await EmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeesCodeButtonClick } });
        }

        public async void EmployeesButtonClickEvent()
        {
            SelectEmployeesPopupVisible = true;
            await GetEmployeesList();
            await InvokeAsync(StateHasChanged);
        }


        public void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.EmployeeID = Guid.Empty;
                DataSource.EmployeeName = string.Empty;
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                DataSource.EmployeeID = selectedEmployee.Id;
                DataSource.EmployeeName = selectedEmployee.Name;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İş Emri ButtonEdit

        SfTextBox WorkOrdersButtonEdit;
        bool SelectWorkOrdersPopupVisible = false;
        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        public async Task WorkOrdersCodeOnCreateIcon()
        {
            var WorkOrdersCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WorkOrdersButtonClickEvent);
            await WorkOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WorkOrdersCodeButtonClick } });
        }

        public async void WorkOrdersButtonClickEvent()
        {
            SelectWorkOrdersPopupVisible = true;
            await GetWorkOrdersList();
            await InvokeAsync(StateHasChanged);
        }


        public void WorkOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkOrderID = Guid.Empty;
                DataSource.WorkOrderCode = string.Empty;
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var selectedWorkOrder = args.RowData;

            if (selectedWorkOrder != null)
            {
                DataSource.WorkOrderID = selectedWorkOrder.Id;
                DataSource.WorkOrderCode = selectedWorkOrder.Code;
                SelectWorkOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Duruş ButtonEdit

        SfTextBox HaltReasonsButtonEdit;
        bool SelectHaltReasonsPopupVisible = false;
        List<ListHaltReasonsDto> HaltReasonsList = new List<ListHaltReasonsDto>();

        public async Task HaltReasonsCodeOnCreateIcon()
        {
            var HaltReasonsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, HaltReasonsButtonClickEvent);
            await HaltReasonsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", HaltReasonsCodeButtonClick } });
        }

        public async void HaltReasonsButtonClickEvent()
        {
            SelectHaltReasonsPopupVisible = true;
            await GetHaltReasonsList();
            await InvokeAsync(StateHasChanged);
        }


        public void HaltReasonsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.HaltID   = Guid.Empty;
                LineDataSource.HaltName = string.Empty;
                LineDataSource.HaltCode = string.Empty;
            }
        }

        public async void HaltReasonsDoubleClickHandler(RecordDoubleClickEventArgs<ListHaltReasonsDto> args)
        {
            var selectedHaltReason = args.RowData;

            if (selectedHaltReason != null)
            {
                LineDataSource.HaltID   = selectedHaltReason.Id;
                LineDataSource.HaltName = selectedHaltReason.Name;
                LineDataSource.HaltCode = selectedHaltReason.Code;
                SelectHaltReasonsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Fason Üretim Takip Satırları İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionTrackingsDto() { };

            DataSource.OperationStartDate = _date;
            DataSource.OperationEndDate = null;
            DataSource.SelectProductionTrackingHaltLines = new List<SelectProductionTrackingHaltLinesDto>();
            GridLineList = DataSource.SelectProductionTrackingHaltLines;

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductionTrackingsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await ProductionTrackingsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectProductionTrackingHaltLines;


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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectProductionTrackingHaltLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectProductionTrackingHaltLinesDto();
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectProductionTrackingHaltLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectProductionTrackingHaltLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectProductionTrackingHaltLines.Remove(line);
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
            SelectProductionTrackingsDto result;

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.SelectProductionTrackingHaltLines = GridLineList;
                var createInput = ObjectMapper.Map<SelectProductionTrackingsDto, CreateProductionTrackingsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                DataSource.SelectProductionTrackingHaltLines = GridLineList;

                var updateInput = ObjectMapper.Map<SelectProductionTrackingsDto, UpdateProductionTrackingsDto>(DataSource);

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

        public void OnDateFocus()
        {
            if (DataSource.OperationStartDate == DateTime.MinValue || DataSource.OperationStartDate == null)
            {
                _endDatePicker.Enabled = false;
                ModalManager.WarningPopupAsync(L["UIConfirmationModalTitleBase"], L["UIWarningPopupMessageBase"]);
            }
        }
        public void OnDateChange()
        {
            _endDatePicker.Enabled = true;
        }

        protected async Task OnLineSubmit()
        {
            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectProductionTrackingHaltLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectProductionTrackingHaltLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectProductionTrackingHaltLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectProductionTrackingHaltLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectProductionTrackingHaltLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectProductionTrackingHaltLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectProductionTrackingHaltLines;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region GetList Metotları

        private async Task GetShiftsList()
        {
            ShiftsList = (await ShiftsAppService.GetListAsync(new ListShiftsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
        }

        private async Task GetEmployeesList()
        {
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        private async Task GetHaltReasonsList()
        {
            HaltReasonsList = (await HaltsAppService.GetListAsync(new ListHaltReasonsParameterDto())).Data.ToList();
        }

        #endregion
    }
}
