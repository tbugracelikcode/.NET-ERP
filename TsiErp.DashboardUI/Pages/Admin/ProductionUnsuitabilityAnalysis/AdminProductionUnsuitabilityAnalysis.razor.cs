using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.ProductionUnsuitabilityAnalysis
{
    public partial class AdminProductionUnsuitabilityAnalysis
    {
        List<Models.ProductionUnsuitabilityAnalysis> dataprodunsuitability = new List<Models.ProductionUnsuitabilityAnalysis>();
        List<AdminProductionUnsuitabilityAnalysisChart> datachart = new List<AdminProductionUnsuitabilityAnalysisChart>();
        SfGrid<Models.ProductionUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int? selectedActionIndex { get; set; }
        int? selectedactionID = 4;
        private bool isGridChecked = true;
        string chartTitle = "Genel Uygunsuzluk Grafiği";
        private int frequencyChart;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] { };
        public string unsuitabilityTitle = "Genel Uygunsuzluk Oranı:";
        string chartAverageLabel = "Yıllık Ortalama Değer :";
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

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

                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); frequencyChart = 0; chartAverageLabel = "Yıllık Ortalama Değer: "; break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); frequencyChart = 1; chartAverageLabel = "9 Aylık Ortalama Değer: "; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); frequencyChart = 2; chartAverageLabel = "6 Aylık Ortalama Değer: "; break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); frequencyChart = 3; chartAverageLabel = "3 Aylık Ortalama Değer: "; break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); frequencyChart = 4; chartAverageLabel = "2 Aylık Ortalama Değer: "; break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); frequencyChart = 5; chartAverageLabel = "1 Aylık Ortalama Değer: "; break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); frequencyChart = 6; chartAverageLabel = "1 Haftalık Ortalama Değer: "; break;
                default: break;
            }

            #endregion

            #region Aksiyon Seçimi
            switch(selectedactionID)
            {
                case 1: chartTitle = "Hurda Grafiği"; unsuitabilityTitle = "Hurda Oranı:"; break;
                case 2: chartTitle = "Düzeltme Grafiği"; unsuitabilityTitle = "Düzeltme Oranı:"; break;
                case 3: chartTitle = "Olduğu Gibi Kullanılacak Grafiği"; unsuitabilityTitle = "Olduğu Gibi Kullanılacak Oranı:"; break;
                case 4: chartTitle = "Genel Uygunsuzluk Grafiği"; unsuitabilityTitle = "Genel Uygunsuzluk Oranı:"; break;
            }

            #endregion

            
            dataprodunsuitability = await UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate);
            datachart = await UretimUygunsuzlukService.GetProductionUnsuitabilityChart(startDate, endDate, frequencyChart, selectedactionID);
            VisibleSpinner = false;
            StateHasChanged();
            await Grid.Refresh();
            await ChartInstance.RefreshAsync();

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

        private async void OnCompareButtonClicked()
        {
            ShowCompareModal();
        }

        private async void ShowCompareModal()
        {
            compareModalVisible = true;
        }

        private async void HideCompareModal()
        {
            compareModalVisible = false;
            MultiSelectVal = null;
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
