using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.OperationUnsuitabilityReport
{
    public partial class OperationUnsuitabilityReportsListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        public class UnsComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<UnsComboBox> _unsComboBox = new List<UnsComboBox>
        {
            new UnsComboBox(){ID = "Scrap", Text="ComboboxScrap"},
            new UnsComboBox(){ID = "Correction", Text="ComboboxCorrection"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="ComboboxToBeUsedAs"}
        };

        public bool isCreatedNewWorkOrder = false;
        public string CreatedWorkOrderNo = string.Empty;
        public int comboIndex = 0;


        protected override async void OnInitialized()
        {
            BaseCrudService = OperationUnsuitabilityReportsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "OprUnsRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
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
                            case "OperationUnsuitabilityReportContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationUnsuitabilityReportContextAdd"], Id = "new" }); break;
                            case "OperationUnsuitabilityReportContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationUnsuitabilityReportContextChange"], Id = "changed" }); break;
                            case "OperationUnsuitabilityReportContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationUnsuitabilityReportContextDelete"], Id = "delete" }); break;
                            case "OperationUnsuitabilityReportContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["OperationUnsuitabilityReportContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationUnsuitabilityReportsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("OprUnsRecordsChildMenu")
            };


            comboIndex = 0;
            foreach (var item in _unsComboBox)
            {
                item.Text = L[item.Text];
            }

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        public override async void ShowEditPage()
        {
            foreach (var item in _unsComboBox)
            {
                item.Text = L[item.Text];
            }

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
                    #region Aksiyon Indexleme

                    if (DataSource.Action_ == L["ComboboxScrap"].Value) comboIndex = 0;
                    else if (DataSource.Action_ == L["ComboboxCorrection"].Value) comboIndex = 1;
                    else if (DataSource.Action_ == L["ComboboxToBeUsedAs"].Value) comboIndex = 2;

                    #endregion

                    EditPageVisible = true;

                    DataSource.EmployeeName = DataSource.EmployeeName + " " + DataSource.EmployeeSurname;

                    var createdWorkOrder = (await WorkOrdersAppService.GetAsync(DataSource.UnsuitabilityWorkOrderID.GetValueOrDefault())).Data;

                    if (createdWorkOrder != null && createdWorkOrder.Id != Guid.Empty)
                    {
                        isCreatedNewWorkOrder = true;
                        CreatedWorkOrderNo = createdWorkOrder.WorkOrderNo;
                    }
                    else
                    {
                        isCreatedNewWorkOrder = false;
                        CreatedWorkOrderNo = string.Empty;
                    }

                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private void UnsComboBoxValueChangeHandler(ChangeEventArgs<string, UnsComboBox> args)
        {
            switch (args.ItemData.ID)
            {
                case "Scrap":
                    DataSource.Action_ = L["ComboboxScrap"].Value;
                    comboIndex = 0;
                    break;

                case "Correction":
                    DataSource.Action_ = L["ComboboxCorrection"].Value;
                    comboIndex = 1;
                    break;

                case "ToBeUsedAs":
                    DataSource.Action_ = L["ComboboxToBeUsedAs"].Value;
                    comboIndex = 2;
                    break;

                default: break;
            }
        }



        #region İş Emri ButtonEdit

        SfTextBox WorkOrdersButtonEdit;
        bool SelectWorkOrdersPopupVisible = false;
        List<ListWorkOrdersDto> WorkOrdersList = new List<ListWorkOrdersDto>();

        public async Task WorkOrdersOnCreateIcon()
        {
            var WorkOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WorkOrdersButtonClickEvent);
            await WorkOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WorkOrdersButtonClick } });
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
                DataSource.WorkOrderNo = string.Empty;
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.StationGroupID = Guid.Empty;
                DataSource.StationGroupName = string.Empty;
                DataSource.OperationID = Guid.Empty;
                DataSource.OperationCode = string.Empty;
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.WorkOrderID = selectedOrder.Id;
                DataSource.WorkOrderNo = selectedOrder.WorkOrderNo;

                DataSource.ProductID = selectedOrder.ProductID;
                DataSource.ProductCode = selectedOrder.ProductCode;
                DataSource.ProductName = selectedOrder.ProductName;
                DataSource.StationGroupID = selectedOrder.StationGroupID;
                DataSource.StationGroupCode = selectedOrder.StationGroupCode;
                DataSource.OperationID = selectedOrder.ProductsOperationID;
                DataSource.OperationCode = selectedOrder.ProductsOperationCode;
                DataSource.ProductionOrderID = selectedOrder.ProductionOrderID;
                DataSource.ProductionOrderFicheNo = selectedOrder.ProductionOrderFicheNo;

                DataSource.StationID = Guid.Empty;
                DataSource.StationCode = string.Empty;
                DataSource.StationName = string.Empty;
                SelectWorkOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsCodeButtonEdit;
        SfTextBox StationsNameButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<SelectStationsDto> StationsList = new List<SelectStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsCodeButtonClickEvent);
            await StationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsCodeButtonClickEvent()
        {
            if (DataSource.WorkOrderID == Guid.Empty || DataSource.WorkOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitlebase"], L["UIWarningWorkOrderMessageStationbase"]);
            }
            else
            {
                SelectStationsPopupVisible = true;
                await GetStationsList();

            }
            await InvokeAsync(StateHasChanged);
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsNameButtonClickEvent()
        {
            if (DataSource.WorkOrderID == Guid.Empty || DataSource.WorkOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitlebase"], L["UIWarningWorkOrderMessageStationbase"]);
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
                DataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<SelectStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.StationID = selectedUnitSet.Id;
                DataSource.StationCode = selectedUnitSet.Code;
                DataSource.StationName = selectedUnitSet.Name;
                DataSource.StationGroupID = selectedUnitSet.GroupID;
                DataSource.StationGroupName = selectedUnitSet.StationGroup;
                DataSource.StationGroupCode = selectedUnitSet.StationGroupCode;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Personel ButtonEdit

        SfTextBox EmployeesButtonEdit;
        bool SelectEmployeesPopupVisible = false;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        public async Task EmployeesOnCreateIcon()
        {
            var EmployeesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeesButtonClickEvent);
            await EmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeesButtonClick } });
        }

        public async void EmployeesButtonClickEvent()
        {
            if (DataSource.WorkOrderID == Guid.Empty || DataSource.WorkOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitlebase"], L["UIWarningWorkOrderMessageEmployeebase"]);
            }
            else
            {
                SelectEmployeesPopupVisible = true;
                await GetEmployeesList();

            }
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
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.EmployeeID = selectedOrder.Id;
                DataSource.EmployeeName = selectedOrder.Name + " " + selectedOrder.Surname;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion


        #region Hata Başlığı ButtonEdit

        SfTextBox UnsuitabilityItemsButtonEdit;
        bool SelectUnsuitabilityItemsPopupVisible = false;
        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        public async Task UnsuitabilityItemsOnCreateIcon()
        {
            var UnsuitabilityItemsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnsuitabilityItemsButtonClickEvent);
            await UnsuitabilityItemsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnsuitabilityItemsButtonClick } });
        }

        public async void UnsuitabilityItemsButtonClickEvent()
        {

            SelectUnsuitabilityItemsPopupVisible = true;
            await GetUnsuitabilityItemsList();

            await InvokeAsync(StateHasChanged);
        }


        public void UnsuitabilityItemsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.UnsuitabilityItemsID = Guid.Empty;
                DataSource.UnsuitabilityItemsName = string.Empty;
            }
        }

        public async void UnsuitabilityItemsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnsuitabilityItemsDto> args)
        {
            var selectedUnsuitabilityItem = args.RowData;

            if (selectedUnsuitabilityItem != null)
            {
                DataSource.UnsuitabilityItemsID = selectedUnsuitabilityItem.Id;
                DataSource.UnsuitabilityItemsName = selectedUnsuitabilityItem.Name;
                SelectUnsuitabilityItemsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion


        #region GetList Metotları

        private async Task GetEmployeesList()
        {
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList.Clear();
            var productOperationId = (await WorkOrdersAppService.GetAsync(DataSource.WorkOrderID.GetValueOrDefault())).Data.ProductsOperationID;

            if (productOperationId != Guid.Empty)
            {
                var productOperation = (await ProductsOperationsAppService.GetAsync(productOperationId.GetValueOrDefault())).Data;

                foreach (var item in productOperation.SelectProductsOperationLines)
                {
                    var selectStationDto = (await StationsAppService.GetAsync(item.StationID.GetValueOrDefault())).Data;
                    StationsList.Add(selectStationDto);
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["EmptyProductOperationError"]);
                await InvokeAsync(StateHasChanged);
            }

            //StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.ToList();
        }

        private async Task GetUnsuitabilityItemsList()
        {
            var WorkCenterId = (await StationsAppService.GetAsync(DataSource.StationID.GetValueOrDefault())).Data.GroupID;
            if (WorkCenterId != Guid.Empty)
            {

                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.
                    Where(t => t.StationGroupId == WorkCenterId)
                    .ToList();
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("OprUnsRecordsChildMenu");
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
