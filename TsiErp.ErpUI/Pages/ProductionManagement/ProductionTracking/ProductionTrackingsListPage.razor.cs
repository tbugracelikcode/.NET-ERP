﻿using DevExpress.DataProcessing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;


namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionTracking
{
    public partial class ProductionTrackingsListPage : IDisposable
    {

        [Inject]
        ModalManager ModalManager { get; set; }
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        private bool LineCrudPopup = false;

        private SfDatePicker<DateTime?> _endDatePicker;

        List<SelectProductsOperationLinesDto> OperationLineList = new List<SelectProductsOperationLinesDto>();
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        SelectShiftsDto ShiftDataSource = new SelectShiftsDto();

        public bool HaltReasonEnable = false;
        public bool StartTimeEnable = false;
        public bool EndTimeEnable = false;
        DateTime MinStartTime = DateTime.Now;
        DateTime MaxEndTime = DateTime.Now;


        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ProductionTrackingsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProdTrackingsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();

        }

        #region Tür Enum Combobox

        public IEnumerable<SelectProductionTrackingsDto> types = GetEnumDisplayTypesNames<ProductionTrackingTypesEnum>();

        public static List<SelectProductionTrackingsDto> GetEnumDisplayTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<ProductionTrackingTypesEnum>()
                       .Select(x => new SelectProductionTrackingsDto
                       {
                           ProductionTrackingTypes = x,
                           ProductionTrackingTypesName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private void ProductionTrackingTypeValueChangeHandler(ChangeEventArgs<ProductionTrackingTypesEnum, SelectProductionTrackingsDto> args)
        {
            if (args.ItemData.ProductionTrackingTypes == ProductionTrackingTypesEnum.Durus)
            {
                HaltReasonEnable = true;
            }
            else
            {
                HaltReasonEnable = false;
            }
        }


        #endregion

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
                //DataSource.HaltTime = 0;
                //DataSource.OperationTime = 0;
                StartTimeEnable = false;
                EndTimeEnable = false;
                MinStartTime = DateTime.Now;
                MaxEndTime = DateTime.Now;
                ShiftDataSource = new SelectShiftsDto();
            }
        }

        public async void ShiftsDoubleClickHandler(RecordDoubleClickEventArgs<ListShiftsDto> args)
        {
            var selectedShift = args.RowData;

            if (selectedShift != null)
            {
                DataSource.ShiftID = selectedShift.Id;
                DataSource.ShiftCode = selectedShift.Code;
                //DataSource.HaltTime = selectedShift.TotalBreakTime;
                //DataSource.OperationTime = selectedShift.TotalWorkTime;
                ShiftDataSource = (await ShiftsAppService.GetAsync(selectedShift.Id)).Data;

                if (ShiftDataSource != null && ShiftDataSource.Id != Guid.Empty)
                {
                    var today = GetSQLDateAppService.GetDateFromSQL().Date;

                    MinStartTime = today + ShiftDataSource.SelectShiftLinesDto.Min(t => t.StartHour).GetValueOrDefault();
                    MaxEndTime = today + ShiftDataSource.SelectShiftLinesDto.Max(t => t.EndHour).GetValueOrDefault();
                }

                StartTimeEnable = true;
                EndTimeEnable = true;
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
                DataSource.EmployeeSurname = string.Empty;
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                DataSource.EmployeeID = selectedEmployee.Id;
                DataSource.EmployeeName = selectedEmployee.Name;
                DataSource.EmployeeSurname = selectedEmployee.Surname;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
            DataSource.EmployeeName = DataSource.EmployeeName + " " + DataSource.EmployeeSurname;
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
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductsOperationID = Guid.Empty;
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
                        DataSource.ProductID = selectedWorkOrder.ProductID.GetValueOrDefault();
                        DataSource.ProductionOrderID = selectedWorkOrder.ProductionOrderID.GetValueOrDefault();
                        DataSource.ProductsOperationID = selectedWorkOrder.ProductsOperationID.GetValueOrDefault();

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
                    DataSource.ProductID = selectedWorkOrder.ProductID.GetValueOrDefault();
                    DataSource.ProductionOrderID = selectedWorkOrder.ProductionOrderID.GetValueOrDefault();
                    DataSource.ProductsOperationID = selectedWorkOrder.ProductsOperationID.GetValueOrDefault();

                    OperationLineList = (await ProductsOperationsAppService.GetAsync(selectedWorkOrder.ProductsOperationID.GetValueOrDefault())).Data.SelectProductsOperationLines.ToList();

                    DataSource.CustomerCode = (await CurrentAccountCardsAppService.GetAsync(DataSource.CurrentAccountCardID.GetValueOrDefault())).Data.CustomerCode;

                    DataSource.WorkOrderCode = selectedWorkOrder.WorkOrderNo;
                    SelectWorkOrdersPopupVisible = false;


                    await InvokeAsync(StateHasChanged);
                }

            }
            await InvokeAsync(StateHasChanged);
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
                DataSource.HaltReasonID = Guid.Empty;
                DataSource.HaltReasonCode = string.Empty;
            }
        }

        public async void HaltReasonsDoubleClickHandler(RecordDoubleClickEventArgs<ListHaltReasonsDto> args)
        {
            var selectedHaltReason = args.RowData;

            if (selectedHaltReason != null)
            {
                DataSource.HaltReasonID = selectedHaltReason.Id;
                DataSource.HaltReasonCode = selectedHaltReason.Code;
                SelectHaltReasonsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCustomerCodeButtonEdit = new();
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
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                ProductionTrackingTypes = ProductionTrackingTypesEnum.Operasyonda,
            };

            StartTimeEnable = false;
            EndTimeEnable = false;
            MinStartTime = DateTime.Now;
            MaxEndTime = DateTime.Now;
            ShiftDataSource = new SelectShiftsDto();

            DataSource.OperationStartDate = GetSQLDateAppService.GetDateFromSQL().Date;
            DataSource.OperationEndDate = GetSQLDateAppService.GetDateFromSQL().Date;

            HaltReasonEnable = false;

            foreach (var item in types)
            {
                item.ProductionTrackingTypesName = L[item.ProductionTrackingTypesName];
            }

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ProductionTrackingContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextAdd"], Id = "new" }); break;
                            case "ProductionTrackingContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextChange"], Id = "changed" }); break;
                            case "ProductionTrackingContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextDelete"], Id = "delete" }); break;
                            case "ProductionTrackingContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;

                    foreach (var item in types)
                    {
                        item.ProductionTrackingTypesName = L[item.ProductionTrackingTypesName];
                    }

                    if (DataSource.ProductionTrackingTypes == ProductionTrackingTypesEnum.Durus)
                    {
                        HaltReasonEnable = true;
                    }
                    else
                    {
                        HaltReasonEnable = false;
                    }

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
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                        DataSource = (await ProductionTrackingsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;


                        ShowEditPage();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalMessageBase"]);
                        if (res == true)
                        {
                            await DeleteAsync(args.RowInfo.RowData.Id);
                            await GetListDataSourceAsync();
                            await _grid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
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

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void OnDateFocus()
        {
            if (DataSource.OperationStartDate == DateTime.MinValue || DataSource.OperationStartDate == null)
            {
                _endDatePicker.Enabled = false;
                await ModalManager.WarningPopupAsync(L["UIConfirmationModalTitleBase"], L["UIWarningPopupMessageBase"]);
            }
        }
        public void OnDateChange()
        {
            DataSource.OperationEndDate = DataSource.OperationStartDate;
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
            #region Validations

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

            if (DataSource.EmployeeID == Guid.Empty && DataSource.ProductionTrackingTypes != ProductionTrackingTypesEnum.Durus)
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

            #endregion

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

                DataSource.OperationEndDate = DataSource.OperationStartDate;

                #region Toplam Süreyi Set Etme

                var notWorkHoursList = ShiftDataSource.SelectShiftLinesDto.Where(t=>t.StartHour >= DataSource.OperationStartTime.GetValueOrDefault() && t.EndHour <= DataSource.OperationEndTime.GetValueOrDefault() && t.Type == ShiftLinesTypeEnum.Mola).ToList();

                decimal subtractedTime = 0;

                if(notWorkHoursList != null && notWorkHoursList.Count > 0)
                {
                    foreach (var notWorkHours in notWorkHoursList)
                    {
                        subtractedTime = subtractedTime + Convert.ToDecimal(notWorkHours.EndHour.Value.Subtract(notWorkHours.StartHour.Value).TotalSeconds);
                    }
                }

                if(DataSource.ProductionTrackingTypes == ProductionTrackingTypesEnum.Operasyonda)
                {
                    DataSource.OperationTime = Convert.ToDecimal(DataSource.OperationEndTime.Value.Subtract(DataSource.OperationStartTime.Value).TotalSeconds) - subtractedTime;

                    DataSource.HaltTime = 0;
                    DataSource.AdjustmentTime = 0;
                }
                else if (DataSource.ProductionTrackingTypes == ProductionTrackingTypesEnum.Durus)
                {
                    //DataSource.HaltTime = Convert.ToDecimal(DataSource.OperationEndTime.Value.Subtract(DataSource.OperationStartTime.Value).TotalSeconds) - subtractedTime;
                    DataSource.HaltTime = Convert.ToDecimal(DataSource.OperationEndTime.Value.Subtract(DataSource.OperationStartTime.Value).TotalSeconds);

                    DataSource.OperationTime = 0;
                    DataSource.AdjustmentTime = 0;
                }
                else if (DataSource.ProductionTrackingTypes == ProductionTrackingTypesEnum.Ayar)
                {
                    DataSource.AdjustmentTime = Convert.ToDecimal(DataSource.OperationEndTime.Value.Subtract(DataSource.OperationStartTime.Value).TotalSeconds) - subtractedTime;

                    DataSource.OperationTime = 0;
                    DataSource.HaltTime = 0;
                }

                #endregion

                await base.OnSubmit();
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
