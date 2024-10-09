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

namespace TsiErp.ErpUI.Pages.Dashboard.OperationalDashboard
{
    public partial class Global
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ListProductsDto> HMYMProductsList = new List<ListProductsDto>();
        public List<ListContractTrackingFichesDto> TotalPendingProductsList = new List<ListContractTrackingFichesDto>();
        public List<ContractWaitingProducts> ContractWaitingProductsList = new List<ContractWaitingProducts>();
        public List<ListShipmentPlanningsDto> ShipmentPlanningsList = new List<ListShipmentPlanningsDto>();
        public List<SelectedDays> SelectedDaysList = new List<SelectedDays>();
        private SfGrid<ListProductsDto> _HMYMDetailGrid;
        private SfGrid<ContractWaitingProducts> _TotalPendingDetailGrid;
        private SfGrid<SelectedDays> _SelectedDayDetailGrid;
        public int HMYMCriticalQuantity = 0;
        public int TotalPendingQuantity = 0;

        SfCalendar<DateTime> calendar;
        DateTime today = DateTime.Today;
        DateTime selectedDate = DateTime.Today;

        SelectShipmentPlanningsDto DataSource;

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


        public bool HMYMDetailModalVisible = false;
        public bool TotalPendingDetailModalVisible = false;
        public bool SelectedDayDetailModalVisible = false;

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

            today = GetSQLDateAppService.GetDateFromSQL().Date;

            ShipmentPlanningsList = (await ShipmentPlanningsAppService.ODGetListbyDateAsync(today)).Data.ToList();
            #endregion
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
            foreach(var planning in ShipmentPlanningsList)
            {
                if (args.Date == planning.ShipmentPlanningDate)
                {
                    args.CellData.ClassList += " personal-appointment";
                }
            }
        }

        public async void ValuechangeHandler()
        {
            DataSource = (await ShipmentPlanningsAppService.ODGetbyDateAsync(selectedDate)).Data;

            SelectedDaysList.Clear();
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
            SelectedDayDetailModalVisible = true;

        }
        public async void HideSelectedDayDetailPopup()
        {
            SelectedDayDetailModalVisible = false;
        }

        #endregion

    }

}


