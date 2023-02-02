using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.HaltReason.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.Business.Extensions.ObjectMapping;
using Syncfusion.Blazor.HeatMap.Internal;
using Syncfusion.Blazor.Calendars;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Business.Entities.CurrentAccountCard.Services;

namespace TsiErp.ErpUI.Pages.ContractProductionTracking
{
    public partial class ContractProductionTrackingsListPage
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

        SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        #endregion

        private SfGrid<ListContractProductionTrackingsDto> _grid;

        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        private DateTime? _date = DateTime.Today;

        private SfDatePicker<DateTime?> _endDatePicker;

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ContractProductionTrackingsAppService;
            CreateMainContextMenuItems();

            await GetWorkOrdersList();
            await GetStationsList();
            await GetShiftsList();
            await GetEmployeesList();
            await GetCurrentAccountCardsList();
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectContractProductionTrackingsDto() { };

            DataSource.OperationStartDate = _date;
            DataSource.OperationEndDate = null;

            ShowEditPage();


            await Task.CompletedTask;
        }


        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
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
                    DataSource = (await ContractProductionTrackingsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
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

        #region Cari
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

        public async Task CurrentAccountCardValueChangeHandler(ChangeEventArgs<string, ListCurrentAccountCardsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.CurrentAccountCardID = args.ItemData.Id;
                DataSource.CurrentAccountCardCode = args.ItemData.Code;

            }
            else
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
