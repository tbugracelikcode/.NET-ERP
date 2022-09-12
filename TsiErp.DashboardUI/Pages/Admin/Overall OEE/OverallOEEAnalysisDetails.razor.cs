using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.Overall_OEE
{
    public partial class OverallOEEAnalysisDetails
    {
        List<StationDetailedProductAnalysis> dataproduct = new List<StationDetailedProductAnalysis>();
        List<StationDetailedEmployeeAnalysis> dataemployee = new List<StationDetailedEmployeeAnalysis>();
        List<StationDetailedHaltAnalysis> datahalt = new List<StationDetailedHaltAnalysis>();
        List<StationDetailedProductChart> dataproductchart = new List<StationDetailedProductChart>();
        List<StationDetailedHaltAnalysis> datahaltchart = new List<StationDetailedHaltAnalysis>();

        SfGrid<StationDetailedProductAnalysis> ProductGrid;
        SfGrid<StationDetailedEmployeeAnalysis> EmployeeGrid;
        SfGrid<StationDetailedHaltAnalysis> HaltGrid;
        SfChart ChartInstance;

        #region Değişkenler

        bool VisibleSpinner = false;
        private bool isProductsChecked = false;
        int products = 1;
        string stationName = string.Empty;
        double columnwidth1;
        double columnwidth2;
        double columnwidth3;

        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public int stationID { get; set; }

        #endregion


        protected override void OnInitialized()
        {
            dataproduct = IstasyonDetayService.GetStationDetailedProductAnalysis(stationID, startDate, endDate);
            dataemployee = IstasyonDetayService.GetStationDetailedEmployeeAnalysis(stationID, startDate, endDate);
            datahalt = IstasyonDetayService.GetStationDetailedHaltAnalysis(stationID, startDate, endDate);
            datahaltchart = IstasyonDetayService.GetStationDetailedHaltAnalysisChart(stationID, startDate, endDate);
            dataproductchart = IstasyonDetayService.GetStationDetailedProductChart(stationID, startDate, endDate, 1);
            stationName = datahalt.Select(t => t.StationName).FirstOrDefault();

            #region Sütun Genişlikleri

            if (dataemployee.Count() <= 3)
            {
                columnwidth3 = 0.1;
            }
            if (datahaltchart.Count() <= 3)

            {
                columnwidth2 = 0.1;
            }

            #endregion
        }

        private void OnChangeProductCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (isProductsChecked) { products = 2; }
            else { products = 1; }
            dataproductchart = IstasyonDetayService.GetStationDetailedProductChart(stationID, startDate, endDate, products);
            ChartInstance.RefreshAsync();
        }

        private void OnBackButtonClicked()
        {
            this.VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/machine-analysis");
        }
    }
}
