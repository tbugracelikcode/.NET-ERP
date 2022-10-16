using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.ContractUnsuitabilityAnalysis
{
    public partial class AdminContractUnsuitabilityAnalysisDetails
    {
        List<Models.ContractUnsuitabilityAnalysis> datacontract = new List<Models.ContractUnsuitabilityAnalysis>();
        List<Models.ContractUnsuitabilityAnalysis> datachart = new List<Models.ContractUnsuitabilityAnalysis>();
        List<Models.ContractUnsuitabilityAnalysis> dataconuns = new List<Models.ContractUnsuitabilityAnalysis>();
        SfGrid<Models.ContractUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(330 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today;
        [Parameter]
        public DateTime dateStart { get; set; }
        [Parameter]
        public DateTime dateEnd { get; set; }
        [Parameter]
        public int cariID { get; set; }
        [Parameter]
        public int timeIndex { get; set; }
        [Parameter]
        public int total { get; set; }

        private int? selectedTimeIndex { get; set; }
        private int? selectedActionIndex { get; set; }
        int? selectedactionID = 5;
        private int frequencyChart;
        private bool isGridChecked = true;
        SfChart ChartInstance;
        string chartTitle = "Genel Uygunsuzluk Analizi Grafiği";
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] { };
        public string unsuitabilityTitle = "Genel Uygunsuzluk Oranı:";
        string chartAverageLabel = string.Empty;
        string chartAverageValue = string.Empty;

        #endregion

        protected async override void OnInitialized()
        {

            datacontract = await FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailed(dateStart, dateEnd, cariID);
            datachart = await FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailedChart(dateStart, dateEnd, timeIndex, 5, cariID, total);
            selectedTimeIndex = timeIndex;
            chartAverageValue = datachart.Average(t => t.Percent).ToString("p2");
            switch (selectedTimeIndex)
            {
                case 0: chartAverageLabel = "Yıllık Ortalama Değer: "; break;
                case 1: chartAverageLabel = "9 Aylık Ortalama Değer: "; break;
                case 2: chartAverageLabel = "6 Aylık Ortalama Değer: "; break;
                case 3: chartAverageLabel = "3 Aylık Ortalama Değer: "; break;
                case 4: chartAverageLabel = "2 Aylık Ortalama Değer: "; break;
                case 5: chartAverageLabel = "1 Aylık Ortalama Değer: "; break;
                case 6: chartAverageLabel = "1 Haftalık Ortalama Değer: "; break;
                default: break;
            }


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

            endDate = DateTime.Today;

            #region Zaman Seçimi
            switch(selectedTimeIndex)
            {
                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); frequencyChart = 0; chartAverageLabel = "Yıllık Ortalama Değer: "; break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); frequencyChart = 1; chartAverageLabel = "9 Aylık Ortalama Değer: "; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); frequencyChart = 2; chartAverageLabel = "6 Aylık Ortalama Değer: "; break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); frequencyChart = 3; chartAverageLabel = "3 Aylık Ortalama Değer: "; break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); frequencyChart = 4; chartAverageLabel = "2 Aylık Ortalama Değer: "; break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); frequencyChart = 5; chartAverageLabel = "1 Aylık Ortalama Değer: "; break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); frequencyChart = 6; chartAverageLabel = "1 Haftalık Ortalama Değer: "; break;
                default:break;
            }
           

            #endregion

            #region Aksiyon Seçimi
            switch(selectedactionID)
            {
                case 1: chartTitle = "Hurda Analizi Grafiği"; unsuitabilityTitle = "Hurda Oranı:"; break;
                case 2: chartTitle = "Red Analizi Grafiği"; unsuitabilityTitle = "Red Oranı:"; break;
                case 3: chartTitle = "Olduğu Gibi Kullanılacak Analizi Grafiği"; unsuitabilityTitle = "Olduğu Gibi Kullanılacak Oranı:"; break;
                case 4: chartTitle = "Düzeltilecek Analizi Grafiği"; unsuitabilityTitle = "Düzetilecek Oranı:"; break;
                case 5: chartTitle = "Genel Uygunsuzluk Grafiği"; unsuitabilityTitle = "Genel Uygunsuzluk Oranı:"; break;
            }
            
            #endregion

            datacontract = await FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailed(startDate, endDate, cariID);
            dataconuns = await FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);
            total = dataconuns.Where(t => t.ContractSupplierID == cariID).Select(t => t.ContractReceiptQuantity).FirstOrDefault();
            datachart = await FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailedChart(startDate, endDate, frequencyChart, selectedactionID, cariID, -1);
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
        private void OnBackButtonClicked()
        {
            NavigationManager.NavigateTo("/admin/contract-unsuitability-analysis"); ;
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
        new ComboboxUnsuitability(){ TypeID= 2, TypeText= "Red" },
        new ComboboxUnsuitability(){ TypeID= 3, TypeText= "Olduğu Gibi Kullanılacak" },
        new ComboboxUnsuitability(){ TypeID= 4, TypeText= "Düzeltme" },
        new ComboboxUnsuitability(){ TypeID= 5, TypeText= "Hepsini Göster" }
     };

        #endregion

    }
}
