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

        DateTime startDate = DateTime.Today.AddDays(-(365 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int? selectedActionIndex { get; set; }
        int? selectedactionID = 4;
        private bool isGridChecked = true;
        string chartTitle = "Toplu Uygunsuzluk Grafiği";
        private int frequencyChart;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;

        #endregion

        protected override async void OnInitialized()
        {

            dataprodunsuitability =await UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate);
            datachart = await UretimUygunsuzlukService.GetProductionUnsuitabilityChart(startDate, endDate, 0, 4);

        }
        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, ComboboxUnsuitability> args)
        {
            selectedactionID = args.Value;

            StateHasChanged();
        }

        #region Component Metotları

        private async void OnDateButtonClicked()
        {
            VisibleSpinner = true;
            await Task.Delay(1);
            StateHasChanged();


            #region Zaman Seçimi
            switch (selectedTimeIndex)
            {
                case 0: startDate = DateTime.Today.AddDays(-365); frequencyChart = 0; break;
                case 1: startDate = DateTime.Today.AddDays(-273); frequencyChart = 1; break;
                case 2: startDate = DateTime.Today.AddDays(-181); frequencyChart = 2; break;
                case 3: startDate = DateTime.Today.AddDays(-90); frequencyChart = 3; break;
                case 4: startDate = DateTime.Today.AddDays(-60); frequencyChart = 4; break;
                case 5: startDate = DateTime.Today.AddDays(-30); frequencyChart = 5; break;
                case 6: startDate = DateTime.Today.AddDays(-7); frequencyChart = 6; break;
                default: break;
            }

            #endregion

            #region Aksiyon Seçimi
            switch(selectedactionID)
            {
                case 1: chartTitle = "Hurda Grafiği";break;
                case 2: chartTitle = "Düzeltme Grafiği"; break;
                case 3: chartTitle = "Olduğu Gibi Kullanılacak Grafiği"; break;
                case 4: chartTitle = "Toplu Uygunsuzluk Grafiği"; break;
            }

            #endregion

            
            dataprodunsuitability = await UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate);
            datachart = await UretimUygunsuzlukService.GetProductionUnsuitabilityChart(startDate, endDate, frequencyChart, selectedactionID);
            await Grid.Refresh();
            await ChartInstance.RefreshAsync();
            VisibleSpinner = false;
            StateHasChanged();
            
        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }

        private void OnDetailButtonClicked(string unsuitabilityCode)
        {
            VisibleSpinner = true;

            if (selectedactionID == null) { selectedactionID = 0; }
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
