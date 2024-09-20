using AutoMapper.Internal.Mappers;
using DevExpress.Blazor.Internal.ComponentStructureHelpers;
using DevExpress.Office.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlanningManagement.ShipmentPlanningList
{
    public partial class ShipmentPlanningListsListPage : IDisposable
    {
        private SfGrid<SelectShipmentPlanningLinesDto> _LineGrid;
        private SfGrid<SelectShipmentPlanningLinesDto> _CalculateGrid;
        private SfGrid<ListProductionOrdersDto> _ProductionOrdersGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }

        SelectShipmentPlanningLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductionOrderGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectShipmentPlanningLinesDto> GridLineList = new List<SelectShipmentPlanningLinesDto>();

        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();


        private bool LineCrudPopup = false;
        public bool CalculateModalVisible = false;

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
            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            DataSource = new SelectShipmentPlanningsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("ShipmentPlanningChildMenu"),
                ShipmentPlanningDate = today,
                PlannedLoadingTime = today,
            };

            DataSource.SelectShipmentPlanningLines = new List<SelectShipmentPlanningLinesDto>();
            GridLineList = DataSource.SelectShipmentPlanningLines;

            filterStartDate = today;
            filterEndDate = today;

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
            if (GridContextMenu.Count == 0)
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
                            case "ShipmentPlanningsContextStationOccupancy":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShipmentPlanningsContextStationOccupancy"], Id = "staoccupancy" }); break;
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
                    if (args.RowInfo.RowData != null)
                    {

                        ProductionOrdersList.Clear();
                    IsChanged = true;
                    DataSource = (await ShipmentPlanningsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectShipmentPlanningLines;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
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

                case "staoccupancy":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ShipmentPlanningsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectShipmentPlanningLines;
                    CalculateModalVisible = true;
                    }

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
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    }

                    break;
                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

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
                    if (args.RowInfo.RowData != null)
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
                                    RequestedLoadingDate = salesOrder.CustomerRequestedDate.GetValueOrDefault(),
                                    ProductID = product.Id,
                                    ProductCode = product.Code,
                                    LinkedProductionOrderID = line.LinkedProductionOrderID.GetValueOrDefault(),
                                    ProductType = product.ProductType
                                };

                                GridLineList.Add(planningLineModel);
                                GridLineList = DataSource.SelectShipmentPlanningLines;

                                DataSource.TotalNetKG = GridLineList.Select(t => t.NetWeightKG).Sum();
                                DataSource.TotalGrossKG = GridLineList.Select(t => t.GrossWeightKG).Sum();
                                DataSource.TotalAmount = Convert.ToInt32(GridLineList.Select(t => t.ShipmentQuantity).Sum());
                                DataSource.PlannedAmount = Convert.ToInt32(GridLineList.Select(t => t.PlannedQuantity).Sum());
                            }

                        }


                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
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
            if (filterStartDate.Year != 1900 && filterEndDate.Year != 1900)
            {
                ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.Date_ >= filterStartDate && t.Date_ <= filterEndDate && t.ProductionOrderState == Entities.Enums.ProductionOrderStateEnum.Baslamadi).ToList();

                await _ProductionOrdersGrid.Refresh();
            }
        }

        public async void CalculateButtonClicked()
        {
            SpinnerService.Show();
            await Task.Delay(100);

            List<SelectWorkOrdersDto> workOrdersList = new List<SelectWorkOrdersDto>();

            DateTime purchaseStartDate = DateTime.Now;
            DateTime stationFreeDate = DateTime.Now;

            foreach (var line in GridLineList.Where(t => t.ProductType == Entities.Enums.ProductTypeEnum.YM).ToList())
            {
                var productionOrder = (await ProductionOrdersAppService.GetAsync(line.ProductionOrderID.GetValueOrDefault())).Data;

                if (productionOrder.Id != Guid.Empty && productionOrder != null)
                {
                    #region Başlangıç Tarihi

                    #region İstasyon Boşa Çıkma Tarihi Bulma

                    workOrdersList = (await WorkOrdersAppService.GetSelectListbyProductionOrderAsync(productionOrder.Id)).Data.ToList();

                    var workOrder = workOrdersList.FirstOrDefault();

                    var stationId = workOrder.StationID.GetValueOrDefault();

                    if (stationId != Guid.Empty)
                    {
                        var stationOccupancy = (await StationOccupanciesAppService.GetbyStationAsync(stationId)).Data;

                        if (stationOccupancy != null && stationOccupancy.Id != Guid.Empty)
                        {
                            stationFreeDate = (await StationOccupanciesAppService.GetbyStationAsync(stationId)).Data.FreeDate.GetValueOrDefault();
                        }
                        else
                        {
                            stationFreeDate = (await StationsAppService.GetAsync(stationId)).Data.EndDate;
                        }
                    }


                    #endregion

                    #region Reçete Satırlarından Sipariş Bulma

                    var billsofMaterial = (await BillsofMaterialsAppService.GetAsync(productionOrder.BOMID.Value)).Data;

                    if (billsofMaterial != null && billsofMaterial.Id != Guid.Empty && billsofMaterial.SelectBillsofMaterialLines != null && billsofMaterial.SelectBillsofMaterialLines.Count > 0)
                    {
                        foreach (var bomLine in billsofMaterial.SelectBillsofMaterialLines)
                        {
                            var purchaseOrderLine = (await PurchaseOrdersAppService.GetLinebyProductandProductionOrderAsync(bomLine.ProductID.Value, productionOrder.Id)).Data;

                            if (purchaseOrderLine != null && purchaseOrderLine.Id != Guid.Empty) // Varsa satırın termin tarihi alınacak
                            {
                                purchaseStartDate = purchaseOrderLine.SupplyDate.Value;
                            }
                            else
                            {
                                var purchaseOrderLineWithoutProductionOrder = (await PurchaseOrdersAppService.GetLineListAsync()).Data.Where(t => t.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Beklemede && t.ProductID == bomLine.ProductID).ToList();

                                if (purchaseOrderLineWithoutProductionOrder.Count > 0) // Açık 1 adet sipariş varsa satırın termin tarihi alınacak
                                {
                                    purchaseStartDate = purchaseOrderLineWithoutProductionOrder.Min(t => t.SupplyDate.Value).Date;
                                }
                                else 
                                {
                                    purchaseStartDate = DataSource.ShipmentPlanningDate;
                                }
                            }
                        }

                    }

                    #endregion

                    #region Satın Alma Termin Tarihi ve İstasyon Boşa Çıkma Tarihi Karşılaştırması

                    if (stationFreeDate > purchaseStartDate)
                    {
                        line.PlannedStartDate = stationFreeDate;
                    }
                    else if (stationFreeDate < purchaseStartDate)
                    {
                        line.PlannedStartDate = purchaseStartDate;
                    }
                    else if (stationFreeDate == purchaseStartDate)
                    {
                        if (DataSource.ShipmentPlanningDate == stationFreeDate)
                        {
                            line.PlannedStartDate = DataSource.ShipmentPlanningDate;
                        }
                        else
                        {
                            line.PlannedStartDate = stationFreeDate;
                        }
                    }

                    line.PlannedStartDate = new DateTime(line.PlannedStartDate.Value.Year, line.PlannedStartDate.Value.Month, line.PlannedStartDate.Value.Day);

                    #endregion

                    #endregion

                    #region Bitiş Tarihi

                    DateTime date = line.PlannedStartDate.GetValueOrDefault();
                    DateTime stationFirstDate = date;
                    decimal dailyAvailableTime = 0;
                    decimal remainder = 0;
                    decimal firstDayRemainder = 0;

                    foreach (var item in workOrdersList)
                    {
                        firstDayRemainder = (remainder * -1);
                        stationFirstDate = date;

                        var productOperation = (await ProductsOperationsAppService.GetAsync(item.ProductsOperationID.GetValueOrDefault())).Data;

                        if (productOperation != null && productOperation.Id != Guid.Empty && productOperation.SelectProductsOperationLines.Count > 0)
                        {
                            var OperationLine = productOperation.SelectProductsOperationLines.Where(t => t.StationID == item.StationID).FirstOrDefault();

                            decimal totalOprTime = OperationLine.AdjustmentAndControlTime + (OperationLine.OperationTime * item.PlannedQuantity);

                            remainder = totalOprTime;

                            for (decimal i = remainder; remainder > 0; i++)
                            {

                                if (firstDayRemainder > 0)
                                {
                                    dailyAvailableTime = firstDayRemainder;
                                }
                                else
                                {
                                    dailyAvailableTime = (await CalendarsAppService.GetLinebyStationDateAsync(stationId, date)).Data.AvailableTime;
                                    //dailyAvailableTime = 30000;
                                }

                                remainder = remainder - dailyAvailableTime;

                                if (remainder >= 0)
                                {
                                    date = date.AddDays(1);

                                    firstDayRemainder = 0;
                                }
                                else
                                {
                                    var stationOccupaciesDataSource = (await StationOccupanciesAppService.GetbyStationAsync(item.StationID.GetValueOrDefault())).Data;

                                    if (stationOccupaciesDataSource != null && stationOccupaciesDataSource.Id != Guid.Empty) // Update
                                    {

                                        var previousLine = stationOccupaciesDataSource.SelectStationOccupancyLines.Where(t => t.ShipmentPlanningID == DataSource.Id);

                                        if (previousLine.Count() > 0)
                                        {
                                            int lineIndex = stationOccupaciesDataSource.SelectStationOccupancyLines.IndexOf(previousLine.FirstOrDefault());

                                            stationOccupaciesDataSource.SelectStationOccupancyLines[lineIndex].PlannedStartDate = stationFirstDate;
                                            stationOccupaciesDataSource.SelectStationOccupancyLines[lineIndex].PlannedEndDate = date;
                                        }
                                        else
                                        {
                                            SelectStationOccupancyLinesDto occupancyLineModel = new SelectStationOccupancyLinesDto
                                            {
                                                LineNr = stationOccupaciesDataSource.SelectStationOccupancyLines.Count + 1,
                                                PlannedStartDate = stationFirstDate,
                                                PlannedEndDate = date,
                                                ProductionOrderID = productionOrder.Id,
                                                ProductsOperationID = productOperation.Id,
                                                ShipmentPlanningID = DataSource.Id,
                                                WorkOrderID = workOrder.Id,
                                            };

                                            stationOccupaciesDataSource.SelectStationOccupancyLines.Add(occupancyLineModel);
                                        }

                                        stationOccupaciesDataSource.FreeDate = date;

                                        var updatedOccupancy = ObjectMapper.Map<SelectStationOccupanciesDto, UpdateStationOccupanciesDto>(stationOccupaciesDataSource);

                                        await StationOccupanciesAppService.UpdateAsync(updatedOccupancy);
                                    }
                                    else // Insert
                                    {
                                        stationOccupaciesDataSource = new SelectStationOccupanciesDto();
                                        stationOccupaciesDataSource.SelectStationOccupancyLines = new List<SelectStationOccupancyLinesDto>();

                                        SelectStationOccupancyLinesDto occupancyLineModel = new SelectStationOccupancyLinesDto
                                        {
                                            LineNr = stationOccupaciesDataSource.SelectStationOccupancyLines.Count + 1,
                                            PlannedStartDate = stationFirstDate,
                                            PlannedEndDate = date,
                                            ProductionOrderID = productionOrder.Id,
                                            ProductsOperationID = productOperation.Id,
                                            ShipmentPlanningID = DataSource.Id,
                                            WorkOrderID = workOrder.Id,
                                        };

                                        stationOccupaciesDataSource.SelectStationOccupancyLines.Add(occupancyLineModel);

                                        stationOccupaciesDataSource.StationID = item.StationID.Value;
                                        stationOccupaciesDataSource.FreeDate = date;

                                        var createOccupancy = ObjectMapper.Map<SelectStationOccupanciesDto, CreateStationOccupanciesDto>(stationOccupaciesDataSource);

                                        await StationOccupanciesAppService.CreateAsync(createOccupancy);

                                    }

                                    var stationOccHistoryList = (await StationOccupancyHistoriesAppService.GetListAsync(new ListStationOccupancyHistoriesParameterDto())).Data.Where(t => t.ShipmentPlanningID == DataSource.Id && t.StationID == item.StationID.Value).ToList();

                                    if (stationOccHistoryList != null && stationOccHistoryList.Count() > 0)
                                    {
                                        foreach (var stationHistory in stationOccHistoryList)
                                        {
                                            await StationOccupancyHistoriesAppService.DeleteAsync(stationHistory.Id);
                                        }
                                    }

                                    CreateStationOccupancyHistoriesDto occupancyHistoryModel = new CreateStationOccupancyHistoriesDto
                                    {
                                        FreeDate = date,
                                        ShipmentPlanningID = DataSource.Id,
                                        StationID = item.StationID.Value,
                                    };

                                    await StationOccupancyHistoriesAppService.CreateAsync(occupancyHistoryModel);

                                    break;
                                }
                            }



                        }
                    }

                    line.PlannedEndDate = date;

                    #endregion
                }

            }

            foreach (var line in GridLineList.Where(t => t.ProductType == Entities.Enums.ProductTypeEnum.MM).ToList())
            {
                var productionOrder = (await ProductionOrdersAppService.GetAsync(line.ProductionOrderID.GetValueOrDefault())).Data;

                if (productionOrder.Id != Guid.Empty && productionOrder != null)
                {
                    #region Başlangıç Tarihi


                    #region İstasyon Boşa Çıkma Tarihi Bulma

                    workOrdersList = (await WorkOrdersAppService.GetSelectListbyProductionOrderAsync(productionOrder.Id)).Data.ToList();

                    var workOrder = workOrdersList.FirstOrDefault();

                    var stationId = workOrder.StationID.GetValueOrDefault();

                    if (stationId != Guid.Empty)
                    {
                        var stationOccupancy = (await StationOccupanciesAppService.GetbyStationAsync(stationId)).Data;

                        if (stationOccupancy != null && stationOccupancy.Id != Guid.Empty)
                        {
                            stationFreeDate = (await StationOccupanciesAppService.GetbyStationAsync(stationId)).Data.FreeDate.GetValueOrDefault();
                        }
                        else
                        {
                            stationFreeDate = (await StationsAppService.GetAsync(stationId)).Data.EndDate;
                        }
                    }


                    #endregion

                    #region Reçete Satırlarından Sipariş Bulma

                    var billsofMaterial = (await BillsofMaterialsAppService.GetAsync(productionOrder.BOMID.Value)).Data;

                    if (billsofMaterial != null && billsofMaterial.Id != Guid.Empty && billsofMaterial.SelectBillsofMaterialLines != null && billsofMaterial.SelectBillsofMaterialLines.Count > 0)
                    {
                        foreach (var bomLine in billsofMaterial.SelectBillsofMaterialLines)
                        {
                            var purchaseOrderLine = (await PurchaseOrdersAppService.GetLinebyProductandProductionOrderAsync(bomLine.ProductID.Value, productionOrder.Id)).Data;

                            if (purchaseOrderLine != null && purchaseOrderLine.Id != Guid.Empty) // Varsa satırın termin tarihi alınacak
                            {
                                purchaseStartDate = purchaseOrderLine.SupplyDate.Value;
                            }
                            else
                            {
                                var purchaseOrderLineWithoutProductionOrder = (await PurchaseOrdersAppService.GetLineListAsync()).Data.Where(t => t.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Beklemede && t.ProductID == bomLine.ProductID).ToList();

                                if (purchaseOrderLineWithoutProductionOrder.Count > 0) // Açık 1 adet sipariş varsa satırın termin tarihi alınacak
                                {
                                    purchaseStartDate = purchaseOrderLineWithoutProductionOrder.Min(t => t.SupplyDate.Value).Date;
                                }
                                else
                                {
                                    purchaseStartDate = DataSource.ShipmentPlanningDate;
                                }
                            }
                        }

                    }

                    #endregion

                    #region Satın Alma Termin Tarihi ve İstasyon Boşa Çıkma Tarihi Karşılaştırması

                    var semiProductList = GridLineList.Where(t => t.LinkedProductionOrderID == productionOrder.Id).ToList();

                    DateTime? latestSemiProductEndDate = new DateTime(1 - 1 - 1900);

                    if (semiProductList != null && semiProductList.Count > 0)
                    {
                        latestSemiProductEndDate = semiProductList.Max(t => t.PlannedEndDate);
                    }

                    if (stationFreeDate > purchaseStartDate)
                    {
                        line.PlannedStartDate = stationFreeDate;
                    }
                    else if (stationFreeDate < purchaseStartDate)
                    {
                        line.PlannedStartDate = purchaseStartDate;
                    }
                    else if (stationFreeDate == purchaseStartDate)
                    {
                        if (DataSource.ShipmentPlanningDate == stationFreeDate)
                        {
                            line.PlannedStartDate = DataSource.ShipmentPlanningDate;
                        }
                        else
                        {
                            line.PlannedStartDate = stationFreeDate;
                        }
                    }

                    if(line.PlannedStartDate < latestSemiProductEndDate)
                    {
                        line.PlannedStartDate = latestSemiProductEndDate;
                    }

                    line.PlannedStartDate = new DateTime(line.PlannedStartDate.Value.Year, line.PlannedStartDate.Value.Month, line.PlannedStartDate.Value.Day);

                    #endregion

                    #endregion

                    #region Bitiş Tarihi

                    DateTime date = line.PlannedStartDate.GetValueOrDefault();
                    DateTime stationFirstDate = date;
                    decimal dailyAvailableTime = 0;
                    decimal remainder = 0;
                    decimal firstDayRemainder = 0;

                    foreach (var item in workOrdersList)
                    {
                        firstDayRemainder = (remainder * -1);
                        stationFirstDate = date;

                        var productOperation = (await ProductsOperationsAppService.GetAsync(item.ProductsOperationID.GetValueOrDefault())).Data;

                        if (productOperation != null && productOperation.Id != Guid.Empty && productOperation.SelectProductsOperationLines.Count > 0)
                        {
                            var OperationLine = productOperation.SelectProductsOperationLines.Where(t => t.StationID == item.StationID).FirstOrDefault();

                            decimal totalOprTime = OperationLine.AdjustmentAndControlTime + (OperationLine.OperationTime * item.PlannedQuantity);

                            remainder = totalOprTime;

                            for (decimal i = remainder; remainder > 0; i++)
                            {

                                if (firstDayRemainder > 0)
                                {
                                    dailyAvailableTime = firstDayRemainder;
                                }
                                else
                                {
                                    dailyAvailableTime = (await CalendarsAppService.GetLinebyStationDateAsync(item.StationID.GetValueOrDefault(), date)).Data.AvailableTime;
                                    //dailyAvailableTime = 30000;
                                }

                                remainder = remainder - dailyAvailableTime;

                                if (remainder >= 0)
                                {
                                    date = date.AddDays(1);

                                    firstDayRemainder = 0;
                                }
                                else
                                {
                                    var stationOccupaciesDataSource = (await StationOccupanciesAppService.GetbyStationAsync(item.StationID.GetValueOrDefault())).Data;

                                    if (stationOccupaciesDataSource != null && stationOccupaciesDataSource.Id != Guid.Empty) // Update
                                    {

                                        var previousLine = stationOccupaciesDataSource.SelectStationOccupancyLines.Where(t => t.ShipmentPlanningID == DataSource.Id);

                                        if (previousLine.Count() > 0)
                                        {
                                            int lineIndex = stationOccupaciesDataSource.SelectStationOccupancyLines.IndexOf(previousLine.FirstOrDefault());

                                            stationOccupaciesDataSource.SelectStationOccupancyLines[lineIndex].PlannedStartDate = stationFirstDate;
                                            stationOccupaciesDataSource.SelectStationOccupancyLines[lineIndex].PlannedEndDate = date;
                                        }
                                        else
                                        {
                                            SelectStationOccupancyLinesDto occupancyLineModel = new SelectStationOccupancyLinesDto
                                            {
                                                LineNr = stationOccupaciesDataSource.SelectStationOccupancyLines.Count + 1,
                                                PlannedStartDate = stationFirstDate,
                                                PlannedEndDate = date,
                                                ProductionOrderID = productionOrder.Id,
                                                ProductsOperationID = productOperation.Id,
                                                ShipmentPlanningID = DataSource.Id,
                                                WorkOrderID = item.Id,
                                            };

                                            stationOccupaciesDataSource.SelectStationOccupancyLines.Add(occupancyLineModel);
                                        }

                                        stationOccupaciesDataSource.FreeDate = date;

                                        var updatedOccupancy = ObjectMapper.Map<SelectStationOccupanciesDto, UpdateStationOccupanciesDto>(stationOccupaciesDataSource);

                                        await StationOccupanciesAppService.UpdateAsync(updatedOccupancy);
                                    }
                                    else // Insert
                                    {
                                        stationOccupaciesDataSource = new SelectStationOccupanciesDto();
                                        stationOccupaciesDataSource.SelectStationOccupancyLines = new List<SelectStationOccupancyLinesDto>();

                                        SelectStationOccupancyLinesDto occupancyLineModel = new SelectStationOccupancyLinesDto
                                        {
                                            LineNr = stationOccupaciesDataSource.SelectStationOccupancyLines.Count + 1,
                                            PlannedStartDate = stationFirstDate,
                                            PlannedEndDate = date,
                                            ProductionOrderID = productionOrder.Id,
                                            ProductsOperationID = productOperation.Id,
                                            ShipmentPlanningID = DataSource.Id,
                                            WorkOrderID = item.Id,
                                        };

                                        stationOccupaciesDataSource.SelectStationOccupancyLines.Add(occupancyLineModel);

                                        stationOccupaciesDataSource.StationID = item.StationID.Value;
                                        stationOccupaciesDataSource.FreeDate = date;

                                        var createOccupancy = ObjectMapper.Map<SelectStationOccupanciesDto, CreateStationOccupanciesDto>(stationOccupaciesDataSource);

                                        await StationOccupanciesAppService.CreateAsync(createOccupancy);

                                    }

                                    var stationOccHistoryList = (await StationOccupancyHistoriesAppService.GetListAsync(new ListStationOccupancyHistoriesParameterDto())).Data.Where(t => t.ShipmentPlanningID == DataSource.Id && t.StationID == item.StationID.Value).ToList();

                                    if (stationOccHistoryList != null && stationOccHistoryList.Count() > 0)
                                    {
                                        foreach (var stationHistory in stationOccHistoryList)
                                        {
                                            await StationOccupancyHistoriesAppService.DeleteAsync(stationHistory.Id);
                                        }
                                    }

                                    CreateStationOccupancyHistoriesDto occupancyHistoryModel = new CreateStationOccupancyHistoriesDto
                                    {
                                        FreeDate = date,
                                        ShipmentPlanningID = DataSource.Id,
                                        StationID = item.StationID.Value,
                                    };

                                    await StationOccupancyHistoriesAppService.CreateAsync(occupancyHistoryModel);

                                    break;
                                }
                            }



                        }
                    }

                    line.PlannedEndDate = date;

                    #endregion
                }

               
            }

            GridLineList = DataSource.SelectShipmentPlanningLines;

            SpinnerService.Hide();
            await _CalculateGrid.Refresh();

            #region Bilgilendirme Modalı

            await ModalManager.MessagePopupAsync("Bilgilendirme", "Hesaplama bitmiştir.");

            #endregion

            await OnSubmit();
            await InvokeAsync(StateHasChanged);
        }

        public void CellInfoHandler(QueryCellInfoEventArgs<ListProductionOrdersDto> Args)
        {
            if (GridLineList.Any(t=>t.ProductionOrderID == Args.Data.Id))
            {
                Args.Cell.AddStyle(new string[] { "background-color: #69F713; color: black; " });
               
            }
            else
            {
                Args.Cell.AddStyle(new string[] { "background-color: white; color: black; " });
            }
            StateHasChanged();
        }

        public void HideCalculateModal()
        {
            CalculateModalVisible = false;
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




