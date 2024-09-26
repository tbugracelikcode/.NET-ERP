using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductsOperation
{
    public partial class ProductsOperationsListPage : IDisposable
    {
        private SfGrid<SelectProductsOperationLinesDto> _LineGrid;
        private SfGrid<SelectContractOfProductsOperationsDto> _ContractOfProductsOperationsGrid;
        private SfGrid<AmountsbyProductionOrders> _AmountsbyProductionOrdersGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectProductsOperationLinesDto StationLineDataSource;

       
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ContextMenuItemModel> StationLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ContractOfProductsOperationsGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectProductsOperationLinesDto> StationGridLineList = new List<SelectProductsOperationLinesDto>();

        List<SelectContractOfProductsOperationsDto> ContractOfProductsOperationsGridLineList = new List<SelectContractOfProductsOperationsDto>();

        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        List<AmountsbyProductionOrders> AmountsbyProductionOrdersList = new List<AmountsbyProductionOrders>();

        #region Üretim Emirlerine Göre Toplamlar 

        public class AmountsbyProductionOrders
        {
            public Guid ProductionOrderID { get; set; }

            public string ProductionOrderFİcheNr { get; set; }

            public Guid SalesOrderID { get; set; }

            public string SalesOrderFicheNr { get; set; }

            public string CustomerOrderNr { get; set; }

            public decimal Amount { get; set; }
        };

        #endregion


        bool StationLineCrudPopup;

        //bool QualityPlanLineCrudPopup = new();

        bool AmountsbyProductionOrdersPopup;

        bool SelectCurrentAccountCardsPopupVisible;

        protected override async void OnInitialized()
        {

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProdOperationsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateStationLineContextMenuItems();
            CreateContractOfProductsOperationsContextMenuItems();

            BaseCrudService = ProductsOperationsAppService;
            _L = L;

        }

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
                DataSource.Name = string.Empty;
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

                if (DataSource.TemplateOperationID != Guid.Empty && DataSource.ProductID != Guid.Empty)
                {
                    if (DataSource.TemplateOperationID != null && DataSource.ProductID != null)
                    {
                        DataSource.Name = DataSource.ProductCode + " / " + DataSource.TemplateOperationName;
                    }
                }

                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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
                StationLineDataSource.StationID = Guid.Empty;
                StationLineDataSource.StationCode = string.Empty;
                StationLineDataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                StationLineDataSource.StationID = selectedUnitSet.Id;
                StationLineDataSource.StationCode = selectedUnitSet.Code;
                StationLineDataSource.StationName = selectedUnitSet.Code;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şablon Operasyon ButtonEdit

        SfTextBox TemplateOperationButtonEdit;
        bool SelectTemplateOperationsPopupVisible = false;
        List<ListTemplateOperationsDto> TemplateOperationsList = new List<ListTemplateOperationsDto>();

        public async Task TemplateOperationOnCreateIcon()
        {
            var TemplateOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TemplateOperationButtonClickEvent);
            await TemplateOperationButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TemplateOperationsButtonClick } });
        }

        public async void TemplateOperationButtonClickEvent()
        {
            SelectTemplateOperationsPopupVisible = true;
            await GetTemplateOperationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void TemplateOperationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TemplateOperationID = Guid.Empty;
                DataSource.TemplateOperationCode = string.Empty;
                DataSource.TemplateOperationName = string.Empty;
                DataSource.WorkCenterID = Guid.Empty;
                DataSource.WorkCenterName = string.Empty;
                DataSource.Name = string.Empty;
            }
        }

        public async void TemplateOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListTemplateOperationsDto> args)
        {
            var selectedTemplateOperation = args.RowData;

            if (selectedTemplateOperation != null)
            {

                StationGridLineList.Clear();

                DataSource.TemplateOperationID = selectedTemplateOperation.Id;
                DataSource.TemplateOperationCode = selectedTemplateOperation.Code;
                DataSource.TemplateOperationName = selectedTemplateOperation.Name;
                DataSource.WorkCenterID = selectedTemplateOperation.WorkCenterID;
                DataSource.WorkCenterName = selectedTemplateOperation.WorkCenterName;

                if(DataSource.TemplateOperationID != Guid.Empty && DataSource.ProductID != Guid.Empty)
                {
                    if(DataSource.TemplateOperationID != null && DataSource.ProductID != null)
                    {
                        DataSource.Name = DataSource.ProductCode + " / " + DataSource.TemplateOperationName;
                    }
                }

                foreach (var line in selectedTemplateOperation.SelectTemplateOperationLines)
                {
                    SelectProductsOperationLinesDto lineModel = new SelectProductsOperationLinesDto
                    {
                        AdjustmentAndControlTime = line.AdjustmentAndControlTime,
                        Alternative = line.Alternative,
                        LineNr = line.LineNr,
                        StationID = line.StationID,
                        StationCode = line.StationCode,
                        StationName = line.StationName,
                        ProductsOperationName = DataSource.Name,
                        ProductsOperationID = DataSource.Id,
                        ProductsOperationCode = DataSource.Code,
                        ProcessQuantity = line.ProcessQuantity,
                        Priority = line.Priority,
                        OperationTime = line.OperationTime,
                    };

                    StationGridLineList.Add(lineModel);
                    await _LineGrid.Refresh();
                }
                SelectTemplateOperationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Ürüne Özel Operasyon Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsOperationsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProdOperationsChildMenu")
            };

            DataSource.SelectProductsOperationLines = new List<SelectProductsOperationLinesDto>();
            StationGridLineList = DataSource.SelectProductsOperationLines;

            DataSource.SelectContractOfProductsOperationsLines = new List<SelectContractOfProductsOperationsDto>();
            ContractOfProductsOperationsGridLineList = DataSource.SelectContractOfProductsOperationsLines;

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        protected async Task StationLineBeforeInsertAsync()
        {
            StationLineDataSource = new SelectProductsOperationLinesDto()
            {
                Alternative = false,
                Priority = StationGridLineList.Count + 1,
                LineNr = StationGridLineList.Count + 1
            };

            await Task.CompletedTask;
        }

        protected void CreateStationLineContextMenuItems()
        {
            if (StationLineGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ProductsOperationLineContextAdd":
                                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationLineContextAdd"], Id = "new" }); break;
                            case "ProductsOperationLineContextChange":
                                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationLineContextChange"], Id = "changed" }); break;
                            case "ProductsOperationLineContextDelete":
                                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationLineContextDelete"], Id = "delete" }); break;
                            case "ProductsOperationLineContextRefresh":
                                StationLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
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
                            case "ProductsOperationContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationContextAdd"], Id = "new" }); break;
                            case "ProductsOperationContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationContextChange"], Id = "changed" }); break;
                            case "ProductsOperationContextAmounts":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationContextAmounts"], Id = "amounts" }); break;
                            case "ProductsOperationContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationContextDelete"], Id = "delete" }); break;
                            case "ProductsOperationContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductsOperationContextRefresh"], Id = "refresh" }); break;
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
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductsOperationsDto> args)
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
                    DataSource = (await ProductsOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    StationGridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();
                    ContractOfProductsOperationsGridLineList = DataSource.SelectContractOfProductsOperationsLines.OrderBy(t => t.LineNr).ToList();

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "amounts":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductsOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    var workOrdersList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductsOperationID == DataSource.Id).ToList();


                    foreach (var workorder in workOrdersList)
                    {
                        var productionOrderDataSource = (await ProductionOrdersAppService.GetAsync(workorder.ProductionOrderID.GetValueOrDefault())).Data;

                        if (AmountsbyProductionOrdersList.Where(t => t.ProductionOrderID == productionOrderDataSource.Id).Count() == 0)
                        {
                            AmountsbyProductionOrders amountsbyProductionOrdersModel = new AmountsbyProductionOrders
                            {
                                ProductionOrderID = productionOrderDataSource.Id,
                                CustomerOrderNr = productionOrderDataSource.CustomerOrderNo,
                                ProductionOrderFİcheNr = productionOrderDataSource.FicheNo,
                                SalesOrderFicheNr = productionOrderDataSource.OrderFicheNo,
                                SalesOrderID = productionOrderDataSource.OrderID.Value,
                                Amount = workOrdersList.Where(t => t.ProductionOrderID == productionOrderDataSource.Id).Count()
                            };

                            AmountsbyProductionOrdersList.Add(amountsbyProductionOrdersModel);
                            await InvokeAsync(StateHasChanged);
                        }

                    }

                    AmountsbyProductionOrdersPopup = true;
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

        public async void StationLineContextMenuClick(ContextMenuClickEventArgs<SelectProductsOperationLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    StationLineDataSource = new SelectProductsOperationLinesDto();
                    StationLineCrudPopup = true;
                    await StationLineBeforeInsertAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        StationLineDataSource = args.RowInfo.RowData;
                    StationLineCrudPopup = true;
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
                            DataSource.SelectProductsOperationLines.Remove(args.RowInfo.RowData);



                            await _LineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {

                                int selectedIndex = StationGridLineList.FindIndex(t => t.Id == line.Id);

                                if (selectedIndex >= 0)
                                {
                                    int selectedPriority = StationGridLineList[selectedIndex].Priority;


                                    StationGridLineList.Remove(line);

                                    for (int i = 0; i < StationGridLineList.Count; i++)
                                    {
                                        StationGridLineList[i].LineNr = i + 1;
                                        StationGridLineList[i].Priority = i + 1;
                                    }

                                    DataSource.SelectProductsOperationLines = StationGridLineList;

                                    await _LineGrid.Refresh();

                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    await GetListDataSourceAsync();
                                    await InvokeAsync(StateHasChanged);
                                }
                            }
                            else
                            {
                                DataSource.SelectProductsOperationLines.Remove(line);
                            }
                        }
                    }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideStationLinesPopup()
        {
            StationLineCrudPopup = false;
        }

        public void HideQualityPlanLinesPopup()
        {
            //QualityPlanLineCrudPopup = false;
        }

        public void HideAmountsbyProductionOrdersPopup()
        {
            AmountsbyProductionOrdersPopup = false;
        }


        protected async Task OnStationLineSubmit()
        {
            if (StationLineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectProductsOperationLines.Contains(StationLineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectProductsOperationLines.FindIndex(t => t.LineNr == StationLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectProductsOperationLines[selectedLineIndex] = StationLineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectProductsOperationLines.Add(StationLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectProductsOperationLines.FindIndex(t => t.Id == StationLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectProductsOperationLines[selectedLineIndex] = StationLineDataSource;
                }
            }

            StationGridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();

            await _LineGrid.Refresh();

            HideStationLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        public async void ArrowUpBtnClicked()
        {
            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == 0))
            {
                StationGridLineList[index].Priority -= 1;
                StationGridLineList[index - 1].Priority += 1;
                StationGridLineList[index].LineNr -= 1;
                StationGridLineList[index - 1].LineNr += 1;

                StationGridLineList = StationGridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectProductsOperationLines = StationGridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ArrowDownBtnClicked()
        {

            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == StationGridLineList.Count()))
            {
                StationGridLineList[index].Priority += 1;
                StationGridLineList[index + 1].Priority -= 1;
                StationGridLineList[index].LineNr += 1;
                StationGridLineList[index + 1].LineNr -= 1;

                StationGridLineList = StationGridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectProductsOperationLines = StationGridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetTemplateOperationsList()
        {
            TemplateOperationsList = (await TemplateOperationsAppService.GetListAsync(new ListTemplateOperationsParameterDto())).Data.ToList();
        }

        #endregion

        #region Fason Tedarikçi Grid İşlemler

        protected void CreateContractOfProductsOperationsContextMenuItems()
        {
            if (ContractOfProductsOperationsGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ContractOfProductsOperationContextSelectSupplier":
                                ContractOfProductsOperationsGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractOfProductsOperationContextSelectSupplier"], Id = "new" }); break;
                            case "ContractOfProductsOperationContextDelete":
                                ContractOfProductsOperationsGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContractOfProductsOperationContextDelete"], Id = "delete" }); break;
                            default: break;
                        }
                    }
                }
            }
        }


        public async void ContractOfProductsOperationsContextMenuClick(ContextMenuClickEventArgs<SelectContractOfProductsOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    SelectCurrentAccountCardsPopupVisible = true;
                    await GetCurrentAccountCardsList();
                    await InvokeAsync(StateHasChanged);
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
                            ContractOfProductsOperationsGridLineList.Remove(line);

                            for (int i = 0; i < ContractOfProductsOperationsGridLineList.Count; i++)
                            {
                                ContractOfProductsOperationsGridLineList[i].LineNr = i + 1;
                            }

                            DataSource.SelectContractOfProductsOperationsLines = ContractOfProductsOperationsGridLineList;

                            await _ContractOfProductsOperationsGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {

                                int selectedIndex = ContractOfProductsOperationsGridLineList.FindIndex(t => t.Id == line.Id);

                                if (selectedIndex >= 0)
                                {

                                    ContractOfProductsOperationsGridLineList.Remove(line);

                                    for (int i = 0; i < ContractOfProductsOperationsGridLineList.Count; i++)
                                    {
                                        ContractOfProductsOperationsGridLineList[i].LineNr = i + 1;
                                    }

                                    DataSource.SelectContractOfProductsOperationsLines = ContractOfProductsOperationsGridLineList;

                                    await _ContractOfProductsOperationsGrid.Refresh();


                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    //await GetListDataSourceAsync();
                                    await InvokeAsync(StateHasChanged);
                                }
                            }
                            else
                            {
                                ContractOfProductsOperationsGridLineList.Remove(line);

                                for (int i = 0; i < ContractOfProductsOperationsGridLineList.Count; i++)
                                {
                                    ContractOfProductsOperationsGridLineList[i].LineNr = i + 1;
                                }

                                DataSource.SelectContractOfProductsOperationsLines = ContractOfProductsOperationsGridLineList;

                                await _ContractOfProductsOperationsGrid.Refresh();
                                await InvokeAsync(StateHasChanged);
                            }
                        }
                    }
                    }
                    break;

                default:
                    break;
            }
        }


        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                ContractOfProductsOperationsGridLineList.Add(new SelectContractOfProductsOperationsDto
                {
                    CurrentAccountCardID = selectedUnitSet.Id,
                    CurrentAccountCardName = selectedUnitSet.Name,
                    ProductsOperationID = DataSource.Id,
                    LineNr = ContractOfProductsOperationsGridLineList.Count + 1
                });

                DataSource.SelectContractOfProductsOperationsLines = ContractOfProductsOperationsGridLineList;

                await _ContractOfProductsOperationsGrid.Refresh();

                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        public async override void HideEditPage()
        {
            StationGridLineList.Clear();
            ContractOfProductsOperationsGridLineList.Clear();
            EditPageVisible = false;
            await InvokeAsync(StateHasChanged);
        }

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ProdOperationsChildMenu");
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

