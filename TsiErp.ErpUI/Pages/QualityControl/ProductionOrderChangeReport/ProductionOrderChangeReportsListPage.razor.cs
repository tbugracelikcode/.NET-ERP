using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.ProductionOrderChangeReport
{
    public partial class ProductionOrderChangeReportsListPage : IDisposable
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
            new UnsComboBox(){ID = "Remanufacturing", Text="ComboboxRemanufacturing"},
            new UnsComboBox(){ID = "ProductionCancel", Text="ComboboxProductionCancel"}
        };

        public int actionComboIndex = 0;

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionOrderChangeReportsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProdOrderChangeRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
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
                            case "ProductionOrderChangeReportsContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderChangeReportsContextAdd"], Id = "new" }); break;
                            case "ProductionOrderChangeReportsContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderChangeReportsContextChange"], Id = "changed" }); break;
                            case "ProductionOrderChangeReportsContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderChangeReportsContextDelete"], Id = "delete" }); break;
                            case "ProductionOrderChangeReportsContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderChangeReportsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionOrderChangeReportsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ProdOrderChangeRecordsChildMenu")
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
                    foreach (var item in _unsComboBox)
                    {
                        item.Text = L[item.Text];
                    }

                    #region String Combobox Index Ataması
                    string scrap = L["ComboboxScrap"].Value;
                    string remanufacturing = L["ComboboxRemanufacturing"].Value;
                    string cancel = L["ComboboxProductionCancel"].Value;
                    var a = DataSource.Action_;

                    if (DataSource.Action_ == scrap) actionComboIndex = 0;
                    else if (DataSource.Action_ == remanufacturing) actionComboIndex = 1;
                    else if (DataSource.Action_ == cancel) actionComboIndex = 2; 
                    #endregion

                    EditPageVisible = true;

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
                    actionComboIndex = 0;
                    break;

                case "Remanufacturing":
                    DataSource.Action_ = L["ComboboxRemanufacturing"].Value;
                    actionComboIndex = 1;
                    break;

                case "ProductionCancel":
                    DataSource.Action_ = L["ComboboxProductionCancel"].Value;
                    actionComboIndex = 2;
                    break;

                default: break;
            }
        }


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

        #region Üretim Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {

            SelectProductionOrdersPopupVisible = true;
            await GetProductionOrdersList();

            await InvokeAsync(StateHasChanged);
        }


        public void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
                DataSource.LinkedProductionOrderID = Guid.Empty;
                DataSource.LinkedProductionOrderFicheNo = string.Empty;
                DataSource.SalesOrderID = Guid.Empty;
                DataSource.SalesOrderFicheNo = string.Empty;
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                DataSource.ProductionOrderID = selectedProductionOrder.Id;
                DataSource.ProductionOrderFicheNo = selectedProductionOrder.FicheNo;
                DataSource.LinkedProductionOrderID = selectedProductionOrder.LinkedProductionOrderID;
                DataSource.LinkedProductionOrderFicheNo = selectedProductionOrder.LinkedProductionOrderFicheNo;
                DataSource.SalesOrderID = selectedProductionOrder.OrderID;
                DataSource.SalesOrderFicheNo = selectedProductionOrder.OrderFicheNo;
                DataSource.ProductID = selectedProductionOrder.FinishedProductID;
                DataSource.ProductCode =selectedProductionOrder.FinishedProductCode;
                DataSource.ProductName = selectedProductionOrder.FinishedProductName;
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region GetList Metotları

        private async Task GetUnsuitabilityItemsList()
        {
            var unsuitabilityTypesItem = (await UnsuitabilityTypesItemsAppService.GetWithUnsuitabilityItemDescriptionAsync("ProductionOrderChange")).Data;

            if (unsuitabilityTypesItem != null)
            {
                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.Where(t => t.UnsuitabilityTypesItemsName == unsuitabilityTypesItem.Name).ToList();
            }
        }

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ProdOrderChangeRecordsChildMenu");
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
