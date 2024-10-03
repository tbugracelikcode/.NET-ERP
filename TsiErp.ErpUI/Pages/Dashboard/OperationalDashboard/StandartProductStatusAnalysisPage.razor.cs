using DevExpress.Blazor.Internal;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.ErpUI.Pages.Dashboard.OperationalDashboard
{
    public partial class StandartProductStatusAnalysisPage
    {
        public List<StandartProductAnalysis> StandartProductAnalysisList = new List<StandartProductAnalysis>();

        public List<RawMaterialDetail> RawMaterialDetailList = new List<RawMaterialDetail>();

        SfGrid<StandartProductAnalysis> _grid;

        SfGrid<RawMaterialDetail> _rawgrid;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        public string[] InitialGroup = (new string[] { "ProductGroupName" });
        public List<ContextMenuItemModel> GridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public class StandartProductAnalysis
        {
            public Guid ProductGroupID { get; set; }
            public string ProductGroupName { get; set; }
            public Guid ProductionOrderID { get; set; }
            public Guid ContractTrackingFicheID { get; set; }
            public Guid ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal ReadyStockQuantity { get; set; }
            public decimal CriticalStockQuantity { get; set; }
            public decimal CriticalReadyStockDifferenceQuantity { get; set; }
            public decimal ProductionContinuingQuantity { get; set; }
            public decimal ContractContinuingQuantity { get; set; }
            public decimal OpenWorkOrdersPlannedQuantity { get; set; }
            public decimal OpenWorkOrdersRequirementQuantity { get; set; }
        }

        public class RawMaterialDetail
        {
            public Guid UnitSetID { get; set; }
            public string UnitSetCode { get; set; }
            public Guid FinishedProductID { get; set; }
            public string FinishedProductCode { get; set; }
            public string FinishedProductName { get; set; }
            public Guid ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal RawMaterialQuantity { get; set; }
            public decimal RawMateriaPossiblelQuantity { get; set; }
        }

        public bool RawMaterialModalVisible = false;


        protected override async void OnInitialized()
        {

            CreateMainContextMenuItems();

            GetStandartProductAnalysisList();

            await InvokeAsync(StateHasChanged);

        }

        public async void GetStandartProductAnalysisList()
        {
            StandartProductAnalysisList.Clear();

            var productsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.isStandart).ToList();

            foreach (var product in productsList)
            {

                #region Hazır Stok

                var grandTotalofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == product.Id).ToList();

                decimal readyStock = grandTotalofProduct.Sum(t => t.Amount);

                #endregion

                #region Devam Eden Üretim/Fason Miktar

                var openProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.ProductionOrderState == Entities.Enums.ProductionOrderStateEnum.DevamEdiyor && t.FinishedProductID == product.Id).ToList();

                decimal productionContinuingQuantity = 0;
                decimal contractContinuingQuantity = 0;

                if (openProductionOrdersList != null && openProductionOrdersList.Count > 0)
                {
                    productionContinuingQuantity = openProductionOrdersList.Sum(t => t.PlannedQuantity) - openProductionOrdersList.Sum(t => t.ProducedQuantity);
                }

                var contractTrackingFicheList = (await ContractTrackingFichesAppService.GetListbyProductIDAsync(product.Id)).Data.Where(t => t.Balance_ > 0).ToList();

                if (contractTrackingFicheList != null && contractTrackingFicheList.Count > 0)
                {
                    contractContinuingQuantity = contractTrackingFicheList.Sum(t => t.Balance_);
                }

                #endregion

                #region Açık İş Emirleri İhtiyaç Miktarı

                decimal openWorkOrdersPlanned = 0;

                var bomLineList = (await BillsofMaterialsAppService.GetLineListbyProductIDAsync(product.Id)).Data.ToList();

                if (bomLineList != null && bomLineList.Count > 0)
                {
                    foreach (var bomLine in bomLineList)
                    {
                        var notStartedProductionOrderList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.ProductionOrderState == Entities.Enums.ProductionOrderStateEnum.Baslamadi && t.FinishedProductID == bomLine.FinishedProductID).ToList();

                        if (notStartedProductionOrderList != null && notStartedProductionOrderList.Count > 0)
                        {
                            openWorkOrdersPlanned += notStartedProductionOrderList.Sum(t => t.PlannedQuantity) * bomLine.Quantity;
                        }
                    }
                } 

                #endregion


                StandartProductAnalysis standartProductAnalysisModel = new StandartProductAnalysis
                {
                    ProductID = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    ReadyStockQuantity = readyStock,
                    CriticalStockQuantity = product.CriticalStockQuantity,
                    CriticalReadyStockDifferenceQuantity = readyStock - product.CriticalStockQuantity,
                    ProductionContinuingQuantity = productionContinuingQuantity,
                    ContractContinuingQuantity= contractContinuingQuantity,
                    OpenWorkOrdersPlannedQuantity = openWorkOrdersPlanned,
                    OpenWorkOrdersRequirementQuantity = openWorkOrdersPlanned - (readyStock + productionContinuingQuantity + contractContinuingQuantity),
                    ProductGroupID = product.ProductGrpID,
                    ProductGroupName = product.ProductGrp

                };

                StandartProductAnalysisList.Add(standartProductAnalysisModel);
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                GridContextMenu.Add(new ContextMenuItemModel { Text = L["StandartProductStatusAnalysisContextRawMaterialDetail"], Id = "rawmaterial" });
            }
        }

        public async void OnContextMenuClick(ContextMenuClickEventArgs<StandartProductAnalysis> args)
        {
            switch (args.Item.Id)
            {

                case "rawmaterial":

                    if (args.RowInfo.RowData != null)
                    {
                        RawMaterialDetailList.Clear();

                        var record = args.RowInfo.RowData;

                        var bomDataSource = (await BillsofMaterialsAppService.GetbyProductIDAsync(record.ProductID)).Data;

                        if (bomDataSource != null && bomDataSource.Id != Guid.Empty && bomDataSource.SelectBillsofMaterialLines != null && bomDataSource.SelectBillsofMaterialLines.Count > 0)
                        {
                            foreach (var bomLine in bomDataSource.SelectBillsofMaterialLines)
                            {
                                decimal rawQuantity = 0;
                                decimal rawPossibleQuantity = 0;

                                var grandTotalofRawList = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID).ToList();

                                if (grandTotalofRawList.Count > 0)
                                {
                                    rawQuantity = grandTotalofRawList.Sum(t => t.Amount);
                                }

                                rawPossibleQuantity = bomLine.Size == 0 ? 0: (rawQuantity / bomLine.Size);

                                RawMaterialDetail rawMaterialDetailModel = new RawMaterialDetail
                                {
                                    FinishedProductID = record.ProductID,
                                    FinishedProductCode = record.ProductCode,
                                    FinishedProductName = record.ProductName,
                                    ProductCode = bomLine.ProductCode,
                                    ProductID = bomLine.ProductID.GetValueOrDefault(),
                                    ProductName = bomLine.ProductName,
                                    RawMaterialQuantity = rawQuantity,
                                    RawMateriaPossiblelQuantity = rawPossibleQuantity,
                                    UnitSetCode = bomLine.UnitSetCode,
                                    UnitSetID = bomLine.UnitSetID.GetValueOrDefault(),
                                };

                                RawMaterialDetailList.Add(rawMaterialDetailModel);
                            }
                        }

                        RawMaterialModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "refresh":
                    GetStandartProductAnalysisList();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideRawMaterialModal()
        {
            RawMaterialDetailList.Clear();

            RawMaterialModalVisible = false;
        }
    }
}
