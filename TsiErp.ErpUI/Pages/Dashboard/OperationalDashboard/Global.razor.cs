using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using Syncfusion.Blazor.Cards;
using Syncfusion.Blazor.Buttons;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Business.Entities.Employee.Services;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine;
using TsiErp.Business.Entities.ContractTrackingFiche.Services;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using Syncfusion.Blazor.Calendars;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Business.Entities.ShipmentPlanning.Services;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine;
using TsiErp.Business.Entities.Route.Services;
using DevExpress.XtraRichEdit.Model;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Business.Entities.PurchasePrice.Services;
using Syncfusion.Blazor.Charts;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Dynamic;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;

namespace TsiErp.ErpUI.Pages.Dashboard.OperationalDashboard
{
    public partial class Global
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ListProductsDto> HMYMProductsList = new List<ListProductsDto>();
        public List<ListSalesOrderDto> CustomersList = new List<ListSalesOrderDto>();
        public List<ListContractTrackingFichesDto> TotalPendingProductsList = new List<ListContractTrackingFichesDto>();
        public List<ListProductionTrackingsDto> ProducedAndFaultyList = new List<ListProductionTrackingsDto>();
        public List<ListProductionTrackingsDto> ProductionList = new List<ListProductionTrackingsDto>();
        public List<ListOperationUnsuitabilityReportsDto> FaultyList = new List<ListOperationUnsuitabilityReportsDto>();
        public List<ContractWaitingProducts> ContractWaitingProductsList = new List<ContractWaitingProducts>();
        public List<ListShipmentPlanningsDto> ShipmentPlanningsList = new List<ListShipmentPlanningsDto>();
        public List<SelectSalesOrderLinesDto> OrdersList = new List<SelectSalesOrderLinesDto>();
        public List<ProductGroupOrder> ProductGroupOrderList = new List<ProductGroupOrder>();
        public List<SelectedDays> SelectedDaysList = new List<SelectedDays>();
        private SfGrid<ListProductsDto> _HMYMDetailGrid;
        private SfGrid<ContractWaitingProducts> _TotalPendingDetailGrid;
        private SfGrid<SelectedDays> _SelectedDayDetailGrid;
        private SfGrid<ListProductionTrackingsDto> _ProductionDetailGrid;
        private SfGrid<ListOperationUnsuitabilityReportsDto> _FaultyDetailGrid;

        public int HMYMCriticalQuantity = 0;
        public int TotalPendingQuantity = 0;
        public int TotalOrdersQuantity = 0;
        public decimal TotalProducedQuantity = 0;
        public decimal TotalFaultyQuantity = 0;
        public decimal Productivity = 0;

        SfCalendar<DateTime> calendar;
        DateTime today = DateTime.Today;
        DateTime selectedDate = DateTime.Today;

        SelectShipmentPlanningsDto DataSource;
        SelectSalesOrderDto LineDataSource;

        public class ContractWaitingProducts
        {
            public Guid ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public Guid ProductionOrderID { get; set; }
            public string ProductionOrderNo { get; set; }
            public Guid CurrentAccountID { get; set; }
            public string CurrentAccountName { get; set; }
            public decimal Amount_ { get; set; }
            public decimal OccuredAmount { get; set; }
            public decimal WaitingAmount { get; set; }
            public int NumberofOperation { get; set; }
        }

        public class SelectedDays
        {
            public Guid ProductGroupID { get; set; }
            public string ProductGroupName { get; set; }
            public decimal PlannedAmount { get; set; }
        }

        public class ChartData
        {
            public string CurrentAccountCard { get; set; }
            public double Quantity { get; set; }
        }

        public class ProductGroupOrder
        {
            public Guid ProductGroupID { get; set; }
            public string ProductGroupName { get; set; }
            public string ValuePercent { get; set; }
            public decimal TotalQuantity { get; set; }
        }



        public bool HMYMDetailModalVisible = false;
        public bool TotalPendingDetailModalVisible = false;
        public bool SelectedDayDetailModalVisible = false;
        public bool WorkingTimeDetailModalVisible = false;
        public bool ProducedAndFaultyModalVisible = false;

        public List<ChartData> TopSales = new List<ChartData>();
        public List<SelectSalesOrderLinesDto> SalesOrderLinesList = new List<SelectSalesOrderLinesDto>();

        protected override async void OnInitialized()
        {

            #region Kritik stok seviyesinin altına düşen HM-YM

            HMYMProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.ProductType == Entities.Enums.ProductTypeEnum.HM || t.ProductType == Entities.Enums.ProductTypeEnum.YM && t.Amount < t.CriticalStockQuantity).ToList();

            HMYMCriticalQuantity = HMYMProductsList.Count();

            #endregion

            #region Fasonda bekleyen toplam ürün adedi

            TotalPendingProductsList = (await ContractTrackingFichesAppService.GetListAsync(new ListContractTrackingFichesParameterDto())).Data.ToList();

            TotalPendingQuantity = TotalPendingProductsList.Select(t => t.Amount_).Sum() - TotalPendingProductsList.Select(t => t.OccuredAmount_).Sum();


            #endregion

            #region Planlama listeleri

            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            ShipmentPlanningsList = (await ShipmentPlanningsAppService.ODGetListbyDateAsync(today)).Data.ToList();
           

            #endregion

            #region Üretim Adedi ve Hatalı Adet


            ProducedAndFaultyList = (await ProductionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.OperationStartDate == today).ToList();

            TotalProducedQuantity = (int)ProducedAndFaultyList.Select(t => t.ProducedQuantity).Sum();
            TotalFaultyQuantity = (int)ProducedAndFaultyList.Select((t) => t.FaultyQuantity).Sum();

            if(TotalProducedQuantity > 0)
            {

                Productivity = (100 - ((TotalFaultyQuantity / TotalProducedQuantity) * 100));
            }
            else
            {
                Productivity = 0;
            }

            ProductionList = (await ProductionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.ProductionTrackingTypes == Entities.Enums.ProductionTrackingTypesEnum.Operasyonda && t.OperationStartDate == today).ToList();

            FaultyList = (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.Date_ == today).ToList();
            #endregion

            Most5SalesGetList();
            SalesOrderGetList();

        }

        #region HMYM
        public void ShowHMYMDetailButtonClickedAsync()
        {
            HMYMDetailModalVisible = true;

        }
        public void HideHMYMDetailPopup()
        {
            HMYMDetailModalVisible = false;
        }
        #endregion

        #region TOTAL PENDING
        public async void ShowTotalPendingDetailButtonClickedAsync()
        {
            ContractWaitingProductsList.Clear();

            foreach (var pendingProduct in TotalPendingProductsList)
            {

                var qualityPlan = (await ContractQualityPlansAppService.GetAsync(pendingProduct.ContractQualityPlanID)).Data;

                int numberofOperation = 0;

                if (qualityPlan == null)
                {
                    qualityPlan = new SelectContractQualityPlansDto();
                }
                else
                {
                    numberofOperation = qualityPlan.SelectContractQualityPlanOperations.Count;
                }

                ContractWaitingProducts pendingModel = new ContractWaitingProducts
                {
                    ProductID = pendingProduct.ProductID.GetValueOrDefault(),
                    ProductCode = pendingProduct.ProductCode,
                    ProductName = pendingProduct.ProductName,
                    ProductionOrderID = pendingProduct.ProductionOrderID.GetValueOrDefault(),
                    ProductionOrderNo = pendingProduct.ProductionOrderNr,
                    CurrentAccountID = pendingProduct.CurrentAccountCardID.GetValueOrDefault(),
                    CurrentAccountName = pendingProduct.CurrentAccountCardName,
                    Amount_ = pendingProduct.Amount_,
                    OccuredAmount = pendingProduct.OccuredAmount_,
                    WaitingAmount = pendingProduct.Amount_ - pendingProduct.OccuredAmount_,
                    NumberofOperation = numberofOperation,

                };

                ContractWaitingProductsList.Add(pendingModel);
            }


            TotalPendingDetailModalVisible = true;
            await InvokeAsync(StateHasChanged);

        }
        public async void HideTotalPendingDetailPopup()
        {
            TotalPendingDetailModalVisible = false;
        }
        #endregion

        #region PLANNING LIST

        public void CustomDates(RenderDayCellEventArgs args)
        {
                args.IsDisabled = true;

                foreach (var planning in ShipmentPlanningsList)
                {
                    if (args.Date == planning.ShipmentPlanningDate)
                    {
                        args.IsDisabled = false;
                        args.CellData.ClassList += " personal-appointment";
                    }
                }
            

        }

        public async void ValuechangeHandler()
        {

            DataSource = (await ShipmentPlanningsAppService.ODGetbyDateAsync(selectedDate)).Data;

            SelectedDaysList.Clear();

            if(DataSource != null && DataSource.Id != Guid.Empty && DataSource.SelectShipmentPlanningLines != null  && DataSource.SelectShipmentPlanningLines.Count > 0)
            {
                foreach (var plannedShipment in DataSource.SelectShipmentPlanningLines)
                {

                    var product = (await ProductsAppService.GetAsync(plannedShipment.ProductID.GetValueOrDefault())).Data;


                    if (product.ProductType == Entities.Enums.ProductTypeEnum.MM)
                    {
                        SelectedDays selectedModel = new SelectedDays
                        {
                            ProductGroupID = product.ProductGrpID,
                            ProductGroupName = product.ProductGrp,
                            PlannedAmount = plannedShipment.PlannedQuantity,
                        };

                        SelectedDaysList.Add(selectedModel);
                    }

                }

                
            }
           
            SelectedDayDetailModalVisible = true;

        }
        public async void HideSelectedDayDetailPopup()
        {
            SelectedDayDetailModalVisible = false;

            ShipmentPlanningsList.Clear();

            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            ShipmentPlanningsList = (await ShipmentPlanningsAppService.ODGetListbyDateAsync(today)).Data.ToList();
        }

        #endregion

        #region MOST 5 SALES

        public async void Most5SalesGetList()
        {

            SalesOrderLinesList = (await SalesOrdersAppService.GetLineSelectListAsync()).Data.ToList();


            var groupedList = SalesOrderLinesList.GroupBy(t => t.CurrentAccountCardID).ToList();

            foreach (var group in groupedList)
            {
                decimal totalQuantity = group.Sum(t => t.Quantity);

                ChartData model = new ChartData
                {
                    CurrentAccountCard = group.Select(t => t.CurrentAccountCardName).FirstOrDefault(),
                    Quantity = (double)totalQuantity
                };

                TopSales.Add(model);
            }

            TopSales = TopSales.OrderByDescending(t => t.Quantity).Take(5).ToList();

        }
        #endregion

        #region SALES ORDER
        public async void SalesOrderGetList()
        {

            OrdersList = (await SalesOrdersAppService.ODGetLineOrderstListAsync()).Data.Where(t => t.SalesOrderLineStateEnum == Entities.Enums.SalesOrderLineStateEnum.Beklemede || t.SalesOrderLineStateEnum == Entities.Enums.SalesOrderLineStateEnum.Onaylandı).ToList();

            var groupedOrderList = OrdersList.GroupBy(t => t.ProductGroupID).ToList();

            decimal allQuantities = OrdersList.Sum(t => t.Quantity);

            foreach (var item in groupedOrderList)
            {

                ProductGroupOrder productGroupOrderModel = new ProductGroupOrder
                {
                    ProductGroupID = item.Select(t => t.ProductGroupID).FirstOrDefault().GetValueOrDefault(),
                    ProductGroupName = item.Select(t => t.ProductGroupName).FirstOrDefault(),
                    TotalQuantity = item.Sum(t => t.Quantity),
                    ValuePercent = ((item.Sum(t => t.Quantity) / allQuantities) * 100).ToString("N1") + "%"
                };

                ProductGroupOrderList.Add(productGroupOrderModel);
            }


        }
        #endregion

        #region PRODUCED AND FAULTY
        public async void ShowProducedAndFaultyDetailButtonClickedAsync()
        {
            ProducedAndFaultyModalVisible = true;
            await InvokeAsync(StateHasChanged);

        }
        public async void HideProducedAndFaultyDetailPopup()
        {
            ProducedAndFaultyModalVisible = false;
        }

        #endregion

    }

}


