﻿using BlazorBootstrap;
using DevExpress.Blazor.Reporting;
using DevExpress.CodeParser;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using TsiErp.Business.Entities.BankAccount.Services;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.SalesManagementParameter.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using TsiErp.Business.Entities.SalesOrder.Services;
using TsiErp.Business.Entities.UnitSet.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Pages.ShippingManagement.PackingList;
using TsiErp.ErpUI.Reports.ProductionManagement;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.ShippingInstruction;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionOrder
{
    public partial class ProductionOrdersListPage : IDisposable
    {

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        #region HM-YM İzleme Değişkenleri

        public class SPFPTracking
        {
            public ProductTypeEnum LineType { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string UnitSet { get; set; }
            public decimal Quantity { get; set; }
            public decimal TotalCounsume { get; set; }
            public decimal TotalWastage { get; set; }
            public decimal TotalOutput { get; set; }
            public decimal Size { get; set; }
        }

        public List<SPFPTracking> TrackingList = new List<SPFPTracking>();
        public List<StockReserveFiches> ReserveList = new List<StockReserveFiches>();

        private SfGrid<SPFPTracking> _TrackingGrid;
        private SfGrid<ListProductionOrdersDto> _ProdOrderGrid;

        public bool TrackingModalVisible = false;
        public List<ItemModel> TrackingToolbarItems { get; set; } = new List<ItemModel>();

        #endregion

        [Inject]
        SpinnerService Spinner { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> StockFicheGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> StockFicheLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductionOrderChangeReportGridContextMenu { get; set; } = new List<ContextMenuItemModel>();


        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public List<ListProductionOrdersDto> OrdersList = new List<ListProductionOrdersDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        List<SelectStockFicheLinesDto> StockFicheLineList;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        #region Malzeme Fişleri Değişkenleri

        public List<ListStockFichesDto> StockFichesList = new List<ListStockFichesDto>();
        SelectStockFichesDto StockFicheDataSource;
        SelectStockFicheLinesDto StockFicheLineDataSource;
        private SfGrid<SelectStockFicheLinesDto> _StockFicheLineGrid;
        private SfGrid<ListStockFichesDto> _StockFicheGrid;
        public bool StockFicheModalVisible = false;
        public bool StockFicheEditPageVisible = false;
        public bool StockFicheLineCrudPopupVisible = false;

        #endregion

        #region Üretim Emri Değişiklik Raporları Değişkenleri


        public List<ListProductionOrderChangeReportsDto> ProductionOrderChangeReportsList = new List<ListProductionOrderChangeReportsDto>();
        public List<SelectWorkOrdersDto> WorkOrdersList = new List<SelectWorkOrdersDto>();
        private SfGrid<ListProductionOrderChangeReportsDto> _ProductionOrderChangeReportGrid;
        SelectProductionOrderChangeReportsDto SelectProductionOrderChangeReportDataSource;
        ListWorkOrdersParameterDto ListWorkOrdersParameterDto;
        public bool ProductionOrderChangeReportModalVisible = false;
        public bool ProductionOrderChangeReportViewModalVisible = false;
        public bool WorkOrderModalVisible = false;
        private SfGrid<SelectWorkOrdersDto> _WorkOrderGrid;

        public int actionComboIndex = 0;

        #endregion

        #region Teknik Resim Değiştirme Değişkenleri

        SelectTechnicalDrawingsDto TechDrawingDataSource;
        public bool TechDrawingModalVisible = false;
        public string OldTechDrawingNo = string.Empty;
        public string NewTechDrawingNo = string.Empty;

        #endregion


        private bool Visibility { get; set; }
        public bool ProdRefNosModalVisible = false;
        private bool isProductionDateReferenceNumberSelected = false;
        public Guid NewProductionDateReferenceID = Guid.Empty;
        public string NewProductionDateReferenceNo = string.Empty;

        public bool OccuredAmountPopup = false;
        public Guid? productionDateReferanceID = Guid.Empty;
        public decimal quantity = 0;
        Guid? BranchIDParameter;
        Guid? WarehouseIDParameter;


        #region Toplu Rezerv Yapma Değişkenleri
        public class StockReserveFiches
        {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
        DateTime filterStartDate = DateTime.Today;
        DateTime filterEndDate = DateTime.Today;


        public bool BulkReserveModalVisible = false;

        #endregion
        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionOrdersAppService;
            _L = L;

            var purchaseParameter = (await PurchaseManagementParametersAppService.GetPurchaseManagementParametersAsync()).Data;
            BranchIDParameter = purchaseParameter.BranchID;
            WarehouseIDParameter = purchaseParameter.WarehouseID;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductionOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateStockFichesContextMenuItems();
            CreateStockFicheLinesContextMenuItems();
            CreateProductionOrderChangeContextMenuItems();

            var stockParameterDataSource = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data;

            futureDateParameter = stockParameterDataSource.FutureDateParameter;

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
                            case "ProductionOrderContextWorkOrders":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextWorkOrders"], Id = "workorders" }); break;
                            case "ProductionOrderContextProductionDateRefNos":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextProductionDateRefNos"], Id = "productiondaterefnos" }); break;
                            case "ProductionOrderContextOccuredAmountEntry":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextOccuredAmountEntry"], Id = "occuredamountentry" }); break;
                            //case "ProductionOrderContextConsumptionReceipt":
                            //    MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextConsumptionReceipt"], Id = "consumptionreceipt" }); break;
                            case "ProductionOrderContextCancel":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextCancel"], Id = "cancel" }); break;
                            //case "ProductionOrderContextMaterialSupplyStatus":
                            //    MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextMaterialSupplyStatus"], Id = "materialsupplystatus" }); break;
                            case "ProductionOrderContextWrite":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "ProductionOrderContextRequestForm":
                                                subMenus.Add(new MenuItem { Text = L["ProductionOrderContextRequestForm"], Id = "requestform" }); break;
                                            case "ProductionOrderContextPrintProdOrder":
                                                subMenus.Add(new MenuItem { Text = L["ProductionOrderContextPrintProdOrder"], Id = "printprodorder" }); break;
                                            case "ProductionOrderContextViewProdOrder":
                                                subMenus.Add(new MenuItem { Text = L["ProductionOrderContextViewProdOrder"], Id = "viewprodorder" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }


                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextWrite"], Id = "write", Items = subMenus }); break;


                            case "ProductionOrderContextMaterialFiches":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextMaterialFiches"], Id = "materialfiches" }); break;
                            case "ProductionOrderContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextRefresh"], Id = "refresh" }); break;
                            case "ProductionOrderContextUnsuitabilityRecords":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextUnsuitabilityRecords"], Id = "unsrecords" }); break;
                            case "ProductionOrderContextTrackingChart":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextTrackingChart"], Id = "trackingchart" }); break;
                            case "ProductionOrderContextChangeTechnicalDrawing":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextChangeTechnicalDrawing"], Id = "changetechdrawing" }); break;
                            case "ProductionOrderContextBulkReserve":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextBulkReserve"], Id = "bulkreserve" }); break;


                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductionOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "workorders":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        WorkOrdersList = (await WorkOrdersAppService.GetSelectListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

                        WorkOrderModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }

                    break;
                case "productiondaterefnos":

                    if (args.RowInfo.RowData != null)
                    {
                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        //

                        ProdRefNosModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "occuredamountentry":

                    Spinner.Show();

                    await Task.Delay(100);
                    if (isProductionDateReferenceNumberSelected == true)
                    {
                        if (args.RowInfo.RowData != null)
                        {

                            DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                            if (DataSource.WarehouseID == Guid.Empty || DataSource.WarehouseID == null)
                            {
                                var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;
                                DataSource.WarehouseID = warehouse.Id;
                                DataSource.WarehouseCode = warehouse.Code;
                            }

                            if (DataSource.BranchID == Guid.Empty || DataSource.BranchID == null)
                            {
                                var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;
                                DataSource.BranchID = branch.Id;
                                DataSource.BranchCode = branch.Code;
                            }

                            OccuredAmountPopup = true;

                            Spinner.Hide();

                            await InvokeAsync(StateHasChanged);
                        }

                    }
                    else
                    {
                        Spinner.Hide();
                        await ModalManager.WarningPopupAsync(L["UIProductionDateReferenceNoTitle"], L["UIProductionDateReferenceNoMessage"]);
                    }


                    break;

                //case "consumptionreceipt":

                //    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                //    SelectStockFichesDto stockFicheModel = new SelectStockFichesDto
                //    {
                //        BranchID = DataSource.BranchID.GetValueOrDefault(),
                //        BranchCode = DataSource.BranchCode,
                //        PurchaseOrderID = Guid.Empty,
                //        PurchaseOrderFicheNo = string.Empty,
                //        CurrencyID = Guid.Empty,
                //        CurrencyCode = string.Empty,
                //        Date_ = GetSQLDateAppService.GetDateFromSQL(),
                //        Description_ = string.Empty,
                //        ExchangeRate = 0,
                //        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                //        FicheType = StockFicheTypeEnum.SarfFisi,
                //        InputOutputCode = 1,
                //        NetAmount = 0,
                //        WarehouseID = DataSource.WarehouseID.GetValueOrDefault(),
                //        WarehouseCode = DataSource.WarehouseCode,
                //        TransactionExchangeCurrencyID = Guid.Empty,
                //        TransactionExchangeCurrencyCode = string.Empty,
                //        Time_ = null,
                //        SpecialCode = string.Empty,
                //        PurchaseRequestID = Guid.Empty,
                //        PurchaseRequestFicheNo = string.Empty,
                //        ProductionOrderID = DataSource.Id,
                //        ProductionDateReferance = Guid.Empty,
                //        ProductionOrderCode = DataSource.FicheNo,

                //    };

                //    SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                //    {
                //         Date_ = stockFicheModel.Date_,
                //          FicheType = StockFicheTypeEnum.SarfFisi,
                //           LineAmount = 0,
                //            LineDescription = string.Empty,
                //             LineNr = 1,
                //              MRPID = Guid.Empty,
                //               MRPLineID = Guid.Empty,
                //                ProductID = DataSource.FinishedProductID.GetValueOrDefault(),
                //                 ProductCode = DataSource.FinishedProductCode,
                //                  ProductName = DataSource.FinishedProductName,
                //                   ProductionDateReferance = Guid.Empty,
                //                    PurchaseOrderID = Guid.Empty,
                //                     PurchaseOrderFicheNo = string.Empty,
                //                      PurchaseOrderLineID = Guid.Empty,
                ////                       Quantity = DataSource.Q
                //    };

                //    break;


                case "cancel":

                    if (args.RowInfo.RowData != null)
                    {

                        Spinner.Show();

                        await Task.Delay(100);

                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        DataSource.Cancel_ = true;

                        var updatedEntity = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

                        await ProductionOrdersAppService.UpdateAsync(updatedEntity);

                        var workOrdersList = (await WorkOrdersAppService.GetSelectListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

                        if (workOrdersList != null && workOrdersList.Count > 0)
                        {
                            foreach (var workOrder in workOrdersList)
                            {
                                workOrder.IsCancel = true;

                                var updatedWorkOrder = ObjectMapper.Map<SelectWorkOrdersDto, UpdateWorkOrdersDto>(workOrder);

                                await WorkOrdersAppService.UpdateAsync(updatedWorkOrder);
                            }
                        }

                        await GetListDataSourceAsync();

                        await _grid.Refresh();

                        Spinner.Hide();

                        await InvokeAsync(StateHasChanged);
                    }

                    break;


                case "materialsupplystatus":

                    break;


                case "requestform":
                    if (args.RowInfo.RowData != null)
                    {

                        RawMaterialRequestFormDynamicReport = new XtraReport();
                        RawMaterialRequestFormReportVisible = true;
                        await CreateRawMaterialRequestFormReport(args.RowInfo.RowData.Id);

                        await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "printprodorder":

                    break;


                case "viewprodorder":

                    break;


                case "materialfiches":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

                        StockFicheModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }



                    break;


                case "refresh":

                    await GetListDataSourceAsync();

                    await _grid.Refresh();

                    await InvokeAsync(StateHasChanged);

                    break;


                case "unsrecords":
                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        ProductionOrderChangeReportsList = (await ProductionOrderChangeReportsAppService.GetListAsync(new ListProductionOrderChangeReportsParameterDto())).Data.Where(t => t.ProductionOrderID == DataSource.Id).ToList();


                        ProductionOrderChangeReportModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }



                    break;


                case "trackingchart":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        TrackingToolbarItems.Clear();

                        var bomDataSource = (await BillsofMaterialsAppService.GetListbyProductIDAsync(DataSource.FinishedProductID.GetValueOrDefault())).Data;

                        if (bomDataSource != null && bomDataSource.Id != Guid.Empty)
                        {
                            if (bomDataSource.SelectBillsofMaterialLines != null && bomDataSource.SelectBillsofMaterialLines.Count > 0)
                            {
                                TrackingList.Clear();

                                foreach (var bomline in bomDataSource.SelectBillsofMaterialLines)
                                {
                                    var consumeQuantity = (await StockFichesAppService.GetLineConsumeListbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Data.Sum(t => t.Quantity);
                                    var wastageQuantity = (await StockFichesAppService.GetLineWastageListbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Data.Sum(t => t.Quantity);

                                    SPFPTracking sPFPTrackingModel = new SPFPTracking
                                    {
                                        LineType = bomline.MaterialType,
                                        ProductCode = bomline.ProductCode,
                                        ProductName = bomline.ProductName,
                                        UnitSet = bomline.UnitSetCode,
                                        Quantity = DataSource.PlannedQuantity * bomline.Quantity,
                                        TotalCounsume = consumeQuantity,
                                        TotalWastage = wastageQuantity,
                                        TotalOutput = wastageQuantity + consumeQuantity,
                                        Size = 0
                                    };

                                    TrackingList.Add(sPFPTrackingModel);
                                }

                                TrackingModalVisible = true;

                                TrackingToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = L["UIExportTracking"] });

                                await InvokeAsync(StateHasChanged);
                            }

                        }
                        else
                        {
                            await ModalManager.MessagePopupAsync(L["UIMessageNullBomTitle"], L["UIMessageNullBomMessage"]);
                        }
                    }
                    break;


                case "changetechdrawing":

                    if (args.RowInfo.RowData != null)
                    {

                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        DataSource.TechnicalDrawingUpdateDate_ = GetSQLDateAppService.GetDateFromSQL().Date;

                        NewTechDrawingNo = string.Empty;

                        TechDrawingDataSource = (await TechnicalDrawingsAppService.GetSelectListAsync(DataSource.FinishedProductID.GetValueOrDefault())).Data.Where(t => t.IsApproved && t.CustomerApproval).FirstOrDefault();

                        if (TechDrawingDataSource != null && TechDrawingDataSource.Id != Guid.Empty)
                        {

                            OldTechDrawingNo = TechDrawingDataSource.RevisionNo;
                            TechDrawingModalVisible = true;
                        }
                        else
                        {
                            OldTechDrawingNo = string.Empty;
                            await ModalManager.MessagePopupAsync(L["UIMessageNullTechDrawTitle"], L["UIMessageNullTechDrawMessage"]);
                        }



                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "bulkreserve":
                    if (args.RowInfo.RowData != null)
                    {
                        var today = GetSQLDateAppService.GetDateFromSQL().Date;

                        filterStartDate = new DateTime(today.Year, today.Month, 1);

                        var nextMonthDate = new DateTime(today.Year, today.Month + 1, 1);

                        filterEndDate = nextMonthDate.AddDays(-1);


                        DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        OrdersList.Clear();

                        if (DataSource != null && DataSource.Id != Guid.Empty)
                        {
                            if (filterStartDate.Date <= filterEndDate.Date)
                            {
                                ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.CustomerRequestedDate >= filterStartDate && t.CustomerRequestedDate <= filterEndDate && t.isReserve == false && t.ProductionOrderState != ProductionOrderStateEnum.Iptal).ToList();

                            }
                        }

                        BulkReserveModalVisible = true;


                        await InvokeAsync(StateHasChanged);
                    }
                    break;



                default:
                    break;
            }
        }

        public async Task CreateFiche()
        {
            if (quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningCreateOccuredAmountFicheTitle"], L["UIWarningCreateOccuredAmountFicheMessageQuantity"]);
                return;
            }
            else if (productionDateReferanceID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningCreateOccuredAmountFicheTitle"], L["UIWarningCreateOccuredAmountFicheMessageRefNo"]);
                return;
            }
            else
            {
                DataSource.ProducedQuantity = DataSource.ProducedQuantity + quantity;

                var updateInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

                await ProductionOrdersAppService.UpdateOccuredAmountEntryAsync(updateInput);

                StockFicheLineList = new List<SelectStockFicheLinesDto>();

                var now = GetSQLDateAppService.GetDateFromSQL();

                SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                {
                    UnitSetID = DataSource.UnitSetID,
                    UnitSetCode = DataSource.UnitSetCode,
                    UnitPrice = 0,
                    Quantity = quantity,
                    PurchaseOrderLineID = Guid.Empty,
                    PurchaseOrderID = Guid.Empty,
                    PurchaseOrderFicheNo = string.Empty,
                    ProductName = DataSource.FinishedProductName,
                    ProductID = DataSource.FinishedProductID,
                    ProductCode = DataSource.FinishedProductCode,
                    ProductionDateReferenceID = productionDateReferanceID,
                    LineNr = 0,
                    LineDescription = string.Empty,
                    LineAmount = 0,
                    FicheType = Entities.Enums.StockFicheTypeEnum.UretimdenGirisFisi,
                };

                StockFicheLineList.Add(stockFicheLineModel);

                CreateStockFichesDto stockFicheModel = new CreateStockFichesDto
                {
                    WarehouseID = DataSource.WarehouseID,
                    Time_ = now.TimeOfDay,
                    SpecialCode = string.Empty,
                    PurchaseOrderID = Guid.Empty,
                    ProductionOrderID = DataSource.Id,
                    ProductionDateReferenceID = productionDateReferanceID,
                    NetAmount = 0,
                    InputOutputCode = 0,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                    FicheType = 13,
                    BranchID = DataSource.BranchID,
                    CurrencyID = Guid.Empty,
                    Date_ = now.Date,
                    Description_ = string.Empty,
                    ExchangeRate = 0
                };

                stockFicheModel.SelectStockFicheLines = StockFicheLineList;

                await StockFichesAppService.CreateAsync(stockFicheModel);

                HideOccuredAmountPopup();

                await InvokeAsync(StateHasChanged);
            }
        }

        protected override async Task GetListDataSourceAsync()
        {
            ListDataSource = (await ProductionOrdersAppService.GetNotCanceledListAsync(new ListProductionOrdersParameterDto())).Data.ToList();

            IsLoaded = true;
        }

        public async void HideOccuredAmountPopup()
        {
            OccuredAmountPopup = false;
            quantity = 0;
            productionDateReferanceID = Guid.Empty;
            await GetListDataSourceAsync();
        }

        #region Malzeme Fişleri Metotları

        public void CreateStockFichesContextMenuItems()
        {
            if (StockFicheGridContextMenu.Count() == 0)
            {

                List<MenuItem> subMenus = new List<MenuItem>();


                //subMenus.Add(new MenuItem { Text = L["StockFicheContextAddStockIncome"], Id = "income" });
                //subMenus.Add(new MenuItem { Text = L["StockFicheContextAddStockOutput"], Id = "output" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddConsume"], Id = "consume" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddWastege"], Id = "wastage" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddProductionIncome"], Id = "proincome" });
                //subMenus.Add(new MenuItem { Text = L["StockFicheContextAddWarehouse"], Id = "warehouse" });
                //subMenus.Add(new MenuItem { Text = L["StockFicheContextAddReserved"], Id = "reserved" });

                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFichesGeneralConAdd"], Id = "add", Items = subMenus });
                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextChange"], Id = "changed" });
                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextDelete"], Id = "delete" });
                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextRefresh"], Id = "refresh" });

            }

        }

        public void CreateStockFicheLinesContextMenuItems()
        {
            if (StockFicheLineGridContextMenu.Count() == 0)
            {


                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextAdd"], Id = "new" });
                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextChange"], Id = "changed" });
                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextDelete"], Id = "delete" });
                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextRefresh"], Id = "refresh" });

            }

        }

        protected async Task StockFicheBeforeInsertAsync()
        {
            var productionManagementParameter = (await ProductionManagementParametersAppService.GetProductionManagementParametersAsync()).Data;
            StockFicheDataSource = new SelectStockFichesDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                Time_ = GetSQLDateAppService.GetDateFromSQL().TimeOfDay,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                ProductionOrderID = DataSource.Id,
                CurrencyID = Guid.Empty,
                //BranchID = productionManagementParameter != null && productionManagementParameter.Id != Guid.Empty ? productionManagementParameter.DefaultBranchID : Guid.Empty,
                //WarehouseID = productionManagementParameter != null && productionManagementParameter.Id != Guid.Empty ? productionManagementParameter.DefaultWarehouseID : Guid.Empty,
                BranchID = DataSource.BranchID.GetValueOrDefault(),
                WarehouseID = DataSource.WarehouseID.GetValueOrDefault(),
                BranchCode = DataSource.BranchCode,
                WarehouseCode = DataSource.WarehouseCode,
            };
            StockFicheDataSource.SelectStockFicheLines = new List<SelectStockFicheLinesDto>();
            StockFicheLineList = StockFicheDataSource.SelectStockFicheLines;
            await Task.CompletedTask;
        }

        public async void StockFicheShowEditPage()
        {

            if (StockFicheDataSource != null)
            {

                if (StockFicheDataSource.DataOpenStatus == true && StockFicheDataSource.DataOpenStatus != null)
                {
                    StockFicheEditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    StockFicheEditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void StockFicheMainContextMenuClick(ContextMenuClickEventArgs<ListStockFichesDto> args)
        {
            foreach (var item in types)
            {
                item.FicheTypeName = L[item.FicheTypeName];
            }

            switch (args.Item.Id)
            {
                case "wastage":



                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.FireFisi;
                    Visibility = false;
                    StockFicheEditPageVisible = true;

                    break;

                case "consume":



                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.SarfFisi;
                    Visibility = false;
                    StockFicheEditPageVisible = true;

                    break;

                case "proincome":



                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.UretimdenGirisFisi;
                    Visibility = true;
                    StockFicheEditPageVisible = true;

                    break;

                case "warehouse":


                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.DepoSevkFisi;
                    Visibility = false;
                    StockFicheEditPageVisible = true;

                    break;
                case "reserved":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.StokRezerveFisi;
                    Visibility = false;
                    StockFicheEditPageVisible = true;

                    break;

                case "income":


                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.StokGirisFisi;
                    Visibility = false;
                    StockFicheEditPageVisible = true;

                    break;

                case "output":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.StokCikisFisi;
                    Visibility = false;
                    StockFicheEditPageVisible = true;

                    break;

                case "changed":

                    IsChanged = true;
                    StockFicheDataSource = (await StockFichesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    StockFicheLineList = StockFicheDataSource.SelectStockFicheLines;


                    StockFicheShowEditPage();
                    await InvokeAsync(StateHasChanged);

                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupStockFicheMessageBase"]);
                    if (res == true)
                    {
                        await StockFichesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                        StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                        await _StockFicheGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }


                    break;

                case "refresh":
                    StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                    await _StockFicheGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnListStockFicheLineContextMenuClick(ContextMenuClickEventArgs<SelectStockFicheLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (StockFicheDataSource.FicheType == 0)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningFicheTypeTitleBase"], L["UIWarningFicheTypeMessageBase"]);
                    }
                    else
                    {
                        StockFicheLineDataSource = new SelectStockFicheLinesDto();
                        StockFicheLineCrudPopupVisible = true;
                        StockFicheLineDataSource.FicheType = StockFicheDataSource.FicheType;
                        StockFicheLineDataSource.LineNr = StockFicheLineList.Count + 1;
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        StockFicheLineDataSource = args.RowInfo.RowData;
                        StockFicheLineCrudPopupVisible = true;
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
                                StockFicheDataSource.SelectStockFicheLines.Remove(args.RowInfo.RowData);
                            }
                            else
                            {
                                if (line != null)
                                {
                                    await StockFichesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                                    StockFicheDataSource.SelectStockFicheLines.Remove(line);
                                    StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                                }
                                else
                                {
                                    StockFicheDataSource.SelectStockFicheLines.Remove(line);
                                }
                            }

                            await _StockFicheLineGrid.Refresh();
                            GetTotal();
                            await InvokeAsync(StateHasChanged);
                        }
                    }

                    break;

                case "refresh":
                    StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                    await _StockFicheLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public override async void LineCalculate()
        {
            StockFicheLineDataSource.LineAmount = StockFicheLineDataSource.Quantity * StockFicheLineDataSource.UnitPrice;

            StockFicheLineDataSource.TransactionExchangeLineAmount = StockFicheLineDataSource.Quantity * StockFicheLineDataSource.TransactionExchangeUnitPrice;

            await Task.CompletedTask;
        }



        protected async Task OnStockFicheLineLineSubmit()
        {

            if (StockFicheLineDataSource.UnitSetID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase1"]);
            }
            else if (StockFicheLineDataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase2"]);
            }
            else if (StockFicheLineDataSource.Quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase3"]);
            }
            else
            {
                if (StockFicheLineDataSource.Id == Guid.Empty)
                {
                    if (StockFicheDataSource.SelectStockFicheLines.Contains(StockFicheLineDataSource))
                    {
                        int selectedLineIndex = StockFicheDataSource.SelectStockFicheLines.FindIndex(t => t.LineNr == StockFicheLineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            StockFicheDataSource.SelectStockFicheLines[selectedLineIndex] = StockFicheLineDataSource;
                        }
                    }
                    else
                    {
                        StockFicheDataSource.SelectStockFicheLines.Add(StockFicheLineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = StockFicheDataSource.SelectStockFicheLines.FindIndex(t => t.Id == StockFicheLineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        StockFicheDataSource.SelectStockFicheLines[selectedLineIndex] = StockFicheLineDataSource;
                    }
                }

                StockFicheLineList = StockFicheDataSource.SelectStockFicheLines;

                StockFicheDataSource.NetAmount = StockFicheLineList.Sum(t => t.LineAmount);

                await _StockFicheLineGrid.Refresh();

                HideStockFicheLineCrudModal();
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void OnStockFicheSubmit()
        {

            if (StockFicheDataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectStockFichesDto, CreateStockFichesDto>(StockFicheDataSource);

                await StockFichesAppService.CreateAsync(createInput);
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectStockFichesDto, UpdateStockFichesDto>(StockFicheDataSource);

                await StockFichesAppService.UpdateAsync(updateInput);
            }



            StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

            await _StockFicheGrid.Refresh();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideStockFicheEditPage();

            await InvokeAsync(StateHasChanged);

        }


        public async void OnProdRefNosSubmit()
        {
            if(DataSource.Id != Guid.Empty)
            {
                var updatedEntity = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);
                await ProductionOrdersAppService.UpdateAsync(updatedEntity);

            }

            HideProdRefNosModalViewModal();
            await GetListDataSourceAsync();
            await InvokeAsync(StateHasChanged);

        }


        #region Combobox İşlemleri

        public IEnumerable<SelectStockFichesDto> types = GetEnumDisplayTypeNames<StockFicheTypeEnum>();

        public static List<SelectStockFichesDto> GetEnumDisplayTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<StockFicheTypeEnum>()
                       .Select(x => new SelectStockFichesDto
                       {
                           FicheType = x,
                           FicheTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

        #region GetList Metotları

        private async Task GetBranchesList()
        {
            StockFicheBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        private async Task GetWarehousesList()
        {
            StockFicheWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        #endregion

        #region Şube ButtonEdit

        SfTextBox StockFicheBranchesButtonEdit;
        bool SelectStockFicheBranchesPopupVisible = false;
        List<ListBranchesDto> StockFicheBranchesList = new List<ListBranchesDto>();

        public async Task StockFicheBranchesOnCreateIcon()
        {
            var StockFicheBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockFicheBranchesButtonClickEvent);
            await StockFicheBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockFicheBranchesButtonClick } });
        }

        public async void StockFicheBranchesButtonClickEvent()
        {
            SelectStockFicheBranchesPopupVisible = true;
            await GetBranchesList();
            await InvokeAsync(StateHasChanged);
        }

        public void StockFicheBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.BranchID = Guid.Empty;
                StockFicheDataSource.BranchCode = string.Empty;
            }
        }

        public async void StockFicheBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                StockFicheDataSource.BranchID = selectedUnitSet.Id;
                StockFicheDataSource.BranchCode = selectedUnitSet.Code;
                SelectStockFicheBranchesPopupVisible = false;
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
            await GetCurrenciesList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.CurrencyID = Guid.Empty;
                StockFicheDataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                StockFicheDataSource.CurrencyID = selectedCurrency.Id;
                StockFicheDataSource.CurrencyCode = selectedCurrency.Name;
                SelectCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Depo ButtonEdit

        SfTextBox StockFicheWarehousesButtonEdit;
        bool SelecStockFichetWarehousesPopupVisible = false;
        List<ListWarehousesDto> StockFicheWarehousesList = new List<ListWarehousesDto>();

        public async Task StockFicheWarehousesOnCreateIcon()
        {
            var StockFicheWarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockFicheWarehousesButtonClickEvent);
            await StockFicheWarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockFicheWarehousesButtonClick } });
        }

        public async void StockFicheWarehousesButtonClickEvent()
        {
            SelecStockFichetWarehousesPopupVisible = true;
            await GetWarehousesList();
            await InvokeAsync(StateHasChanged);
        }

        public void SelecStockFicheWarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.WarehouseID = Guid.Empty;
                StockFicheDataSource.WarehouseCode = string.Empty;
            }
        }

        public async void StockFicheWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                StockFicheDataSource.WarehouseID = selectedWarehouse.Id;
                StockFicheDataSource.WarehouseCode = selectedWarehouse.Code;
                SelecStockFichetWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Birim Setleri ButtonEdit

        SfTextBox UnitSetsButtonEdit;
        bool SelectUnitSetsPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        public async Task UnitSetsOnCreateIcon()
        {
            var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
            await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        }

        public async void UnitSetsButtonClickEvent()
        {
            SelectUnitSetsPopupVisible = true;
            await GetUnitSetsList();
            await InvokeAsync(StateHasChanged);
        }

        public void UnitSetsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheLineDataSource.UnitSetID = Guid.Empty;
                StockFicheLineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                StockFicheLineDataSource.UnitSetID = selectedUnitSet.Id;
                StockFicheLineDataSource.UnitSetCode = selectedUnitSet.Name;
                SelectUnitSetsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

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
                StockFicheLineDataSource.ProductID = Guid.Empty;
                StockFicheLineDataSource.ProductCode = string.Empty;
                StockFicheLineDataSource.ProductName = string.Empty;
                StockFicheLineDataSource.UnitSetID = Guid.Empty;
                StockFicheLineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                StockFicheLineDataSource.ProductID = selectedProduct.Id;
                StockFicheLineDataSource.ProductCode = selectedProduct.Code;
                StockFicheLineDataSource.ProductName = selectedProduct.Name;
                StockFicheLineDataSource.UnitSetID = selectedProduct.UnitSetID;
                StockFicheLineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İşlem Dövizi Para Birimleri ButtonEdit

        SfTextBox TransactionExchangeCurrenciesButtonEdit;
        bool SelectTransactionExchangeCurrencyPopupVisible = false;
        List<ListCurrenciesDto> TransactionExchangeCurrenciesList = new List<ListCurrenciesDto>();

        public async Task TransactionExchangeCurrenciesOnCreateIcon()
        {
            var TransactionExchangeCurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TransactionExchangeCurrenciesButtonClickEvent);
            await TransactionExchangeCurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TransactionExchangeCurrenciesButtonClick } });
        }

        public async void TransactionExchangeCurrenciesButtonClickEvent()
        {
            SelectTransactionExchangeCurrencyPopupVisible = true;
            TransactionExchangeCurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void TransactionExchangeCurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.TransactionExchangeCurrencyID = Guid.Empty;
                StockFicheDataSource.TransactionExchangeCurrencyCode = string.Empty;
            }
        }

        public async void TransactionExchangeCurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                StockFicheDataSource.TransactionExchangeCurrencyID = selectedCurrency.Id;
                StockFicheDataSource.TransactionExchangeCurrencyCode = selectedCurrency.Name;
                SelectTransactionExchangeCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Fiş Kod ButtonEdit

        SfTextBox StockFicheCodeButtonEdit;

        public async Task StockFicheCodeOnCreateIcon()
        {
            var StockFicheCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockFicheCodeButtonClickEvent);
            await StockFicheCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockFicheCodesButtonClick } });
        }

        public async void StockFicheCodeButtonClickEvent()
        {
            StockFicheDataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Üretim Tarihi Referans No ButtonEdit

        SfTextBox ProductionDateReferenceNoButtonEdit = new();
        bool SelectProductionDateReferenceNoPopupVisible = false;
        List<ListProductionDateReferenceNumbersDto> ProductionDateReferenceNoList = new List<ListProductionDateReferenceNumbersDto>();
        public async Task ProductionDateReferenceNoOnCreateIcon()
        {
            var ProductionRefNosButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionDateReferenceNoButtonClickEvent);
            await ProductionDateReferenceNoButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionRefNosButtonClick } });
        }

        public async void ProductionDateReferenceNoButtonClickEvent()
        {
            SelectProductionDateReferenceNoPopupVisible = true;
            await GetProductionRefNosList();
            await InvokeAsync(StateHasChanged);

        }
        private async Task GetProductionRefNosList()
        {
            ProductionDateReferenceNoList = (await ProductionDateReferenceNumbersAppService.GetListAsync(new ListProductionDateReferenceNumbersParameterDto())).Data.Where(t => t.Confirmation == true).ToList();
        }
        public void ProductionDateReferenceNoOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductionDateReferenceNo = string.Empty;
                DataSource.ProductionDateReferenceID = Guid.Empty;
            }
        }

        public async void ProductionDateReferenceNoDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionDateReferenceNumbersDto> args)
        {
            var selectedRefNo = args.RowData;

            if (selectedRefNo != null)
            {
                DataSource.ProductionDateReferenceID = selectedRefNo.Id;
                DataSource.ProductionDateReferenceNo = selectedRefNo.ProductionDateReferenceNo;
                SelectProductionDateReferenceNoPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
            isProductionDateReferenceNumberSelected = true;

        }


        #endregion


        public void HideStockFichesModal()
        {
            StockFicheModalVisible = false;
            StockFichesList.Clear();
        }

        public void HideStockFicheEditPage()
        {
            StockFicheEditPageVisible = false;
        }

        public void HideStockFicheLineCrudModal()
        {
            StockFicheLineCrudPopupVisible = false;
        }
        #endregion

        public void HideProdRefNosModalViewModal()
        {

            ProdRefNosModalVisible = false;
        }

        #region Uygunsuzluk Kayıtları Modal İşlemleri

        protected void CreateProductionOrderChangeContextMenuItems()
        {
            if (ProductionOrderChangeReportGridContextMenu.Count() == 0)
            {
                ProductionOrderChangeReportGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderChangeReportContextView"], Id = "review" });
            }
        }


        public async void ProductionOrderChangeContextMenuClick(ContextMenuClickEventArgs<ListProductionOrderChangeReportsDto> args)
        {

            switch (args.Item.Id)
            {
                case "review":

                    if (args.RowInfo.RowData != null)
                    {

                        SelectProductionOrderChangeReportDataSource = (await ProductionOrderChangeReportsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                        #region String Combobox Index Ataması
                        string scrap = L["ComboboxScrap"].Value;
                        string remanufacturing = L["ComboboxRemanufacturing"].Value;
                        string cancel = L["ComboboxProductionCancel"].Value;
                        var a = SelectProductionOrderChangeReportDataSource.Action_;

                        if (SelectProductionOrderChangeReportDataSource.Action_ == scrap) actionComboIndex = 0;
                        else if (SelectProductionOrderChangeReportDataSource.Action_ == remanufacturing) actionComboIndex = 1;
                        else if (SelectProductionOrderChangeReportDataSource.Action_ == cancel) actionComboIndex = 2;
                        #endregion

                        ProductionOrderChangeReportViewModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }

                    break;



                default:
                    break;
            }
        }

        public void HideProductionOrderChangeReportModal()
        {
            ProductionOrderChangeReportModalVisible = false;

            ProductionOrderChangeReportsList.Clear();
        }

        public void HideProductionOrderChangeReportViewModal()
        {
            ProductionOrderChangeReportViewModalVisible = false;
        }

        #endregion

        #region Teknik Resim Değiştirme Modalı İşlemleri

        public void HideTechnicalDrawingUpdateModal()
        {
            TechDrawingModalVisible = false;
        }

        public async void UpdateTechnicalDrawing()
        {
            TechDrawingDataSource.RevisionNo = NewTechDrawingNo;

            var updatedTechnicalDrawing = ObjectMapper.Map<SelectTechnicalDrawingsDto, UpdateTechnicalDrawingsDto>(TechDrawingDataSource);

            await TechnicalDrawingsAppService.UpdateAsync(updatedTechnicalDrawing);

            DataSource.TechnicalDrawingID = TechDrawingDataSource.Id;
            DataSource.TechnicalDrawingNo = TechDrawingDataSource.RevisionNo;

            var updatedEntity = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

            await ProductionOrdersAppService.UpdateChangeTechDrawingAsync(updatedEntity);

            HideTechnicalDrawingUpdateModal();

            await InvokeAsync(StateHasChanged);


        }


        #endregion

        #region Toplu Rezerv Modalı İşlemleri
        public void HideBulkReserveModal()
        {
            BulkReserveModalVisible = false;
        }

        public async void BulkReserveOnClick()
        {
            Spinner.Show();
            await Task.Delay(100);

            var date = GetSQLDateAppService.GetDateFromSQL();

            foreach (var productionOrder in OrdersList)
            {
                #region Stok Reserved Fiche
                var productionOrderBOM = (await BillsofMaterialsAppService.GetAsync(productionOrder.BOMID.GetValueOrDefault())).Data;

                if (productionOrderBOM.Id != Guid.Empty)
                {

                    SelectStockFichesDto stockFicheModel = new SelectStockFichesDto
                    {
                        BranchID = productionOrder.BranchID.GetValueOrDefault(),
                        PurchaseOrderID = Guid.Empty,
                        CurrencyID = Guid.Empty,
                        Date_ = date.Date,
                        Description_ = string.Empty,
                        ExchangeRate = 0,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                        FicheType = StockFicheTypeEnum.StokRezerveFisi,
                        InputOutputCode = 1,
                        NetAmount = 0,
                        WarehouseID = productionOrder.WarehouseID.GetValueOrDefault(),
                        TransactionExchangeCurrencyID = Guid.Empty,
                        Time_ = date.TimeOfDay,
                        SpecialCode = string.Empty,
                        PurchaseRequestID = Guid.Empty,
                        ProductionOrderID = productionOrder.Id,
                        ProductionOrderCode = productionOrder.FicheNo,
                        ProductionDateReferenceID = Guid.Empty,
                    };

                    stockFicheModel.SelectStockFicheLines = new List<SelectStockFicheLinesDto>();

                    int lineNR = 1;

                    foreach (var bomLine in productionOrderBOM.SelectBillsofMaterialLines.Where(t => t.SupplyForm == ProductSupplyFormEnum.Satınalma).ToList())
                    {
                        SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            Date_ = date.Date,
                            FicheType = StockFicheTypeEnum.StokRezerveFisi,
                            LineAmount = 0,
                            LineDescription = string.Empty,
                            LineNr = lineNR,
                            MRPID = Guid.Empty,
                            MRPLineID = Guid.Empty,
                            ProductID = bomLine.ProductID.GetValueOrDefault(),
                            PurchaseOrderID = Guid.Empty,
                            PurchaseOrderLineID = Guid.Empty,
                            Quantity = bomLine.Quantity * productionOrder.PlannedQuantity,
                            InputOutputCode = 1,
                            ProductionOrderID = productionOrder.Id,
                            PurchaseInvoiceID = Guid.Empty,
                            PurchaseInvoiceLineID = Guid.Empty,
                            SalesInvoiceID = Guid.Empty,
                            SalesInvoiceLineID = Guid.Empty,
                            SalesOrderID = productionOrder.OrderID.GetValueOrDefault(),
                            UnitSetID = bomLine.UnitSetID.GetValueOrDefault(),
                            ProductionDateReferenceID = Guid.Empty,


                        };

                        stockFicheModel.SelectStockFicheLines.Add(stockFicheLineModel);

                        lineNR++;
                    }

                    var createStockFicheInput = ObjectMapper.Map<SelectStockFichesDto, CreateStockFichesDto>(stockFicheModel);

                    await StockFichesAppService.CreateAsync(createStockFicheInput);
                }
                #endregion

                #region Production Order IsReserved Update

                await ProductionOrdersAppService.UpdateProductionOrderIsReservedAsync(productionOrder.Id);

                #endregion

            }

            await ModalManager.MessagePopupAsync(L["UIMessageTitle"], L["UIMessageReservedMessage"]);

            //HideBulkReserveModal();

            Spinner.Hide();

            await InvokeAsync(StateHasChanged);


        }

        public async void FilterButtonClicked()
        {
            Spinner.Show();
            await Task.Delay(100);


            if (filterStartDate.Date <= filterEndDate.Date)
            {
                ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.CustomerRequestedDate >= filterStartDate && t.CustomerRequestedDate <= filterEndDate && t.isReserve == false && t.ProductionOrderState != ProductionOrderStateEnum.Iptal).ToList();

                OrdersList = ProductionOrdersList;

                await _ProdOrderGrid.Refresh();
            }


            Spinner.Hide();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region HM YM İzleme Metotları

        public void HideTrackingModal()
        {
            TrackingModalVisible = false;
            TrackingList.Clear();
        }

        public async void TrackingToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();
            ExcelExportProperties.FileName = args.Item.TooltipText + ".xlsx";
            await this._TrackingGrid.ExportToExcelAsync(ExcelExportProperties);


        }

        #endregion

        #region İş Emirleri Metotları
        public void HideWorkOrderModalViewModal()
        {

            WorkOrderModalVisible = false;
        }
        public async void HideToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();
            ExcelExportProperties.FileName = args.Item.TooltipText + ".xlsx";
            await this._TrackingGrid.ExportToExcelAsync(ExcelExportProperties);


        }

        #endregion

        #region Depo ButtonEdit 

        SfTextBox WarehousesButtonEdit;
        bool SelectWarehousesPopupVisible = false;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        public async Task WarehousesOnCreateIcon()
        {
            var WarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WarehousesButtonClickEvent);
            await WarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WarehousesButtonClick } });
        }

        public async void WarehousesButtonClickEvent()
        {
            SelectWarehousesPopupVisible = true;
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {

                var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

                DataSource.WarehouseID = warehouse.Id;
                DataSource.WarehouseCode = warehouse.Code;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                DataSource.WarehouseID = selectedWarehouse.Id;
                DataSource.WarehouseCode = selectedWarehouse.Code;
                SelectWarehousesPopupVisible = false;


                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit

        SfTextBox BranchesButtonEdit;
        bool SelectBranchesPopupVisible = false;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        public async Task BranchesOnCreateIcon()
        {
            var BranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchesButtonClickEvent);
            await BranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchesButtonClick } });
        }

        public async void BranchesButtonClickEvent()
        {
            SelectBranchesPopupVisible = true;
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {

                var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;

                DataSource.BranchID = branch.Id;
                DataSource.BranchCode = branch.Code;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                DataSource.BranchID = selectedBranch.Id;
                DataSource.BranchCode = selectedBranch.Code;


                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Yazdır

        #region HM ve YM İstek Formu
        bool RawMaterialRequestFormReportVisible { get; set; }

        DxReportViewer RawMaterialRequestFormReportViewer { get; set; }

        XtraReport RawMaterialRequestFormDynamicReport { get; set; }

        async Task CreateRawMaterialRequestFormReport(Guid productionOrderId)
        {
            RawMaterialRequestFormDynamicReport.ShowPrintMarginsWarning = false;
            RawMaterialRequestFormDynamicReport.CreateDocument();

            if (productionOrderId != Guid.Empty)
            {
                var list = (await ProductionOrdersAppService.CreateRawMaterialRequestFormReportAsync(productionOrderId)).Data;

                RawMaterialRequestFormReport report = new RawMaterialRequestFormReport();
                report.DataSource = list;
                report.ShowPrintMarginsWarning = false;
                report.CreateDocument();

                RawMaterialRequestFormDynamicReport.Pages.AddRange(report.Pages);

                RawMaterialRequestFormDynamicReport.PrintingSystem.ContinuousPageNumbering = true;
            }

            await Task.CompletedTask;
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
