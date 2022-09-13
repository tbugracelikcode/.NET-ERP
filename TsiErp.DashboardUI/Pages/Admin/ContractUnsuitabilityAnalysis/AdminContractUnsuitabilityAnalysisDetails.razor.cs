using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.ContractUnsuitabilityAnalysis
{
    public partial class AdminContractUnsuitabilityAnalysisDetails
    {
        List<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis> datacontract = new List<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis>();
        List<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis> datachart = new List<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis>();
        SfGrid<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-90);
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
        SfChart ChartInstance;
        string chartTitle = "Toplu Uygunsuzluk Analizi Grafiği";
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;

        #endregion

        protected override void OnInitialized()
        {
            datacontract = FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailed(dateStart, dateEnd, cariID);
            datachart = FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailedChart(dateStart, dateEnd, timeIndex, 5, cariID, total);
            selectedTimeIndex = timeIndex;
        }

        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, ComboboxUnsuitability> args)
        {
            selectedactionID = args.Value;

            StateHasChanged();
        }

        #region Component Metotları

        private void OnDateButtonClicked()
        {
            endDate = DateTime.Today;

            #region Zaman Seçimi

            if (selectedTimeIndex == 0)
            {
                startDate = DateTime.Today.AddDays(-330);
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
                chartTitle = "Hurda Analizi Grafiği";
            }
            else if (selectedactionID == 2)
            {
                chartTitle = "Red Analizi Grafiği";
            }
            else if (selectedactionID == 3)
            {
                chartTitle = "Olduğu Gibi Kullanılacak Analizi Grafiği";
            }
            else if (selectedactionID == 4)
            {
                chartTitle = "Düzeltilecek Analizi Grafiği";
            }
            else if (selectedactionID == 5)
            {
                chartTitle = "Toplu Uygunsuzluk Grafiği";
            }
            #endregion

            Grid.Refresh();
            ChartInstance.RefreshAsync();
            datacontract = FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailed(startDate, endDate, cariID);
            total = FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate).Where(t => t.ContractSupplierID == cariID).Select(t => t.ContractReceiptQuantity).FirstOrDefault();
            datachart = FasonUygunsuzlukDetayService.GetContractUnsuitabilityDetailedChart(startDate, endDate, frequencyChart, selectedactionID, cariID, total);
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
