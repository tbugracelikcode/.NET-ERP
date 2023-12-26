using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionTracking
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

        List<SelectProductsOperationLinesDto> OperationLineList = new List<SelectProductsOperationLinesDto>();


        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ProductionTrackingsAppService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region ButtonEdit Metotları

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
                DataSource.HaltTime = 0;
                DataSource.OperationTime = 0;
            }
        }

        public async void ShiftsDoubleClickHandler(RecordDoubleClickEventArgs<ListShiftsDto> args)
        {
            var selectedShift = args.RowData;

            if (selectedShift != null)
            {
                DataSource.ShiftID = selectedShift.Id;
                DataSource.ShiftCode = selectedShift.Code;
                DataSource.HaltTime = selectedShift.TotalBreakTime;
                DataSource.OperationTime = selectedShift.TotalWorkTime;
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
            if (DataSource.WorkOrderID == Guid.Empty)
            {

                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["UIWarningWorkOrderMessage"]);
            }
            else
            {
                SelectStationsPopupVisible = true;
                await GetStationsList();
            }
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
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CustomerCode = string.Empty;
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var selectedWorkOrder = args.RowData;

            if (selectedWorkOrder != null)
            {

                if (selectedWorkOrder.LineNr > 1)
                {

                    var previousWorkOrder = WorkOrdersList.FirstOrDefault(t => t.ProductionOrderID == selectedWorkOrder.ProductionOrderID && t.LineNr == selectedWorkOrder.LineNr - 1);

                    if (previousWorkOrder != null && previousWorkOrder.ProducedQuantity > 0)
                    {
                        DataSource.WorkOrderID = selectedWorkOrder.Id;
                        DataSource.CurrentAccountCardID = selectedWorkOrder.CurrentAccountCardID;
                        DataSource.PlannedQuantity = selectedWorkOrder.PlannedQuantity;

                        OperationLineList = (await ProductsOperationsAppService.GetAsync(selectedWorkOrder.ProductsOperationID.GetValueOrDefault())).Data.SelectProductsOperationLines.ToList();

                        DataSource.CustomerCode = (await CurrentAccountCardsAppService.GetAsync(DataSource.CurrentAccountCardID.GetValueOrDefault())).Data.CustomerCode;

                        DataSource.WorkOrderCode = selectedWorkOrder.WorkOrderNo;
                        SelectWorkOrdersPopupVisible = false;
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["UIWarningPreviousWorkOrderMessage"]);
                    }

                    await InvokeAsync(StateHasChanged);
                }

                if (selectedWorkOrder.LineNr == 1)
                {
                    DataSource.WorkOrderID = selectedWorkOrder.Id;
                    DataSource.CurrentAccountCardID = selectedWorkOrder.CurrentAccountCardID;
                    DataSource.PlannedQuantity = selectedWorkOrder.PlannedQuantity;

                    OperationLineList = (await ProductsOperationsAppService.GetAsync(selectedWorkOrder.ProductsOperationID.GetValueOrDefault())).Data.SelectProductsOperationLines.ToList();

                    DataSource.CustomerCode = (await CurrentAccountCardsAppService.GetAsync(DataSource.CurrentAccountCardID.GetValueOrDefault())).Data.CustomerCode;

                    DataSource.WorkOrderCode = selectedWorkOrder.WorkOrderNo;
                    SelectWorkOrdersPopupVisible = false;


                    await InvokeAsync(StateHasChanged);
                }

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
                LineDataSource.HaltID = Guid.Empty;
                LineDataSource.HaltName = string.Empty;
                LineDataSource.HaltCode = string.Empty;
            }
        }

        public async void HaltReasonsDoubleClickHandler(RecordDoubleClickEventArgs<ListHaltReasonsDto> args)
        {
            var selectedHaltReason = args.RowData;

            if (selectedHaltReason != null)
            {
                LineDataSource.HaltID = selectedHaltReason.Id;
                LineDataSource.HaltName = selectedHaltReason.Name;
                LineDataSource.HaltCode = selectedHaltReason.Code;
                SelectHaltReasonsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCustomerCodeButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();



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



        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CustomerCode = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrentAccountCardID = selectedUnitSet.Id;
                DataSource.CustomerCode = selectedUnitSet.CustomerCode;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #endregion

        #region Fason Üretim Takip Satırları İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionTrackingsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu")
            };

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
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextRefresh"], Id = "refresh" });
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
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

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.Where(t => !string.IsNullOrEmpty(t.CustomerCode)).ToList();
        }

        private async Task GetStationsList()
        {
            var _stationlist = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();

            foreach (var line in OperationLineList)
            {
                var station = _stationlist.Where(t => t.Id == line.StationID).FirstOrDefault();

                if (!StationsList.Any(t => t.Id == station.Id))
                {
                    StationsList.Add(station);
                }
            }
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


        protected async override Task OnSubmit()
        {

            if (DataSource.Code.Length > 17)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorCodeMaxLength"]);
                return;
            }

            if (DataSource.WorkOrderID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorWordOrderID"]);
                return;
            }

            if (DataSource.StationID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorStationID"]);
                return;
            }

            if (DataSource.EmployeeID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorEmployeeID"]);
                return;
            }

            if (DataSource.CurrentAccountCardID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorCurrentCardID"]);
                return;
            }

            if (DataSource.ShiftID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorShiftID"]);
                return;
            }

            if (DataSource.ProducedQuantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["ValidatorProducedQuantity"]);
                return;
            }

            SelectProductionTrackingsDto entity = null;

            decimal controlProducedAmount = 0;

            if (DataSource.Id != Guid.Empty)
            {
                entity = (await ProductionTrackingsAppService.GetAsync(DataSource.Id)).Data;

                controlProducedAmount = Math.Abs(entity.ProducedQuantity - DataSource.ProducedQuantity);
            }

            controlProducedAmount = controlProducedAmount > 0 ? controlProducedAmount : DataSource.ProducedQuantity;

            var workOrder = (await WorkOrdersAppService.GetAsync(DataSource.WorkOrderID)).Data;

            if (workOrder != null)
            {
                if (workOrder.LineNr > 1)
                {
                    var previousWorkOrderId = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID && t.LineNr == workOrder.LineNr - 1).Select(t => t.Id).FirstOrDefault();

                    var previousWorkOrder = (await WorkOrdersAppService.GetAsync(previousWorkOrderId)).Data;

                    var previousOperationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(previousWorkOrder.ProductionOrderID.GetValueOrDefault(), previousWorkOrder.ProductsOperationID.GetValueOrDefault())).Data;

                    if (previousOperationStockMovement.Id != Guid.Empty)
                    {
                        if (previousOperationStockMovement.TotalAmount > 0)
                        {
                            if (DataSource.ProducedQuantity > 0)
                            {
                                if (previousOperationStockMovement.TotalAmount < controlProducedAmount)
                                {
                                    await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["UIWarningOprStockControlMessage"]);
                                    return;
                                }
                            }
                        }
                    }
                }

                if (DataSource.ProducedQuantity > DataSource.PlannedQuantity)
                {
                    await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitle"], L["UIWarningQuantityControlMessage"]);
                    return;
                }

                await base.OnSubmit();
            }

        }
    }
}
