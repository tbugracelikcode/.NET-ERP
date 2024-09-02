using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionTrackingIoT
{



    public partial class ProductionTrackingsIoTListPage : IDisposable
    {


        [Inject]
        ModalManager ModalManager { get; set; }
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private bool LineCrudPopup = false;

        private DateTime? _date = DateTime.Today;

        private SfDatePicker<DateTime?> _endDatePicker;
        public bool HaltReasonEnable = false;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ProductionTrackingsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "IoTProdTrackingsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            //CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionTrackingsDto() { };

            DataSource.OperationStartDate = _date;
            DataSource.OperationEndDate = null;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ProductionTrackingLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextAdd"], Id = "new" }); break;
                            case "ProductionTrackingLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextChange"], Id = "changed" }); break;
                            case "ProductionTrackingLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextDelete"], Id = "delete" }); break;
                            case "ProductionTrackingLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        //protected void CreateMainContextMenuItems()
        //{
        //    if (GridContextMenu.Count == 0)
        //    {

        //        foreach (var context in contextsList)
        //        {
        //            var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
        //            if (permission)
        //            {
        //                switch (context.MenuName)
        //                {
        //                    case "ProductionTrackingContextAdd":
        //                        MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextAdd"], Id = "new" }); break;
        //                    case "ProductionTrackingContextChange":
        //                        MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextChange"], Id = "changed" }); break;
        //                    case "ProductionTrackingContextDelete":
        //                        MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextDelete"], Id = "delete" }); break;
        //                    case "ProductionTrackingContextRefresh":
        //                        MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionTrackingContextRefresh"], Id = "refresh" }); break;
        //                    default: break;
        //                }
        //            }
        //        }
        //    }
        //}

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
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        //public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductionTrackingsDto> args)
        //{
        //    switch (args.Item.Id)
        //    {
        //        case "new":
        //            await BeforeInsertAsync();
        //            break;

        //        case "changed":
        //            IsChanged = true;
        //            DataSource = (await ProductionTrackingsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
        //            GridLineList = DataSource.SelectProductionTrackingHaltLines;


        //            ShowEditPage();
        //            await InvokeAsync(StateHasChanged);
        //            break;

        //        case "delete":
        //            var res = await ModalManager.ConfirmationAsync(L["UIConfirmationModalTitleBase"], L["UIConfirmationModalMessageBase"]);
        //            if (res == true)
        //            {
        //                await DeleteAsync(args.RowInfo.RowData.Id);
        //                await GetListDataSourceAsync();
        //                await _grid.Refresh();
        //                await InvokeAsync(StateHasChanged);
        //            }
        //            break;

        //        case "refresh":
        //            await GetListDataSourceAsync();
        //            await _grid.Refresh();
        //            await InvokeAsync(StateHasChanged);
        //            break;

        //        default:
        //            break;
        //    }
        //}


        protected override async Task OnSubmit()
        {
            SelectProductionTrackingsDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectProductionTrackingsDto, CreateProductionTrackingsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {

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
            _endDatePicker.Enabled = true;
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
                DataSource.WorkOrderCode = selectedWorkOrder.WorkOrderNo;
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

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
