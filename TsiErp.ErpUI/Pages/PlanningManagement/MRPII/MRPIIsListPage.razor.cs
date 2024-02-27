using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;
using Microsoft.AspNetCore.Components;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.DataAccess.Services.Login;
using Syncfusion.Blazor.Inputs;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;

namespace TsiErp.ErpUI.Pages.PlanningManagement.MRPII
{
    public partial class MRPIIsListPage : IDisposable
    {
        private SfGrid<SelectMRPIILinesDto> _LineGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectMRPIILinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMRPIILinesDto> GridLineList = new List<SelectMRPIILinesDto>();

        private bool LineCrudPopup = false;

        #region Planning Parameters

        int MRPIISourceModule;

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = MRPIIsService;
            _L = L;


            MRPIISourceModule = (await PlanningManagementParametersAppService.GetPlanningManagementParametersAsync()).Data.MRPIISourceModule;

            if (MRPIISourceModule == 1) // OrderAcceptanceRecords
            {
                await GetOrderAcceptances();
            }
            else if (MRPIISourceModule == 2) // SalesOrders
            {
                await GetSalesOrders();
            }

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "MRPIIChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region MRPII Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMRPIIsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("MRPIIChildMenu"),
                CalculationDate = DateTime.Today
            };

            DataSource.SelectMRPIILines = new List<SelectMRPIILinesDto>();
            GridLineList = DataSource.SelectMRPIILines;
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
                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
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
                            //case "MRPIILineContextChange":
                            //    LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIILineContextChange"], Id = "changed" }); break;
                            //case "MRPIILineContextDelete":
                            //    LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIILineContextDelete"], Id = "delete" }); break;
                            case "MRPIILineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIILineContextRefresh"], Id = "refresh" }); break;
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
                            case "MRPIIsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextAdd"], Id = "new" }); break;
                            case "MRPIIsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextChange"], Id = "changed" }); break;
                            case "MRPIIsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextDelete"], Id = "delete" }); break;
                            case "MRPIIsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMRPIIsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await MRPIIsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMRPIILines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectMRPIILinesDto> args)
        {
            switch (args.Item.Id)
            {
                #region Unused Part

                //case "changed":
                //    LineDataSource = args.RowInfo.RowData;
                //    LineCrudPopup = true;
                //    await InvokeAsync(StateHasChanged);

                //    break;
                //case "delete":

                //    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                //    if (res == true)
                //    {
                //        var line = args.RowInfo.RowData;

                //        if (line.Id == Guid.Empty)
                //        {
                //            DataSource.SelectMRPIILines.Remove(args.RowInfo.RowData);
                //        }
                //        else
                //        {
                //            if (line != null)
                //            {
                //                await DeleteAsync(args.RowInfo.RowData.Id);
                //                DataSource.SelectMRPIILines.Remove(line);
                //                await GetListDataSourceAsync();
                //            }
                //            else
                //            {
                //                DataSource.SelectMRPIILines.Remove(line);
                //            }
                //        }

                //        await _LineGrid.Refresh();
                //        GetTotal();
                //        await InvokeAsync(StateHasChanged);
                //    }

                //    break;

                #endregion

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
                if (DataSource.SelectMRPIILines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectMRPIILines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectMRPIILines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectMRPIILines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectMRPIILines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectMRPIILines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectMRPIILines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        public async void BringLinesButtonClicked()
        {
            if (MRPIISourceModule == 1) // OrderAcceptanceRecords
            {
                if (BindingOrderAcceptances == null)
                {
                    BindingOrderAcceptances = new List<Guid>();
                }
                else
                {
                    foreach (var id in BindingOrderAcceptances)
                    {
                        var orderAcceptance = (await OrderAcceptanceRecordsAppService.GetAsync(id)).Data;
                        var orderAcceptanceLineList = orderAcceptance.SelectOrderAcceptanceRecordLines.ToList();

                        if (orderAcceptanceLineList != null && orderAcceptanceLineList.Count > 0)
                        {
                            foreach (var orderAcceptanceLine in orderAcceptanceLineList)
                            {
                                var product = (await ProductsAppService.GetAsync(orderAcceptanceLine.ProductID.GetValueOrDefault())).Data;

                                if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Üretim)
                                {
                                    SelectMRPIILinesDto mrpIILineModel1 = new SelectMRPIILinesDto
                                    {
                                        ProductID = product.Id,
                                        ProductCode = product.Code,
                                        ProductName = product.Name,
                                        LinkedProductCode = string.Empty,
                                        LinkedProductID = Guid.Empty,
                                        LinkedProductName = string.Empty,
                                        LineNr = GridLineList.Count + 1,
                                        EstimatedProductionStartDate = DateTime.Now,
                                        EstimatedProductionEndDate = DateTime.Now,
                                        EstimatedPurchaseSupplyDate = DateTime.Now,
                                        SalesOrderID = Guid.Empty,
                                        SalesOrderNo = string.Empty,
                                        OrderAcceptanceID = orderAcceptance.Id,
                                        OrderAcceptanceNo = orderAcceptance.Code
                                    };

                                    GridLineList.Add(mrpIILineModel1);

                                    var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(product.Id)).Data.SelectBillsofMaterialLines.Where(t => t.ProductSupplyType == 2).ToList();

                                    if (bomLineList != null && bomLineList.Count > 0)
                                    {

                                        foreach (var bomLine in bomLineList)
                                        {
                                            var bomProduct = (await ProductsAppService.GetAsync(bomLine.ProductID.GetValueOrDefault())).Data;

                                            SelectMRPIILinesDto mrpIILineModel2 = new SelectMRPIILinesDto
                                            {
                                                ProductID = bomProduct.Id,
                                                ProductCode = bomProduct.Code,
                                                ProductName = bomProduct.Name,
                                                LinkedProductCode = product.Code,
                                                LinkedProductID = product.Id,
                                                LinkedProductName = product.Name,
                                                LineNr = GridLineList.Count + 1,
                                                EstimatedProductionStartDate = orderAcceptanceLine.PurchaseSupplyDate.GetValueOrDefault(),
                                                EstimatedProductionEndDate = DateTime.Now,
                                                EstimatedPurchaseSupplyDate = orderAcceptanceLine.PurchaseSupplyDate.GetValueOrDefault(),
                                                SalesOrderID = Guid.Empty,
                                                SalesOrderNo = string.Empty,
                                                OrderAcceptanceID = orderAcceptance.Id,
                                                OrderAcceptanceNo = orderAcceptance.Code

                                            };

                                            GridLineList.Add(mrpIILineModel2);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    await _LineGrid.Refresh();
                }
            }

            else if (MRPIISourceModule == 2) // SalesOrders
            {
                if (BindingSalesOrders == null)
                {
                    BindingSalesOrders = new List<Guid>();
                }
                else
                {
                    foreach (var id in BindingSalesOrders)
                    {
                        var salesOrder = (await SalesOrdersAppService.GetAsync(id)).Data;
                        var salesOrderLineList = salesOrder.SelectSalesOrderLines.Where(t => t.SalesOrderLineStateEnum == Entities.Enums.SalesOrderLineStateEnum.Beklemede).ToList();

                        if (salesOrderLineList != null && salesOrderLineList.Count > 0)
                        {
                            foreach (var salesOrderLine in salesOrderLineList)
                            {
                                var product = (await ProductsAppService.GetAsync(salesOrderLine.ProductID.GetValueOrDefault())).Data;

                                if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Üretim)
                                {
                                    SelectMRPIILinesDto mrpIILineModel1 = new SelectMRPIILinesDto
                                    {
                                        ProductID = product.Id,
                                        ProductCode = product.Code,
                                        ProductName = product.Name,
                                        LinkedProductCode = string.Empty,
                                        LinkedProductID = Guid.Empty,
                                        LinkedProductName = string.Empty,
                                        LineNr = GridLineList.Count + 1,
                                        EstimatedProductionStartDate = DateTime.Now,
                                        EstimatedProductionEndDate = DateTime.Now,
                                        EstimatedPurchaseSupplyDate = DateTime.Now,
                                        SalesOrderID = salesOrder.Id,
                                        SalesOrderNo = salesOrder.FicheNo,
                                        OrderAcceptanceID = Guid.Empty,
                                        OrderAcceptanceNo = string.Empty

                                    };

                                    GridLineList.Add(mrpIILineModel1);

                                    var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(product.Id)).Data.SelectBillsofMaterialLines.Where(t => t.ProductSupplyType == 2).ToList();

                                    if (bomLineList != null && bomLineList.Count > 0)
                                    {
                                        DateTime? biggestDate = null;

                                        foreach (var bomLine in bomLineList)
                                        {
                                            var bomProduct = (await ProductsAppService.GetAsync(bomLine.ProductID.GetValueOrDefault())).Data;

                                            SelectMRPIILinesDto mrpIILineModel2 = new SelectMRPIILinesDto
                                            {
                                                ProductID = bomProduct.Id,
                                                ProductCode = bomProduct.Code,
                                                ProductName = bomProduct.Name,
                                                LinkedProductCode = product.Code,
                                                LinkedProductID = product.Id,
                                                LinkedProductName = product.Name,
                                                LineNr = GridLineList.Count + 1,
                                                EstimatedProductionStartDate = salesOrderLine.PurchaseSupplyDate.GetValueOrDefault(),
                                                EstimatedProductionEndDate = DateTime.Now,
                                                EstimatedPurchaseSupplyDate = salesOrderLine.PurchaseSupplyDate.GetValueOrDefault(),
                                                SalesOrderID = salesOrder.Id,
                                                SalesOrderNo = salesOrder.FicheNo,
                                                OrderAcceptanceID = Guid.Empty,
                                                OrderAcceptanceNo = string.Empty

                                            };

                                            GridLineList.Add(mrpIILineModel2);

                                            if (mrpIILineModel2.EstimatedPurchaseSupplyDate > biggestDate)
                                            {
                                                biggestDate = mrpIILineModel2.EstimatedPurchaseSupplyDate;

                                                int indexFinishedProduct = GridLineList.IndexOf(mrpIILineModel1);

                                                GridLineList[indexFinishedProduct].EstimatedProductionStartDate = biggestDate.GetValueOrDefault();

                                                GridLineList[indexFinishedProduct].EstimatedPurchaseSupplyDate = biggestDate.GetValueOrDefault();
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    await _LineGrid.Refresh();
                }

            }

        }

        #region OrderAcceptances MultiSelect

        List<ListOrderAcceptanceRecordsDto> OrderAcceptances = new List<ListOrderAcceptanceRecordsDto>();
        List<Guid> BindingOrderAcceptances = new List<Guid>();

        private async Task GetOrderAcceptances()
        {
            OrderAcceptances = (await OrderAcceptanceRecordsAppService.GetListAsync(new ListOrderAcceptanceRecordsParameterDto())).Data.Where(t => t.OrderAcceptanceRecordState == Entities.Enums.OrderAcceptanceRecordStateEnum.SiparisFiyatOnayiVerildi).ToList();
        }

        #endregion

        #region SalesOrders MultiSelect

        List<ListSalesOrderDto> SalesOrders = new List<ListSalesOrderDto>();
        List<Guid> BindingSalesOrders = new List<Guid>();

        private async Task GetSalesOrders()
        {
            SalesOrders = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.Where(t => t.SalesOrderStateEnum == Entities.Enums.SalesOrderStateEnum.Beklemede).ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MRPIIChildMenu");
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
