
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.ContractQualityPlan.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.ContractUnsuitabilityReport
{
    public partial class ContractUnsuitabilityReportsListPage:IDisposable
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
            new UnsComboBox(){ID = "Reject", Text="ComboboxReject"},
            new UnsComboBox(){ID = "Correction", Text="ComboboxCorrection"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="ComboboxToBeUsedAs"}
        };


        public bool isCreatedNewWorkOrder = false;
        public string CreatedWorkOrderNo = string.Empty;

        protected override async void OnInitialized()
        {
            BaseCrudService = ContractUnsuitabilityReportsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ContUnsRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectContractUnsuitabilityReportsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ContUnsRecordsChildMenu")
            };

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
                    EditPageVisible = true;

                    var createdWorkOrder = (await WorkOrdersAppService.GetbyLinkedWorkOrderAsync(DataSource.WorkOrderID.GetValueOrDefault())).Data;

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
                    break;

                case "Reject":
                    DataSource.Action_ = L["ComboboxReject"].Value;
                    break;

                case "Correction":
                    DataSource.Action_ = L["ComboboxCorrection"].Value;
                    break;

                case "ToBeUsedAs":
                    DataSource.Action_ = L["ComboboxToBeUsedAs"].Value;
                    break;

                default: break;
            }
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {

            foreach (var context in contextsList)
            {
                var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    switch (context.MenuName)
                    {
                        case "ContractUnsuitabilityReportContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractUnsuitabilityReportContextAdd"], Id = "new" }); break;
                        case "ContractUnsuitabilityReportContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractUnsuitabilityReportContextChange"], Id = "changed" }); break;
                        case "ContractUnsuitabilityReportContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractUnsuitabilityReportContextDelete"], Id = "delete" }); break;
                        case "ContractUnsuitabilityReportContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractUnsuitabilityReportContextRefresh"], Id = "refresh" }); break;
                        default: break;
                    }
                }
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
            if(DataSource.ContractTrackingFicheID == null || DataSource.ContractTrackingFicheID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkOrderTitleBase"], L["UIWarningWorkOrderMessageBase"]);
            }
            else
            {

                SelectWorkOrdersPopupVisible = true;
                await GetWorkOrdersList();
            }
            await InvokeAsync(StateHasChanged);
        }


        public void WorkOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkOrderID = Guid.Empty;
                DataSource.WorkOrderFicheNr = string.Empty;
            }
        }

        public async void WorkOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListWorkOrdersDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.WorkOrderID = selectedOrder.Id;
                DataSource.WorkOrderFicheNr = selectedOrder.WorkOrderNo;
                SelectWorkOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Fason Takip Fişi ButtonEdit

        SfTextBox ContractTrackingFichesButtonEdit;
        bool SelectContractTrackingFichesPopupVisible = false;
        List<ListContractTrackingFichesDto> ContractTrackingFichesList = new List<ListContractTrackingFichesDto>();

        public async Task ContractTrackingFichesOnCreateIcon()
        {
            var ContractTrackingFichesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ContractTrackingFichesButtonClickEvent);
            await ContractTrackingFichesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ContractTrackingFichesButtonClick } });
        }

        public async void ContractTrackingFichesButtonClickEvent()
        {
            SelectContractTrackingFichesPopupVisible = true;
            await GetContractTrackingFichesList();
            await InvokeAsync(StateHasChanged);
        }


        public void ContractTrackingFichesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ContractTrackingFicheID = Guid.Empty;
                DataSource.ContractTrackingFicheNr = string.Empty;
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNr = string.Empty;
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
            }
        }

        public async void ContractTrackingFichesDoubleClickHandler(RecordDoubleClickEventArgs<ListContractTrackingFichesDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.ContractTrackingFicheID = selectedOrder.Id;
                DataSource.ContractTrackingFicheNr = selectedOrder.FicheNr;
                DataSource.ProductionOrderID = selectedOrder.ProductionOrderID;
                DataSource.ProductionOrderFicheNr = selectedOrder.ProductionOrderNr;
                DataSource.CurrentAccountCardID = selectedOrder.CurrentAccountCardID;
                DataSource.CurrentAccountCardName = selectedOrder.CurrentAccountCardName;
                DataSource.CurrentAccountCardCode = selectedOrder.CurrentAccountCardCode;
                SelectContractTrackingFichesPopupVisible = false;
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


        private async Task GetWorkOrdersList()
        {
            WorkOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == DataSource.ProductionOrderID).ToList();
        }

        private async Task GetContractTrackingFichesList()
        {
            ContractTrackingFichesList = (await ContractTrackingFichesAppService.GetListAsync(new ListContractTrackingFichesParameterDto())).Data.ToList();
        }

        private async Task GetUnsuitabilityItemsList()
        {
            UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.ToList();
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ContUnsRecordsChildMenu");
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
