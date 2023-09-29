using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.ProductsOperation.Services;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlanningManagement.MRP
{
    public partial class MRPsListPage
    {
        private SfGrid<SelectMRPLinesDto> _LineGrid;
        private SfGrid<ListSalesOrderDto> _OrdersGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectMRPLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMRPLinesDto> GridLineList = new List<SelectMRPLinesDto>();

        List<ListSalesOrderDto> SalesOrdersList = new List<ListSalesOrderDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = MRPsService;
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region MRP Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMRPsDto()
            {
                Date_ = DateTime.Today,
                Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu")
            };
            await GetSalesOrdersList();
            DataSource.SelectMRPLines = new List<SelectMRPLinesDto>();
            GridLineList = DataSource.SelectMRPLines;
            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
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
                //LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDoNotCalculate"], Id = "dontcalculate" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDeleteOrderLines"], Id = "deleteorderlines" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextRefresh"], Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMRPsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    await GetSalesOrdersList();
                    DataSource = (await MRPsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMRPLines;

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectMRPLinesDto> args)
        {
            switch (args.Item.Id)
            {
                #region Delete Case
                //case "delete":

                //    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                //    if (res == true)
                //    {
                //        var line = args.RowInfo.RowData;

                //        if (line.Id == Guid.Empty)
                //        {
                //            DataSource.SelectMRPLines.Remove(args.RowInfo.RowData);
                //        }
                //        else
                //        {
                //            if (line != null)
                //            {
                //                await DeleteAsync(args.RowInfo.RowData.Id);
                //                DataSource.SelectMRPLines.Remove(line);
                //                await GetListDataSourceAsync();
                //            }
                //            else
                //            {
                //                DataSource.SelectMRPLines.Remove(line);
                //            }
                //        }

                //        await _LineGrid.Refresh();
                //        GetTotal();
                //        await InvokeAsync(StateHasChanged);
                //    }

                //    break;
                #endregion

                case "dontcalculate":
                    var line = args.RowInfo.RowData;
                    var selectedIndex = GridLineList.FindIndex(t => t.SalesOrderID == line.SalesOrderID && t.ProductID == line.ProductID);
                    if (selectedIndex >= 0)
                    {
                        GridLineList[selectedIndex].isCalculated = false;
                    }

                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "deleteorderlines":
                    var selectedSalesOrderId = args.RowInfo.RowData.SalesOrderID;

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationSalesOrderLineBase"]);

                    if (res == true)
                    {
                       var notdeletedList = GridLineList.Where(t=>t.SalesOrderID != selectedSalesOrderId).ToList();
                        GridLineList = notdeletedList;
                        await _LineGrid.Refresh();

                    }
                    await InvokeAsync(StateHasChanged);
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

        public async void ArrowRightBtnClicked()
        {

            if (SalesOrdersList.Count != 0)
            {
                ListSalesOrderDto selectedRow = new ListSalesOrderDto();
                List<SelectSalesOrderLinesDto> salesOrderLineList = new List<SelectSalesOrderLinesDto>();

                if (_OrdersGrid.SelectedRecords.Count > 0)
                {
                    selectedRow = _OrdersGrid.SelectedRecords[0];

                    if (selectedRow.Id != Guid.Empty)
                    {
                        salesOrderLineList = (await SalesOrdersAppService.GetAsync(selectedRow.Id)).Data.SelectSalesOrderLines.ToList();
                    }


                    foreach (var orderline in salesOrderLineList)
                    {
                        var product = (await ProductsAppService.GetAsync(orderline.ProductID.GetValueOrDefault())).Data;

                        //if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Satınalma)
                        //{
                        var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(orderline.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();



                        foreach (var bomline in bomLineList)
                        {
                            int calculatedAmount = Convert.ToInt32(orderline.Quantity * bomline.Quantity);

                            decimal amountofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == bomline.ProductID).Select(t => t.Amount).Sum();

                            SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                            {
                                Amount = calculatedAmount,
                                isCalculated = true,
                                isPurchase = false,
                                ProductID = bomline.ProductID.GetValueOrDefault(),
                                MRPID = DataSource.Id,
                                ProductCode = bomline.ProductCode,
                                ProductName = bomline.ProductName,
                                SalesOrderID = selectedRow.Id,
                                UnitSetID = bomline.UnitSetID,
                                LineNr = GridLineList.Count + 1,
                                UnitSetCode = bomline.UnitSetCode,
                                AmountOfStock = amountofProduct,
                                RequirementAmount = Math.Abs(Convert.ToInt32(amountofProduct) - calculatedAmount),
                                SalesOrderLineID = orderline.Id,
                                SalesOrderFicheNo = selectedRow.FicheNo,
                            };

                            GridLineList.Add(mrpLineModel);
                        }
                        //}
                    }

                }
                await _LineGrid.Refresh();
                await _OrdersGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }

        }

        private async Task GetSalesOrdersList()
        {
            SalesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
