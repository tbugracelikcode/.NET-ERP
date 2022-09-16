using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;


namespace TsiErp.DashboardUI.Pages.Admin.MachineAnalysis
{
    public partial class AdminMachineAnalysisDetails
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
        private bool isGridChecked = true;
        double columnwidth2;
        double columnwidth3;

        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public int stationID { get; set; }

        #endregion


        protected override async void OnInitialized()
        {

            dataproduct = await IstasyonDetayService.GetStationDetailedProductAnalysis(stationID, startDate, endDate);
            dataemployee = await IstasyonDetayService.GetStationDetailedEmployeeAnalysis(stationID, startDate, endDate);
            datahalt = await IstasyonDetayService.GetStationDetailedHaltAnalysis(stationID, startDate, endDate);
            datahaltchart = await IstasyonDetayService.GetStationDetailedHaltAnalysisChart(stationID, startDate, endDate);
            dataproductchart = await IstasyonDetayService.GetStationDetailedProductChart(stationID, startDate, endDate, 1);
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

        private async void OnChangeProductCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (isProductsChecked) { products = 2; }
            else { products = 1; }
            dataproductchart = await IstasyonDetayService.GetStationDetailedProductChart(stationID, startDate, endDate, products);
            await ChartInstance.RefreshAsync();
        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }

        private void OnBackButtonClicked()
        {
            this.VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/overall-oee");
        }
    }
}
