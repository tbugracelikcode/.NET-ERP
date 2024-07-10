using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlanningManagement.ShipmentPlanningList
{
    public partial class ShipmentPlanningListsListPage : IDisposable
    {
        private SfGrid<SelectShipmentPlanningLinesDto> _LineGrid;
        private SfGrid<ListProductionOrdersDto> _ProductionOrdersGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectShipmentPlanningLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductionOrderGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectShipmentPlanningLinesDto> GridLineList = new List<SelectShipmentPlanningLinesDto>();

        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        private bool LineCrudPopup = false;

        DateTime filterStartDate = DateTime.Today;
        DateTime filterEndDate = DateTime.Today;

        protected override async void OnInitialized()
        {
            BaseCrudService = ShipmentPlanningsService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ShipmentPlanningChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateProductionOrderContextMenuItems();

        }

        #region ShipmentPlanning Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectShipmentPlanningsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("ShipmentPlanningChildMenu"),
                ShipmentPlanningDate= GetSQLDateAppService.GetDateFromSQL()
            };

            DataSource.SelectShipmentPlanningLines = new List<SelectShipmentPlanningLinesDto>();
            GridLineList = DataSource.SelectShipmentPlanningLines;
            EditPageVisible = true;


            await Task.CompletedTask;
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
                            case "ShipmentPlanningLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningLineContextChange"], Id = "changed" }); break;
                            case "ShipmentPlanningLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningLineContextDelete"], Id = "delete" }); break;
                            case "ShipmentPlanningLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateProductionOrderContextMenuItems()
        {
            if (ProductionOrderGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ProductionOrderContextAddtoShipmentPlanning":
                                ProductionOrderGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextAddtoShipmentPlanning"], Id = "addtoplanning" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ShipmentPlanningsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningsContextAdd"], Id = "new" }); break;
                            case "ShipmentPlanningsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningsContextChange"], Id = "changed" }); break;
                            case "ShipmentPlanningsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningsContextDelete"], Id = "delete" }); break;
                            case "ShipmentPlanningsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListShipmentPlanningsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    ProductionOrdersList.Clear();
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    ProductionOrdersList.Clear();
                    IsChanged = true;
                    DataSource = (await ShipmentPlanningsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectShipmentPlanningLines;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectShipmentPlanningLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);

                    break;
                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectShipmentPlanningLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectShipmentPlanningLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectShipmentPlanningLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
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


        public async void OnProductionOrderContextMenuClick(ContextMenuClickEventArgs<ListProductionOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "addtoplanning":
                    {
                        var line = args.RowInfo.RowData;

                        if (GridLineList.Any(t => t.ProductionOrderID == line.Id))
                        {
                            await ModalManager.WarningPopupAsync(L["UIProductionOrderTitle"], L["UIProductionOrderMessage"]);
                        }
                        else
                        {
                            var result = (await ShipmentPlanningsService.GetLinebyProductionOrderAsync(line.Id)).Data;
                            if (result.Id != Guid.Empty)
                            {
                                await ModalManager.WarningPopupAsync(L["UIProductionOrderTitle"], L["UIProductionOrderMessage"]);
                            }
                            else
                            {
                                var product = (await ProductsAppService.GetAsync(line.FinishedProductID.GetValueOrDefault())).Data;

                                var salesOrder = (await SalesOrderAppService.GetAsync(line.OrderID.GetValueOrDefault())).Data;

                                decimal netKG = line.PlannedQuantity * product.UnitWeight;

                                decimal grossKG = netKG + ((netKG * 8) / 100);

                                SelectShipmentPlanningLinesDto planningLineModel = new SelectShipmentPlanningLinesDto
                                {
                                    ProductionOrderID = line.Id,
                                    CustomerOrderNr = line.CustomerOrderNo,
                                    LineNr = GridLineList.Count + 1,
                                    NetWeightKG = netKG,
                                    GrossWeightKG = grossKG,
                                    PlannedQuantity = line.PlannedQuantity,
                                    UnitWeightKG = product.UnitWeight,
                                    ShipmentQuantity = 0,
                                    ShipmentPlanningID = Guid.Empty,
                                    LineDescription_ = string.Empty,
                                    SentQuantity = 0,
                                    SalesOrderID = line.OrderID,
                                    RequestedLoadingDate = salesOrder.CustomerRequestedDate,
                                    ProductID = product.Id,
                                    ProductCode = product.Code,
                                };

                                GridLineList.Add(planningLineModel);
                                GridLineList = DataSource.SelectShipmentPlanningLines;

                                DataSource.TotalNetKG = GridLineList.Select(t => t.NetWeightKG).Sum();
                                DataSource.TotalGrossKG = GridLineList.Select(t => t.GrossWeightKG).Sum();
                                DataSource.TotalAmount = Convert.ToInt32(GridLineList.Select(t=>t.ShipmentQuantity).Sum());
                                DataSource.PlannedAmount = Convert.ToInt32(GridLineList.Select(t=>t.PlannedQuantity).Sum());
                            }
                            
                        }


                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
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

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectShipmentPlanningLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectShipmentPlanningLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectShipmentPlanningLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectShipmentPlanningLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectShipmentPlanningLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectShipmentPlanningLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectShipmentPlanningLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        public async void FilterButtonClicked()
        {
            if(filterStartDate.Year != 1900 && filterEndDate.Year != 1900)
            {
                ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.Date_ >= filterStartDate && t.Date_ <= filterEndDate && t.ProductionOrderState == Entities.Enums.ProductionOrderStateEnum.Baslamadi).ToList();

                await _ProductionOrdersGrid.Refresh();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ShipmentPlanningChildMenu");
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
