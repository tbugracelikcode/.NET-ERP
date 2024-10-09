using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.CostManagement.CostPeriod.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos;
using TsiErp.Entities.Entities.CostManagement.CostPeriodLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPR.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.CostManagement.CPR
{
    public partial class CPRsListPage : IDisposable
    {
        private SfGrid<SelectCPRManufacturingCostLinesDto> _ManufacturingCostLineGrid;
        private SfGrid<SelectCPRMaterialCostLinesDto> _MaterialCostLineGrid;
        private SfGrid<SelectCPRSetupCostLinesDto> _SetupCostLineGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();


        List<SelectCPRManufacturingCostLinesDto> ManufacturingCostGridLineList = new List<SelectCPRManufacturingCostLinesDto>();
        List<SelectCPRMaterialCostLinesDto> MaterialCostGridLineList = new List<SelectCPRMaterialCostLinesDto>();
        List<SelectCPRSetupCostLinesDto> SetupCostGridLineList = new List<SelectCPRSetupCostLinesDto>();
        public List<ContextMenuItemModel> ManufacturingCostLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MaterialCostLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> SetupCostLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectCPRManufacturingCostLinesDto ManufacturingCostLineDataSource;
        SelectCPRMaterialCostLinesDto MaterialCostLineDataSource;
        SelectCPRSetupCostLinesDto SetupCostLineDataSource;


        private bool ManufacturingCostLineCrudPopup = false;
        private bool MaterialCostLineCrudPopup = false;
        private bool SetupCostLineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = CPRsAppService;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CPRsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            MainContextMenuItems();

            _L = L;
        }

        #region CPR Satır İşlemleri

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCPRsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("CPRsChildMenu"),
                 Date_ = GetSQLDateAppService.GetDateFromSQL()
            };

            EditPageVisible = true;

            DataSource.SelectCPRManufacturingCostLines = new List<SelectCPRManufacturingCostLinesDto>();

            DataSource.SelectCPRMaterialCostLines = new List<SelectCPRMaterialCostLinesDto>();

            DataSource.SelectCPRSetupCostLines = new List<SelectCPRSetupCostLinesDto>();

            ManufacturingCostGridLineList = DataSource.SelectCPRManufacturingCostLines;

            MaterialCostGridLineList = DataSource.SelectCPRMaterialCostLines;

            SetupCostGridLineList = DataSource.SelectCPRSetupCostLines;

            return Task.CompletedTask;
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

                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void MainContextMenuItems()
        {
            if (MainGridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "CPRsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CPRsContextAdd"], Id = "new" }); break;
                            case "CPRsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CPRsContextChange"], Id = "changed" }); break;
                            case "CPRsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CPRsContextDelete"], Id = "delete" }); break;
                            case "CPRsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CPRsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void ManufacturingCostContextMenuItems()
        {
            if (ManufacturingCostLineGridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ManufacturingCostLinesContextAdd":
                                ManufacturingCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ManufacturingCostLinesContextAdd"], Id = "new" }); break;
                            case "ManufacturingCostLinesContextChange":
                                ManufacturingCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ManufacturingCostLinesContextChange"], Id = "changed" }); break;
                            case "ManufacturingCostLinesContextDelete":
                                ManufacturingCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ManufacturingCostLinesContextDelete"], Id = "delete" }); break;
                            case "ManufacturingCostLinesContextRefresh":
                                ManufacturingCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ManufacturingCostLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void MaterialCostContextMenuItems()
        {
            if (MaterialCostLineGridContextMenu.Count == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "MaterialCostLinesContextAdd":
                                MaterialCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaterialCostLinesContextAdd"], Id = "new" }); break;
                            case "MaterialCostLinesContextChange":
                                MaterialCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaterialCostLinesContextChange"], Id = "changed" }); break;
                            case "MaterialCostLinesContextDelete":
                                MaterialCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaterialCostLinesContextDelete"], Id = "delete" }); break;
                            case "MaterialCostLinesContextRefresh":
                                MaterialCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaterialCostLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void SetupCostContextMenuItems()
        {
            if (SetupCostLineGridContextMenu.Count == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "SetupCostLinesContextAdd":
                                SetupCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SetupCostLinesContextAdd"], Id = "new" }); break;
                            case "SetupCostLinesContextChange":
                                SetupCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SetupCostLinesContextChange"], Id = "changed" }); break;
                            case "SetupCostLinesContextDelete":
                                SetupCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SetupCostLinesContextDelete"], Id = "delete" }); break;
                            case "SetupCostLinesContextRefresh":
                                SetupCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["SetupCostLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListCPRsDto> args)
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
                        DataSource = (await CPRsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        ManufacturingCostGridLineList = DataSource.SelectCPRManufacturingCostLines;
                        MaterialCostGridLineList = DataSource.SelectCPRMaterialCostLines;
                        SetupCostGridLineList = DataSource.SelectCPRSetupCostLines;

                        ShowEditPage();

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

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void OnManufacturingCostContextMenuClick(ContextMenuClickEventArgs<SelectCPRManufacturingCostLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    ManufacturingCostLineDataSource = new SelectCPRManufacturingCostLinesDto();
                    ManufacturingCostLineCrudPopup = true;
                    ManufacturingCostLineDataSource.LineNr = ManufacturingCostGridLineList.Count + 1;
                  
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        ManufacturingCostLineDataSource = args.RowInfo.RowData;
                        ManufacturingCostLineCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                        if (res == true)
                        {
                            var line = args.RowInfo.RowData;

                            if (line.Id == Guid.Empty)
                            {
                                DataSource.SelectCPRManufacturingCostLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    DataSource.SelectCPRManufacturingCostLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    DataSource.SelectCPRManufacturingCostLines.Remove(line);
                                }
                            }

                            await _ManufacturingCostLineGrid.Refresh();
                            GetTotal();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _ManufacturingCostLineGrid.Refresh();
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

        public async void OnMaterialCostContextMenuClick(ContextMenuClickEventArgs<SelectCPRMaterialCostLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    MaterialCostLineDataSource = new SelectCPRMaterialCostLinesDto();
                    MaterialCostLineCrudPopup = true;
                    MaterialCostLineDataSource.LineNr = MaterialCostGridLineList.Count + 1;

                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        MaterialCostLineDataSource = args.RowInfo.RowData;
                        MaterialCostLineCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                        if (res == true)
                        {
                            var line = args.RowInfo.RowData;

                            if (line.Id == Guid.Empty)
                            {
                                DataSource.SelectCPRMaterialCostLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    DataSource.SelectCPRMaterialCostLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    DataSource.SelectCPRMaterialCostLines.Remove(line);
                                }
                            }

                            await _MaterialCostLineGrid.Refresh();
                            GetTotal();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _ManufacturingCostLineGrid.Refresh();
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

        public async void OnSetupCostContextMenuClick(ContextMenuClickEventArgs<SelectCPRSetupCostLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    SetupCostLineDataSource = new SelectCPRSetupCostLinesDto();
                    SetupCostLineCrudPopup = true;
                    SetupCostLineDataSource.LineNr = SetupCostGridLineList.Count + 1;

                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        SetupCostLineDataSource = args.RowInfo.RowData;
                        SetupCostLineCrudPopup = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                        if (res == true)
                        {
                            var line = args.RowInfo.RowData;

                            if (line.Id == Guid.Empty)
                            {
                                DataSource.SelectCPRSetupCostLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    DataSource.SelectCPRSetupCostLines.Remove(line);
                                    await GetListDataSourceAsync();
                                }
                                else
                                {
                                    DataSource.SelectCPRSetupCostLines.Remove(line);
                                }
                            }

                            await _SetupCostLineGrid.Refresh();
                            GetTotal();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _SetupCostLineGrid.Refresh();
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

        public void HideManufacturingCostLineModal()
        {
            ManufacturingCostLineCrudPopup = false;
        }

        public void HideMaterialCostLineModal()
        {
            MaterialCostLineCrudPopup = false;
        }

        public void HideSetupCostLineModal()
        {
            SetupCostLineCrudPopup = false;
        }

        protected async Task OnManufacturingCostLineSubmit()
        {

            if (ManufacturingCostLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectCPRManufacturingCostLines.Contains(ManufacturingCostLineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectCPRManufacturingCostLines.FindIndex(t => t.LineNr == ManufacturingCostLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectCPRManufacturingCostLines[selectedLineIndex] = ManufacturingCostLineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectCPRManufacturingCostLines.Add(ManufacturingCostLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectCPRManufacturingCostLines.FindIndex(t => t.Id == ManufacturingCostLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectCPRManufacturingCostLines[selectedLineIndex] = ManufacturingCostLineDataSource;
                }
            }

            ManufacturingCostGridLineList = DataSource.SelectCPRManufacturingCostLines;
            await _ManufacturingCostLineGrid.Refresh();

            HideManufacturingCostLineModal();
            await InvokeAsync(StateHasChanged);

        }

        protected async Task OnMaterialCostLineSubmit()
        {

            if (MaterialCostLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectCPRMaterialCostLines.Contains(MaterialCostLineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectCPRMaterialCostLines.FindIndex(t => t.LineNr == MaterialCostLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectCPRMaterialCostLines[selectedLineIndex] = MaterialCostLineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectCPRMaterialCostLines.Add(MaterialCostLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectCPRMaterialCostLines.FindIndex(t => t.Id == MaterialCostLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectCPRMaterialCostLines[selectedLineIndex] = MaterialCostLineDataSource;
                }
            }

            MaterialCostGridLineList = DataSource.SelectCPRMaterialCostLines;
            await _MaterialCostLineGrid.Refresh();

            HideMaterialCostLineModal();
            await InvokeAsync(StateHasChanged);

        }

        protected async Task OnSetupCostLineSubmit()
        {

            if (SetupCostLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectCPRSetupCostLines.Contains(SetupCostLineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectCPRSetupCostLines.FindIndex(t => t.LineNr == SetupCostLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectCPRSetupCostLines[selectedLineIndex] = SetupCostLineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectCPRSetupCostLines.Add(SetupCostLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectCPRSetupCostLines.FindIndex(t => t.Id == SetupCostLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectCPRSetupCostLines[selectedLineIndex] = SetupCostLineDataSource;
                }
            }

            SetupCostGridLineList = DataSource.SelectCPRSetupCostLines;
            await _SetupCostLineGrid.Refresh();

            HideSetupCostLineModal();
            await InvokeAsync(StateHasChanged);

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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CPRsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region ButtonEdit Metotları

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
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
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

        #region Alıcı ButtonEdit

        SfTextBox RecieverCurrentAccountCardsCodeButtonEdit = new();
        SfTextBox RecieverCurrentAccountCardsCustomerCodeButtonEdit = new();
        SfTextBox RecieverCurrentAccountCardsNameButtonEdit = new();
        bool SelectRecieverCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> RecieverCurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task RecieverCurrentAccountCardsCodeOnCreateIcon()
        {
            var RecieverCurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, RecieverCurrentAccountCardsCodeButtonClickEvent);
            await RecieverCurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", RecieverCurrentAccountCardsCodeButtonClick } });
        }

        public async void RecieverCurrentAccountCardsCodeButtonClickEvent()
        {
            SelectRecieverCurrentAccountCardsPopupVisible = true;
            RecieverCurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task RecieverCurrentAccountCardsNameOnCreateIcon()
        {
            var RecieverCurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, RecieverCurrentAccountCardsNameButtonClickEvent);
            await RecieverCurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", RecieverCurrentAccountCardsNameButtonClick } });
        }

        public async void RecieverCurrentAccountCardsNameButtonClickEvent()
        {
            SelectRecieverCurrentAccountCardsPopupVisible = true;
            RecieverCurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void RecieverCurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.RecieverID = Guid.Empty;
                DataSource.RecieverCode = string.Empty;
                DataSource.RecieverName = string.Empty;

              
            }
        }

        public async void RecieverCurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedReciever = args.RowData;

            if (selectedReciever != null)
            {
                DataSource.RecieverID = selectedReciever.Id; ;
                DataSource.RecieverCode = selectedReciever.Code;
                DataSource.RecieverName = selectedReciever.Name;
                SelectRecieverCurrentAccountCardsPopupVisible = false;

               

                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Tedarikçi ButtonEdit

        SfTextBox SupplierCurrentAccountCardsCodeButtonEdit = new();
        SfTextBox SupplierCurrentAccountCardsCustomerCodeButtonEdit = new();
        SfTextBox SupplierCurrentAccountCardsNameButtonEdit = new();
        bool SelectSupplierCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> SupplierCurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task SupplierCurrentAccountCardsCodeOnCreateIcon()
        {
            var SupplierCurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SupplierCurrentAccountCardsCodeButtonClickEvent);
            await SupplierCurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SupplierCurrentAccountCardsCodeButtonClick } });
        }

        public async void SupplierCurrentAccountCardsCodeButtonClickEvent()
        {
            SelectSupplierCurrentAccountCardsPopupVisible = true;
            SupplierCurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task SupplierCurrentAccountCardsNameOnCreateIcon()
        {
            var SupplierCurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SupplierCurrentAccountCardsNameButtonClickEvent);
            await SupplierCurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SupplierCurrentAccountCardsNameButtonClick } });
        }

        public async void SupplierCurrentAccountCardsNameButtonClickEvent()
        {
            SelectSupplierCurrentAccountCardsPopupVisible = true;
            SupplierCurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void SupplierCurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SupplierID = Guid.Empty;
                DataSource.SupplierCode = string.Empty;
                DataSource.SupplierName = string.Empty;


            }
        }

        public async void SupplierCurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedSupplier = args.RowData;

            if (selectedSupplier != null)
            {
                DataSource.SupplierID = selectedSupplier.Id; ;
                DataSource.SupplierCode = selectedSupplier.Code;
                DataSource.SupplierName = selectedSupplier.Name;
                SelectSupplierCurrentAccountCardsPopupVisible = false;



                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Para Birimleri ButtonEdit

        SfTextBox CurrenciesButtonEdit;
        bool SelectCurrencyPopupVisible = false;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        public async Task CurrenciesOnCreateIcon()
        {
            var CurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrenciesButtonClickEvent);
            await CurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrenciesButtonClick } });
        }

        public async void CurrenciesButtonClickEvent()
        {
            SelectCurrencyPopupVisible = true;
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                DataSource.CurrencyID = selectedCurrency.Id;
                DataSource.CurrencyCode = selectedCurrency.Name;
                SelectCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
