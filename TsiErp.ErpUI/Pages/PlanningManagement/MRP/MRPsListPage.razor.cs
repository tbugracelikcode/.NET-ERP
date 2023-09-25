using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
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

        List<SelectMRPLinesDto> SelectedOrdersList = new List<SelectMRPLinesDto>();

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
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDelete"], Id = "delete" });
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

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectMRPLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectMRPLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectMRPLines.Remove(line);
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

        public async void OnSelectedOrdersAddButtonClicked()
        {
            foreach (var item in SelectedOrdersList)
            {
                SelectMRPLinesDto lineModel = new SelectMRPLinesDto
                {
                    SalesOrderID = item.SalesOrderID,
                    Amount = item.Amount,
                    LineNr = GridLineList.Count + 1,
                    UnitSetID = item.UnitSetID,
                    UnitSetCode = item.UnitSetCode,
                    SalesOrderLineID = item.SalesOrderLineID,
                    SalesOrderFicheNo = item.SalesOrderFicheNo,
                    ProductName = item.ProductName,
                    ProductID = item.ProductID,
                    ProductCode = item.ProductCode,
                    MRPID = item.MRPID,

                };

                GridLineList.Add(lineModel);
            }

            await  _LineGrid.Refresh();

            SelectSalesOrdersPopupVisible = false;

            SelectedOrdersList.Clear();

            await InvokeAsync(StateHasChanged);
        }


        #endregion

        #region Satış Siparişleri Button

        bool SelectSalesOrdersPopupVisible = false;
        List<ListSalesOrderDto> SalesOrdersList = new List<ListSalesOrderDto>();

        public async void SalesOrdersButtonClickEvent()
        {
            SelectedOrdersList.Clear();
            SelectSalesOrdersPopupVisible = true;
            await GetSalesOrdersList();
            await InvokeAsync(StateHasChanged);
        }

        public async void SalesOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListSalesOrderDto> args)
        {
            var selectedSalesOrder = args.RowData;

            if (SelectedOrdersList.Where(t => t.SalesOrderID == selectedSalesOrder.Id).Count() == 0)
            {
                var salesOrderLineList = (await SalesOrdersAppService.GetAsync(selectedSalesOrder.Id)).Data.SelectSalesOrderLines.ToList();

                foreach (var orderline in salesOrderLineList)
                {
                    var product = (await ProductsAppService.GetAsync(orderline.ProductID.GetValueOrDefault())).Data;

                    //if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Satınalma)
                    //{
                        var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(orderline.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                        foreach (var bomline in bomLineList)
                        {
                            SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                            {
                                Amount = Convert.ToInt32(orderline.Quantity * bomline.Quantity),
                                ProductID = bomline.ProductID.GetValueOrDefault(),
                                MRPID = DataSource.Id,
                                ProductCode = bomline.ProductCode,
                                ProductName = bomline.ProductName,
                                SalesOrderID = selectedSalesOrder.Id,
                                UnitSetID = bomline.UnitSetID,
                                UnitSetCode = bomline.UnitSetCode,
                                SalesOrderLineID = orderline.Id,
                                SalesOrderFicheNo = selectedSalesOrder.FicheNo,
                            };

                            SelectedOrdersList.Add(mrpLineModel);
                        }
                    //}
                }

            }

            else
            {
                var deletedOrdersList = SelectedOrdersList.Where(t => t.SalesOrderID != selectedSalesOrder.Id).ToList();

                SelectedOrdersList = deletedOrdersList;
            }

            await _OrdersGrid.Refresh();
        }

        public void RowBound(RowDataBoundEventArgs<ListSalesOrderDto> args)
        {
            if (SelectedOrdersList.Where(t=>t.SalesOrderID == args.Data.Id).Count() == 0)
            {
                args.Row.AddClass(new string[] { "notselectedOrder" });
            }
            else
            {
                args.Row.AddClass(new string[] { "selectedOrder" });
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
