using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
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

namespace TsiErp.ErpUI.Pages.ProductionTrackingIoT
{



    public partial class ProductionTrackingsIoTListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListWorkOrdersDto> WorkOrdersComboBox;
        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        SfComboBox<Guid, ListEmployeesDto> EmployeesComboBox;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        SfComboBox<string, ListStationsDto> StationsComboBox;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        SfComboBox<string, ListShiftsDto> ShiftsComboBox;
        List<ListShiftsDto> ShiftsList = new List<ListShiftsDto>();

        SfComboBox<string, ListHaltReasonsDto> HaltsComboBox;
        List<ListHaltReasonsDto> HaltsList = new List<ListHaltReasonsDto>();

        #endregion

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

            await GetWorkOrdersList();
            await GetStationsList();
            await GetShiftsList();
            await GetHaltsList();
            await GetEmployeesList();
        }

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
                //MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                //MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                //MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
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
                    await ModalManager.MessagePopupAsync("Bilgi", "Seçtiğiniz kayıt ..... tarafından kullanılmaktadır.");
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

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

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
                ModalManager.WarningPopupAsync("Dikkat!", "Lütfen önce başlangıç tarihini seçiniz.");
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

        #region İş Emri
        public async Task WorkOrderFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await WorkOrdersComboBox.FilterAsync(WorkOrdersList, query);
        }

        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
        }

        public async Task WorkOrderValueChangeHandler(ChangeEventArgs<string, ListWorkOrdersDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.WorkOrderID = args.ItemData.Id;
                DataSource.WorkOrderCode = args.ItemData.Code;
            }
            else
            {
                DataSource.WorkOrderID = Guid.Empty;
                DataSource.WorkOrderCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Operatör
        public async Task EmployeeFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await EmployeesComboBox.FilterAsync(EmployeesList, query);
        }

        private async Task GetEmployeesList()
        {
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        public async Task EmployeeValueChangeHandler(ChangeEventArgs<Guid, ListEmployeesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.EmployeeID = args.ItemData.Id;
                DataSource.EmployeeName = args.ItemData.Name;
            }
            else
            {
                DataSource.EmployeeID = Guid.Empty;
                DataSource.EmployeeName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Şubeler
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

        #region Duruş
        public async Task HaltFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await HaltsComboBox.FilterAsync(HaltsList, query);
        }

        private async Task GetHaltsList()
        {
            HaltsList = (await HaltsAppService.GetListAsync(new ListHaltReasonsParameterDto())).Data.ToList();
        }

        public async Task HaltValueChangeHandler(ChangeEventArgs<string, ListHaltReasonsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.HaltID = args.ItemData.Id;
                LineDataSource.HaltCode = args.ItemData.Code;

            }
            else
            {
                LineDataSource.HaltID = Guid.Empty;
                LineDataSource.HaltCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Vardiya
        public async Task ShiftFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ShiftsComboBox.FilterAsync(ShiftsList, query);
        }

        private async Task GetShiftsList()
        {
            ShiftsList = (await ShiftsAppService.GetListAsync(new ListShiftsParameterDto())).Data.ToList();
        }

        public async Task ShiftValueChangeHandler(ChangeEventArgs<string, ListShiftsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ShiftID = args.ItemData.Id;
                DataSource.ShiftCode = args.ItemData.Code;

            }
            else
            {
                DataSource.ShiftID = Guid.Empty;
                DataSource.ShiftCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
