using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;

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
        public int reimbursementComboIndex = 0;
        public int includingOEEComboIndex = 0;
        public int contractProductionComboIndex = 0;

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
            ManufacturingCostContextMenuItems();
            MaterialCostContextMenuItems();
            SetupCostContextMenuItems();

            _L = L;
        }

        #region CPR Satır İşlemleri

        #region Enum Combobox İşlemleri

        public IEnumerable<SelectCPRsDto> incoterms = GetEnumDisplayIncotermsNames<CPRsIncotermEnum>();

        public static List<SelectCPRsDto> GetEnumDisplayIncotermsNames<T>()
        {
            var type = typeof(T);

            return Enum.GetValues(type)
                       .Cast<CPRsIncotermEnum>()
                       .Select(x => new SelectCPRsDto
                       {
                           Incoterms = x,
                           IncotermsName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();

        }

        #endregion

        #region Manuel Combobox İşlemleri

        public class ReimbursementComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ReimbursementComboBox> _ReimbursementComboBox = new List<ReimbursementComboBox>
        {
            new ReimbursementComboBox(){ID = "Yes", Text="Yes"},
            new ReimbursementComboBox(){ID = "No", Text="No"}
        };

        private void ReimbursementComboBoxValueChangeHandler(ChangeEventArgs<string, ReimbursementComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        MaterialCostLineDataSource.Reimbursement = L["Yes"].Value;
                        reimbursementComboIndex = 0;
                        break;

                    case "No":
                        MaterialCostLineDataSource.Reimbursement = L["No"].Value;
                        reimbursementComboIndex = 1;
                        break;


                    default: break;
                }
            }
        }

        public class IncludingOEEComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<IncludingOEEComboBox> _IncludingOEEComboBox = new List<IncludingOEEComboBox>
        {
            new IncludingOEEComboBox(){ID = "Yes", Text="Yes"},
            new IncludingOEEComboBox(){ID = "No", Text="No"}
        };

        private void IncludingOEEComboBoxValueChangeHandler(ChangeEventArgs<string, IncludingOEEComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        ManufacturingCostLineDataSource.IncludingOEE = L["Yes"].Value;
                        includingOEEComboIndex = 0;
                        break;

                    case "No":
                        ManufacturingCostLineDataSource.IncludingOEE = L["No"].Value;
                        includingOEEComboIndex = 1;
                        break;


                    default: break;
                }
            }
        }

        public class ContractProductionComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ContractProductionComboBox> _ContractProductionComboBox = new List<ContractProductionComboBox>
        {
            new ContractProductionComboBox(){ID = "Yes", Text="Yes"},
            new ContractProductionComboBox(){ID = "No", Text="No"}
        };

        private void ContractProductionComboBoxValueChangeHandler(ChangeEventArgs<string, ContractProductionComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Yes":
                        ManufacturingCostLineDataSource.ContractProduction = L["Yes"].Value;
                        contractProductionComboIndex = 0;
                        break;

                    case "No":
                        ManufacturingCostLineDataSource.ContractProduction = L["No"].Value;
                        contractProductionComboIndex = 1;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCPRsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("CPRsChildMenu"),
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                Incoterms = CPRsIncotermEnum.FCA,
                Quantity = 1
            };

            EditPageVisible = true;

            DataSource.SelectCPRManufacturingCostLines = new List<SelectCPRManufacturingCostLinesDto>();

            DataSource.SelectCPRMaterialCostLines = new List<SelectCPRMaterialCostLinesDto>();

            DataSource.SelectCPRSetupCostLines = new List<SelectCPRSetupCostLinesDto>();

            ManufacturingCostGridLineList = DataSource.SelectCPRManufacturingCostLines;

            MaterialCostGridLineList = DataSource.SelectCPRMaterialCostLines;

            SetupCostGridLineList = DataSource.SelectCPRSetupCostLines;

            foreach (var item in _ReimbursementComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _IncludingOEEComboBox)
            {
                item.Text = L[item.Text];
            }

            foreach (var item in _ContractProductionComboBox)
            {
                item.Text = L[item.Text];
            }

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

                    foreach (var item in _ReimbursementComboBox)
                    {
                        item.Text = L[item.Text];
                    }

                    foreach (var item in _IncludingOEEComboBox)
                    {
                        item.Text = L[item.Text];
                    }

                    foreach (var item in _ContractProductionComboBox)
                    {
                        item.Text = L[item.Text];
                    }

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
                                    await CPRsAppService.DeleteSetupCostAsync(args.RowInfo.RowData.Id);
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

        public void HideSetupCostLineModal()
        {
            SetupCostLineCrudPopup = false;
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

        #region Material Cost

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

                            case "MaterialCostLinesContextMaterialPurchaseUnitPrice":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "MaterialCostLinesContextLastPurchasePrice":
                                                subMenus.Add(new MenuItem { Text = L["MaterialCostLinesContextLastPurchasePrice"], Id = "last" }); break;

                                            case "MaterialCostLinesContextHighestPurchasePrice":
                                                subMenus.Add(new MenuItem { Text = L["MaterialCostLinesContextHighestPurchasePrice"], Id = "highest" }); break;

                                            case "MaterialCostLinesContextLowestPurchasePrice":
                                                subMenus.Add(new MenuItem { Text = L["MaterialCostLinesContextLowestPurchasePrice"], Id = "lowest" }); break;

                                            case "MaterialCostLinesContextAveragePurchasePrice":
                                                subMenus.Add(new MenuItem { Text = L["MaterialCostLinesContextAveragePurchasePrice"], Id = "average" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                MaterialCostLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaterialCostLinesContextMaterialPurchaseUnitPrice"], Id = "materialpurchaseprice", Items = subMenus }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void OnMaterialCostContextMenuClick(ContextMenuClickEventArgs<SelectCPRMaterialCostLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    MaterialCostLineDataSource = new SelectCPRMaterialCostLinesDto();
                    MaterialCostLineCrudPopup = true;
                    reimbursementComboIndex = 0;
                    MaterialCostLineDataSource.LineNr = MaterialCostGridLineList.Count + 1;

                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        MaterialCostLineDataSource = args.RowInfo.RowData;

                        if (MaterialCostLineDataSource.Reimbursement == L["Yes"].Value) reimbursementComboIndex = 0;
                        else if (MaterialCostLineDataSource.Reimbursement == L["No"].Value) reimbursementComboIndex = 1;

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
                                    await CPRsAppService.DeleteMaterialCostAsync(args.RowInfo.RowData.Id);
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

                case "last":
                    if (args.RowInfo.RowData != null)
                    {

                        MaterialCostLineDataSource = args.RowInfo.RowData;

                        decimal lastPrice = PurchaseOrdersAppService.LastPurchasePrice(MaterialCostLineDataSource.ProductID.GetValueOrDefault());

                        MaterialCostLineDataSource.BaseMaterialPrice = lastPrice;

                        MaterialCostCalculate();
                        MaterialScrapCostCalculate();

                        await OnMaterialCostLineSubmit();

                        await _MaterialCostLineGrid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "highest":
                    if (args.RowInfo.RowData != null)
                    {

                        MaterialCostLineDataSource = args.RowInfo.RowData;

                        decimal highestPrice = PurchaseOrdersAppService.HighestPurchasePrice(MaterialCostLineDataSource.ProductID.GetValueOrDefault());

                        MaterialCostLineDataSource.BaseMaterialPrice = highestPrice;

                        MaterialCostCalculate();
                        MaterialScrapCostCalculate();

                        await OnMaterialCostLineSubmit();

                        await _MaterialCostLineGrid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "lowest":
                    if (args.RowInfo.RowData != null)
                    {

                        MaterialCostLineDataSource = args.RowInfo.RowData;

                        decimal lowestPrice = PurchaseOrdersAppService.LowestPurchasePrice(MaterialCostLineDataSource.ProductID.GetValueOrDefault());

                        MaterialCostLineDataSource.BaseMaterialPrice = lowestPrice;

                        MaterialCostCalculate();
                        MaterialScrapCostCalculate();

                        await OnMaterialCostLineSubmit();

                        await _MaterialCostLineGrid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "average":
                    if (args.RowInfo.RowData != null)
                    {

                        MaterialCostLineDataSource = args.RowInfo.RowData;

                        decimal averagePrice = PurchaseOrdersAppService.AveragePurchasePrice(MaterialCostLineDataSource.ProductID.GetValueOrDefault());

                        MaterialCostLineDataSource.BaseMaterialPrice = averagePrice;

                        MaterialCostCalculate();
                        MaterialScrapCostCalculate();

                        await OnMaterialCostLineSubmit();

                        await _MaterialCostLineGrid.Refresh();

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _MaterialCostLineGrid.Refresh();
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

            DataSource.SubtotalMaterialCost = MaterialCostGridLineList.Sum(t => t.MaterialCost);
            DataSource.SubtotalMaterialScrapCost = MaterialCostGridLineList.Sum(t => t.ScrapCost);

            HideMaterialCostLineModal();
            await InvokeAsync(StateHasChanged);

        }

        public void HideMaterialCostLineModal()
        {
            MaterialCostLineCrudPopup = false;
        }

        public void MaterialCostCalculate()
        {
            if (MaterialCostLineDataSource.GrossWeightperPart > MaterialCostLineDataSource.NetWeightperPart)
            {
                MaterialCostLineDataSource.MaterialCost = ((MaterialCostLineDataSource.GrossWeightperPart * MaterialCostLineDataSource.BaseMaterialPrice) + (MaterialCostLineDataSource.GrossWeightperPart * MaterialCostLineDataSource.BaseMaterialPrice * MaterialCostLineDataSource.MaterialOverhead / 100));
            }
            else if (MaterialCostLineDataSource.GrossWeightperPart < MaterialCostLineDataSource.NetWeightperPart)
            {
                MaterialCostLineDataSource.MaterialCost = ((MaterialCostLineDataSource.NetWeightperPart * MaterialCostLineDataSource.BaseMaterialPrice) + (MaterialCostLineDataSource.NetWeightperPart * MaterialCostLineDataSource.BaseMaterialPrice * MaterialCostLineDataSource.MaterialOverhead / 100));
            }
            else //Fark etmez
            {
                MaterialCostLineDataSource.MaterialCost = ((MaterialCostLineDataSource.GrossWeightperPart * MaterialCostLineDataSource.BaseMaterialPrice) + (MaterialCostLineDataSource.GrossWeightperPart * MaterialCostLineDataSource.BaseMaterialPrice * MaterialCostLineDataSource.MaterialOverhead / 100));
            }

        }

        public void MaterialScrapCostCalculate()
        {
            MaterialCostLineDataSource.ScrapCost = MaterialCostLineDataSource.ScrapRate * MaterialCostLineDataSource.MaterialCost;
        }

        #endregion

        #region Manufacturing Cost

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

        public async void OnManufacturingCostContextMenuClick(ContextMenuClickEventArgs<SelectCPRManufacturingCostLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    ManufacturingCostLineDataSource = new SelectCPRManufacturingCostLinesDto();
                    ManufacturingCostLineCrudPopup = true;
                    includingOEEComboIndex = 0;
                    contractProductionComboIndex = 0;
                    ManufacturingCostLineDataSource.PartsperCycle = 1;
                    ManufacturingCostLineDataSource.HeadCountatWorkingSystem = 100;
                    ManufacturingCostLineDataSource.LineNr = ManufacturingCostGridLineList.Count + 1;

                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        ManufacturingCostLineDataSource = args.RowInfo.RowData;

                        if (ManufacturingCostLineDataSource.IncludingOEE == L["Yes"].Value) includingOEEComboIndex = 0;
                        else if (ManufacturingCostLineDataSource.IncludingOEE == L["No"].Value) includingOEEComboIndex = 1;

                        if (ManufacturingCostLineDataSource.ContractProduction == L["Yes"].Value) contractProductionComboIndex = 0;
                        else if (ManufacturingCostLineDataSource.ContractProduction == L["No"].Value) contractProductionComboIndex = 1;

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
                                    await CPRsAppService.DeleteManufacturingCostAsync(args.RowInfo.RowData.Id);
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

        public void HideManufacturingCostLineModal()
        {
            ManufacturingCostLineCrudPopup = false;
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

            DataSource.SubtotalManufacturingCost = ManufacturingCostGridLineList.Sum(t => t.ManufacuringStepCost);
            DataSource.SubtotalManufacturingScrapCost = ManufacturingCostGridLineList.Sum(t => t.ScrapCost);

            HideManufacturingCostLineModal();
            await InvokeAsync(StateHasChanged);

        }

        public void ManufacturingWorkingSystemCostCalculate()
        {
            ManufacturingCostLineDataSource.WorkingSystemCostperPart = ManufacturingCostLineDataSource.NetOutputDVOEE == 0 ? 0 : ManufacturingCostLineDataSource.WorkingSystemHourlyRate / ManufacturingCostLineDataSource.NetOutputDVOEE;
        }

        public void ManufacturingLaborCostCalculate()
        {
            ManufacturingCostLineDataSource.LaborCostperPart = ManufacturingCostLineDataSource.NetOutputDVOEE == 0 ? 0 : (ManufacturingCostLineDataSource.DirectLaborHourlyRate / ManufacturingCostLineDataSource.NetOutputDVOEE) / 100 * ManufacturingCostLineDataSource.HeadCountatWorkingSystem;
        }

        public void ManufacturingCalculationforNetOutputOEE()
        {
            ManufacturingCostLineDataSource.WorkingSystemCostperPart = ManufacturingCostLineDataSource.NetOutputDVOEE == 0 ? 0 : ManufacturingCostLineDataSource.WorkingSystemHourlyRate / ManufacturingCostLineDataSource.NetOutputDVOEE;

            ManufacturingCostLineDataSource.LaborCostperPart = ManufacturingCostLineDataSource.NetOutputDVOEE == 0 ? 0 : (ManufacturingCostLineDataSource.DirectLaborHourlyRate / ManufacturingCostLineDataSource.NetOutputDVOEE) / 100 * ManufacturingCostLineDataSource.HeadCountatWorkingSystem;
        }

        public void ManufacturingScrapCostCalculate()
        {
            ManufacturingCostLineDataSource.ScrapCost = ManufacturingCostLineDataSource.ScrapRate * ManufacturingCostLineDataSource.ManufacuringStepCost;
        }

        public void ManufacturingStepCostCalculate()
        {
            ManufacturingCostLineDataSource.ManufacuringStepCost = (ManufacturingCostLineDataSource.WorkingSystemCostperPart + ManufacturingCostLineDataSource.LaborCostperPart) * (1 + (ManufacturingCostLineDataSource.ResidualManufacturingOverhead / 100));
        }

        #endregion



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
                MaterialCostGridLineList.Clear();
                ManufacturingCostGridLineList.Clear();
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

                List<SelectBillsofMaterialLinesDto> bomList = (await BillsofMaterialsAppService.GetLineListbyFinishedProductIDAsync(selectedProduct.Id)).Data.ToList();

                if (bomList != null && bomList.Count > 0)
                {
                    reimbursementComboIndex = 0;

                    MaterialCostGridLineList.Clear();

                    foreach (var bom in bomList)
                    {
                        decimal netWeight = 0;
                        decimal grossWeight = 0;

                        SelectProductsDto bomProduct = (await ProductsAppService.GetAsync(bom.ProductID.GetValueOrDefault())).Data;

                        if(bomProduct != null && bomProduct.Id != Guid.Empty)
                        {
                            if(bomProduct.SupplyForm == ProductSupplyFormEnum.Satınalma)
                            {
                                netWeight = bomProduct.UnitWeight;
                                grossWeight = bomProduct.UnitWeight;
                            }
                            else if(bomProduct.SupplyForm == ProductSupplyFormEnum.Üretim)
                            {
                                netWeight = bomProduct.UnitWeight;  

                                grossWeight = (await BillsofMaterialsAppService.GetbyProductIDAsync(bomProduct.Id)).Data.SelectBillsofMaterialLines.Select(t=>t.Quantity).FirstOrDefault();
                            }
                        } 

                        SelectCPRMaterialCostLinesDto materialCostLineModel = new SelectCPRMaterialCostLinesDto
                        {
                            ProductID = bom.ProductID.GetValueOrDefault(),
                            ProductCode = bom.ProductCode,
                            ProductName = bom.ProductName,
                            BaseMaterialPrice = 0,
                            GrossWeightperPart = grossWeight,
                            LineNr = MaterialCostGridLineList.Count + 1,
                            MaterialCost = 0,
                            Quantity = 0,
                            MaterialOverhead = 0,
                            NetWeightperPart = netWeight,
                            ScrapCost = 0,
                            Reimbursement = L["Yes"].Value,
                            ScrapRate = 0,
                            SurchargesMaterialPrice = 0,
                            UnitPrice = 0,
                        };

                        MaterialCostGridLineList.Add(materialCostLineModel);
                    }

                    await _MaterialCostLineGrid.Refresh();
                }

                var route = (await RoutesAppService.GetbyProductIDAsync(selectedProduct.Id)).Data;

                if (route != null && route.Id != Guid.Empty && route.SelectRouteLines != null && route.SelectRouteLines.Count > 0)
                {
                    ManufacturingCostGridLineList.Clear();

                    includingOEEComboIndex = 0;

                    contractProductionComboIndex = 0;

                    foreach (var routeLine in route.SelectRouteLines)
                    {

                        SelectCPRManufacturingCostLinesDto manufacturingCostLineModel = new SelectCPRManufacturingCostLinesDto
                        {
                            ProductsOperationID = routeLine.ProductsOperationID,
                            ProductsOperationName = routeLine.OperationName,
                            LineNr = ManufacturingCostGridLineList.Count + 1,
                            DirectLaborHourlyRate = 0,
                            ContractProduction = L["Yes"].Value,
                            ContractUnitCost = 0,
                            HeadCountatWorkingSystem = 100,
                            IncludingOEE = L["Yes"].Value,
                            LaborCostperPart = 0,
                            ManufacuringStepCost = 0,
                            Material_ = string.Empty,
                            NetOutputDVOEE = 0,
                            ResidualManufacturingOverhead = 0,
                            WorkingSystemCostperPart = 0,
                            WorkingSystemHourlyRate = 0,
                            PartsperCycle = 0,
                            StationID = Guid.Empty,
                            StationCode = string.Empty,
                            StationName = string.Empty,
                            WorkingSystemInvest = 0,
                            ScrapRate = 0,
                            ScrapCost = 0,
                        };

                        ManufacturingCostGridLineList.Add(manufacturingCostLineModel);
                    }


                }

                //SelectProductsOperationsDto productsOperation = (await ProductsOperationsAppService.GetbyProductAsync(selectedProduct.Id)).Data;

                //if (productsOperation != null && productsOperation.Id != Guid.Empty && productsOperation.SelectProductsOperationLines != null && productsOperation.SelectProductsOperationLines.Count > 0)
                //{
                //    foreach(var line in productsOperation.SelectProductsOperationLines)
                //    {
                //        SelectCPR
                //    }
                //}

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
                DataSource.CurrencyCode = string.Empty;
                DataSource.CurrencyID = Guid.Empty;


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
                DataSource.CurrencyCode = selectedReciever.Currency;
                DataSource.CurrencyID = selectedReciever.CurrencyID.GetValueOrDefault();
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

        #region İstasyon Button Edit

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

            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void StationsNameButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                ManufacturingCostLineDataSource.StationID = Guid.Empty;
                ManufacturingCostLineDataSource.StationCode = string.Empty;
                ManufacturingCostLineDataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedStation = args.RowData;

            if (selectedStation != null)
            {
                ManufacturingCostLineDataSource.StationID = selectedStation.Id;
                ManufacturingCostLineDataSource.StationCode = selectedStation.Code;
                ManufacturingCostLineDataSource.StationName = selectedStation.Name;
                ManufacturingCostLineDataSource.HeadCountatWorkingSystem = selectedStation.TotalEmployees == 0 ? 100 : (1 / selectedStation.TotalEmployees) * 100;

                //SelectStationsOperationsDto StationsOperation = (await StationsOperationsAppService.GetbyStationAsync(selectedStation.Id)).Data;

                //if (StationsOperation != null && StationsOperation.Id != Guid.Empty && StationsOperation.SelectStationsOperationLines != null && StationsOperation.SelectStationsOperationLines.Count > 0)
                //{
                //    foreach(var line in StationsOperation.SelectStationsOperationLines)
                //    {
                //        SelectCPR
                //    }
                //}

                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Malzeme Button Edit

        SfTextBox MaterialProductsCodeButtonEdit;
        SfTextBox MaterialProductsNameButtonEdit;
        bool SelectMaterialProductsPopupVisible = false;
        List<ListProductsDto> MaterialProductsList = new List<ListProductsDto>();
        public async Task MaterialProductsCodeOnCreateIcon()
        {
            var MaterialProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, MaterialProductsCodeButtonClickEvent);
            await MaterialProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", MaterialProductsButtonClick } });
        }

        public async void MaterialProductsCodeButtonClickEvent()
        {
            SelectMaterialProductsPopupVisible = true;

            MaterialProductsList.Clear();

            List<ListProductsDto> products = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

            if (DataSource.ProductID != null && DataSource.ProductID != Guid.Empty)
            {
                List<SelectBillsofMaterialLinesDto> bomList = (await BillsofMaterialsAppService.GetLineListbyFinishedProductIDAsync(DataSource.ProductID.GetValueOrDefault())).Data.ToList();



                foreach (var bill in bomList)
                {
                    ListProductsDto product = products.Where(t => t.Id == bill.ProductID.GetValueOrDefault()).FirstOrDefault();

                    MaterialProductsList.Add(product);
                }
            }
            else
            {
                MaterialProductsList = products;

            }

            await InvokeAsync(StateHasChanged);
        }


        public void MaterialProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                ManufacturingCostLineDataSource.ProductID = Guid.Empty;
                ManufacturingCostLineDataSource.ProductCode = string.Empty;
                ManufacturingCostLineDataSource.ProductName = string.Empty;
            }
        }

        public async void MaterialProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                ManufacturingCostLineDataSource.ProductID = selectedProduct.Id;
                ManufacturingCostLineDataSource.ProductCode = selectedProduct.Code;
                ManufacturingCostLineDataSource.ProductName = selectedProduct.Name;



                SelectMaterialProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Operasyon ButtonEdit

        SfTextBox ProductsOperationsButtonEdit;
        bool SelectProductsOperationPopupVisible = false;
        List<ListProductsOperationsDto> ProductsOperationsList = new List<ListProductsOperationsDto>();

        public async Task ProductsOperationsOnCreateIcon()
        {
            var ProductsOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsOperationsButtonClickEvent);
            await ProductsOperationsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsOperationsButtonClick } });
        }

        public async void ProductsOperationsButtonClickEvent()
        {
            SelectProductsOperationPopupVisible = true;

            if(DataSource.ProductID != null && DataSource.ProductID != Guid.Empty)
            {
                ProductsOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID).ToList();
            }
            else
            {
                ProductsOperationsList = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.ToList();
            }

            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOperationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                SetupCostLineDataSource.ProductsOperationID = Guid.Empty;
                SetupCostLineDataSource.ProductsOperationName = string.Empty;
            }
        }

        public async void ProductsOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsOperationsDto> args)
        {
            var selectedProductsOperation = args.RowData;

            if (selectedProductsOperation != null)
            {
                SetupCostLineDataSource.ProductsOperationID = selectedProductsOperation.Id;
                SetupCostLineDataSource.ProductsOperationName = selectedProductsOperation.Name;
                SelectProductsOperationPopupVisible = false;
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
