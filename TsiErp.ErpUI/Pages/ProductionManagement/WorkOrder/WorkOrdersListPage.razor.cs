using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.WorkOrder
{
    public partial class WorkOrdersListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public bool StationChangeModalVisible = false;
        public string OldStation = string.Empty;
        public string NewStation = string.Empty;
        public Guid NewStationID = Guid.Empty;

        public bool WorkOrderSplitModalVisible = false;
        public string NewWorkOrderNo = string.Empty;
        public decimal NewPlannedQuantity = 0;

        public List<SelectContractTrackingFicheLinesDto> ContractTrackingFicheLinesList = new List<SelectContractTrackingFicheLinesDto>();

        public List<ListProductionTrackingsDto> ProductionTrackingsList = new List<ListProductionTrackingsDto>();

        public bool ProductionTrackingModalVisible = false;
        public bool ContractTrackingFicheLineModalVisible = false;


        protected override async void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "WorkOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateContextMenuItems();
        }

        protected void CreateContextMenuItems()
        {

            foreach (var context in contextsList)
            {
                var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    switch (context.MenuName)
                    {
                        case "WorkOrderContextAdd":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextAdd"], Id = "new" }); break;
                        case "WorkOrderContextChange":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChange"], Id = "changed" }); break;
                        case "WorkOrderContextDelete":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextDelete"], Id = "delete" }); break;
                        case "WorkOrderContextRefresh":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextRefresh"], Id = "refresh" }); break;
                        case "WorkOrderContextProductionTracking":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextProductionTracking"], Id = "prodtracking" }); break;
                        case "WorkOrderContextContractTracking":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextContractTracking"], Id = "contracttracking" }); break;
                        case "WorkOrderContextChangeOperationCriteria":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChangeOperationCriteria"], Id = "changeoprcriteria" }); break;
                        case "WorkOrderContextChangeStation":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChangeStation"], Id = "changestation" }); break;
                        case "WorkOrderContextSplitWorkOrder":
                            MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextSplitWorkOrder"], Id = "splitworkorder" }); break;
                        default: break;
                    }
                }
            }
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListWorkOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "prodtracking":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    ProductionTrackingsList = (await ProductionTrackingsAppService.GetListbyWorkOrderIDAsync(DataSource.Id)).Data.ToList();

                    ProductionTrackingModalVisible = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                case "contracttracking":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    ContractTrackingFicheLinesList = (await ContractTrackingFichesAppService.GetLineListbyWorkOrderIDAsync(DataSource.Id)).Data.ToList();

                    ContractTrackingFicheLineModalVisible = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                case "changeoprcriteria":

                    break;

                case "changestation":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    OldStation = DataSource.StationName;

                    NewStation = string.Empty;

                    NewStationID = Guid.Empty;

                    StationChangeModalVisible = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                case "splitworkorder":

                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    NewStation = string.Empty;

                    NewStationID = Guid.Empty;

                    NewWorkOrderNo = DataSource.WorkOrderNo + "." + (DataSource.SplitQuantity +1).ToString();

                    NewPlannedQuantity = 0;

                    WorkOrderSplitModalVisible = true;

                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }
        }

        #region İş İstasyonu Değiştirme Metotları

        public void HideStationChangeModal()
        {
            StationChangeModalVisible = false;
        }

        public async void OnStationChangeSubmit()
        {

            DataSource.StationID = NewStationID;
            DataSource.StationName = NewStation;

            var updatedEntity = ObjectMapper.Map<SelectWorkOrdersDto, UpdateWorkOrdersDto>(DataSource);

            await WorkOrdersAppService.UpdateAsync(updatedEntity);

            HideStationChangeModal();

            await GetListDataSourceAsync();

            await InvokeAsync(StateHasChanged);

        }

        #endregion

        #region İş Emri Parçalama Metotları

        public void HideWorkOrderSplitModal()
        {
            WorkOrderSplitModalVisible = false;
        }

        public async void OnWorkOrderSplitSubmit()
        {
            DataSource.SplitQuantity = DataSource.SplitQuantity + 1;

            var updatedEntity = ObjectMapper.Map<SelectWorkOrdersDto, UpdateWorkOrdersDto>(DataSource);

            await WorkOrdersAppService.UpdateAsync(updatedEntity);

            var station = (await StationsAppService.GetAsync(NewStationID)).Data;


            CreateWorkOrdersDto createWorkOrderModel = new CreateWorkOrdersDto
            {
                SplitQuantity = 0,
                AdjustmentAndControlTime = DataSource.AdjustmentAndControlTime,
                CurrentAccountCardID = DataSource.CurrentAccountCardID,
                IsCancel = DataSource.IsCancel,
                LineNr = 1,
                WorkOrderState = 1,
                WorkOrderNo = NewWorkOrderNo,
                StationID = NewStationID,
                StationGroupID = station.GroupID,
                RouteID = DataSource.RouteID,
                PropositionID = DataSource.PropositionID,
                ProductsOperationID = DataSource.ProductsOperationID,
                ProductionOrderID = DataSource.ProductionOrderID,
                ProductID = DataSource.ProductID,
                ProducedQuantity = DataSource.ProducedQuantity,
                PlannedQuantity = NewPlannedQuantity,
                OrderID = DataSource.OrderID,
                OperationTime = DataSource.OperationTime,
                OccuredStartDate = DataSource.OccuredStartDate,
                OccuredFinishDate = DataSource.OccuredFinishDate,
                LinkedWorkOrderID = DataSource.LinkedWorkOrderID,
            };

            await WorkOrdersAppService.CreateAsync(createWorkOrderModel);

            HideWorkOrderSplitModal();

            await GetListDataSourceAsync();

            await InvokeAsync(StateHasChanged);

        }

        #endregion

        #region Üretim Takip Metotları

        public void HideProductionTrackingModal()
        {
            ProductionTrackingModalVisible = false;
        }

        #endregion

        #region Fason Takip Metotları

        public void HideContractTrackingFicheModal()
        {
            ContractTrackingFicheLineModalVisible = false;
        }

        #endregion

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsCodeButtonEdit;
        SfTextBox StationsNameButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsCodeButtonClickEvent);
            await StationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsCodeButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsNameButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                NewStationID = Guid.Empty;
                NewStation = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                NewStationID = selectedUnitSet.Id;
                NewStation = selectedUnitSet.Name;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
