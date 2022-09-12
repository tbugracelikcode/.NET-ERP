using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;


namespace TsiErp.DashboardUI.Pages.Admin.ContractUnsuitabilityAnalysis
{
    public partial class AdminContractUnsuitabilityAnalysis
    {
        List<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis> datacontunsuitability = new List<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis>();
        SfGrid<TsiErp.DashboardUI.Models.ContractUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        private int? selectedTimeIndex { get; set; }
        private int threshold;
        private double thresholddouble;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;

        #endregion

        protected override void OnInitialized()
        {
            datacontunsuitability = FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);
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
            }
            else if (selectedTimeIndex == 1)
            {
                startDate = DateTime.Today.AddDays(-273);
            }
            else if (selectedTimeIndex == 2)
            {
                startDate = DateTime.Today.AddDays(-181);
            }
            else if (selectedTimeIndex == 3)
            {
                startDate = DateTime.Today.AddDays(-90);
            }
            else if (selectedTimeIndex == 4)
            {
                startDate = DateTime.Today.AddDays(-60);
            }
            else if (selectedTimeIndex == 5)
            {
                startDate = DateTime.Today.AddDays(-30);
            }
            else if (selectedTimeIndex == 6)
            {
                startDate = DateTime.Today.AddDays(-7);
            }
            #endregion

            thresholddouble = Convert.ToDouble(threshold) / 100;
            Grid.Refresh();
            ChartInstance.RefreshAsync();
            datacontunsuitability = FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);
            StateHasChanged();
            //VisibleSpinner = false;
        }

        private void OnDetailButtonClicked(int cariID, int total)
        {
            if (selectedTimeIndex == null)
            {
                selectedTimeIndex = 3;
            }
            NavigationManager.NavigateTo("/admin/contract-unsuitability-analysis/details" + "/" + cariID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd") + "/" + selectedTimeIndex.ToString() + "/" + total.ToString()); ;
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

        #endregion
    }
}
