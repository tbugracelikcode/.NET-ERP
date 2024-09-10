using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ContractProductionTracking
{
    public partial class ContractProductionTrackingsListPage : IDisposable
    {


        [Inject]
        ModalManager ModalManager { get; set; }
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        private DateTime? _date = DateTime.Today;

        private SfDatePicker<DateTime?> _endDatePicker;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ContractProductionTrackingsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ContractProdTrackingsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();

        }

        #region Fason Üretim Takip Satır İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectContractProductionTrackingsDto() 
            { 
                Code = FicheNumbersAppService.GetFicheNumberAsync("ContractProdTrackingsChildMenu")
            };

            DataSource.OperationStartDate = _date;
            DataSource.OperationEndDate = null;

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
                            case "ContractProductionTrackingContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractProductionTrackingContextAdd"], Id = "new" }); break;
                            case "ContractProductionTrackingContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractProductionTrackingContextChange"], Id = "changed" }); break;
                            case "ContractProductionTrackingContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractProductionTrackingContextDelete"], Id = "delete" }); break;
                            case "ContractProductionTrackingContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractProductionTrackingContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListContractProductionTrackingsDto> args)
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
                    DataSource = (await ContractProductionTrackingsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
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
        }
        
        public async void OnDateFocus()
        {
            if (DataSource.OperationStartDate == DateTime.MinValue || DataSource.OperationStartDate == null)
            {
                _endDatePicker.Enabled = false;
                await ModalManager.WarningPopupAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupDateBase"]);
            }
        }
        public void OnDateChange()
        {
            _endDatePicker.Enabled = true;
        }

        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsButtonClickEvent);
            await CurrentAccountCardsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsButtonClickEvent()
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
                DataSource.CurrentAccountCardCode = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedCurrentAccountCard = args.RowData;

            if (selectedCurrentAccountCard != null)
            {
                DataSource.CurrentAccountCardID = selectedCurrentAccountCard.Id;
                DataSource.CurrentAccountCardCode = selectedCurrentAccountCard.Code;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
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
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                DataSource.ProductID = selectedProduct.Id;
                DataSource.ProductCode = selectedProduct.Code;
                DataSource.ProductName = selectedProduct.Name;
                SelectProductsPopupVisible = false;
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

        #region GetList Metotları

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetShiftsList()
        {
            ShiftsList = (await ShiftsAppService.GetListAsync(new ListShiftsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetEmployeesList()
        {
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeAnnualSeniorityDifferencesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
