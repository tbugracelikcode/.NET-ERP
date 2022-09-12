using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.ProductionUnsuitabilityAnalysis
{
    public partial class AdminProductionUnsuitabilityAnalysis
    {
        List<TsiErp.DashboardUI.Models.ProductionUnsuitabilityAnalysis> dataprodunsuitability = new List<TsiErp.DashboardUI.Models.ProductionUnsuitabilityAnalysis>();
        List<TsiErp.DashboardUI.Models.AdminProductionUnsuitabilityAnalysisChart> datachart = new List<TsiErp.DashboardUI.Models.AdminProductionUnsuitabilityAnalysisChart>();
        SfGrid<TsiErp.DashboardUI.Models.ProductionUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        private int? selectedTimeIndex { get; set; }
        private int? selectedActionIndex { get; set; }
        int? selectedactionID = 4;
        private int threshold;
        private double thresholddouble;
        string chartTitle = "Toplu Uygunsuzluk Grafiği";
        private int frequencyChart;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;

        #endregion

        protected override void OnInitialized()
        {
            dataprodunsuitability = UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate);
            datachart = UretimUygunsuzlukService.GetProductionUnsuitabilityChart(startDate, endDate, 3, 4);
        }
        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, ComboboxUnsuitability> args)
        {
            selectedactionID = args.Value;

            StateHasChanged();
        }

        #region Component Metotları

        private void OnDateButtonClicked()
        {
            //VisibleSpinner = true;
            endDate = DateTime.Today;
            #region Zaman Seçimi
            if (selectedTimeIndex == 0)
            {
                startDate = DateTime.Today.AddDays(-365);
                frequencyChart = 0;
            }
            else if (selectedTimeIndex == 1)
            {
                startDate = DateTime.Today.AddDays(-273);
                frequencyChart = 1;
            }
            else if (selectedTimeIndex == 2)
            {
                startDate = DateTime.Today.AddDays(-181);
                frequencyChart = 2;
            }
            else if (selectedTimeIndex == 3)
            {
                startDate = DateTime.Today.AddDays(-90);
                frequencyChart = 3;
            }
            else if (selectedTimeIndex == 4)
            {
                startDate = DateTime.Today.AddDays(-60);
                frequencyChart = 4;
            }
            else if (selectedTimeIndex == 5)
            {
                startDate = DateTime.Today.AddDays(-30);
                frequencyChart = 5;
            }
            else if (selectedTimeIndex == 6)
            {
                startDate = DateTime.Today.AddDays(-7);
                frequencyChart = 6;
            }
            #endregion

            #region Aksiyon Seçimi
            if (selectedactionID == 1)
            {
                chartTitle = "Hurda Grafiği";
            }
            else if (selectedactionID == 2)
            {
                chartTitle = "Düzeltme Grafiği";
            }
            else if (selectedactionID == 3)
            {
                chartTitle = "Olduğu Gibi Kullanılacak Grafiği";
            }
            else if (selectedactionID == 4)
            {
                chartTitle = "Toplu Uygunsuzluk Grafiği";
            }
            #endregion

            thresholddouble = Convert.ToDouble(threshold) / 100;
            Grid.Refresh();
            ChartInstance.RefreshAsync();
            dataprodunsuitability = UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate);
            datachart = UretimUygunsuzlukService.GetProductionUnsuitabilityChart(startDate, endDate, frequencyChart, selectedactionID);
            StateHasChanged();
            //VisibleSpinner = false;
        }

        private void OnDetailButtonClicked(string unsuitabilityCode)
        {
            if (selectedactionID == null) { selectedactionID = 4; }
            NavigationManager.NavigateTo("/admin/production-unsuitability-analysis/details" + "/" + unsuitabilityCode + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd") + "/" + selectedactionID.ToString()); ;
        }

        private void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = true; }
            else { dataLabels = false; }
        }

        #endregion

        #region Combobox

        private List<ComboboxTimePeriods> timeperiods = new List<ComboboxTimePeriods>() {
        new ComboboxTimePeriods(){ TimeID= 1, TimeText= "Yıllık" },
        new ComboboxTimePeriods(){ TimeID= 2, TimeText= "Son 9 Ay" },
        new ComboboxTimePeriods(){ TimeID= 3, TimeText= "Son 6 Ay" },
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "Son 3 Ay" },
        new ComboboxTimePeriods(){ TimeID= 5, TimeText= "Son 2 Ay" },
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "Son 1 Ay" },
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "Son 1 Hafta" }
     };

        private List<ComboboxUnsuitability> unsuitabilities = new List<ComboboxUnsuitability>() {
        new ComboboxUnsuitability(){ TypeID= 1, TypeText= "Hurda" },
        new ComboboxUnsuitability(){ TypeID= 2, TypeText= "Düzeltme" },
        new ComboboxUnsuitability(){ TypeID= 3, TypeText= "Olduğu Gibi Kullanılacak" },
        new ComboboxUnsuitability(){ TypeID= 4, TypeText= "Hepsini Göster" }
     };

        #endregion

    }
}
