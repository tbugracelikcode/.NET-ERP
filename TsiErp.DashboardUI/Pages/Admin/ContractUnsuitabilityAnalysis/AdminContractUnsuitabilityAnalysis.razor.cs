using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;


namespace TsiErp.DashboardUI.Pages.Admin.ContractUnsuitabilityAnalysis
{
    public partial class AdminContractUnsuitabilityAnalysis
    {
        List<Models.ContractUnsuitabilityAnalysis> datacontunsuitability = new List<Models.ContractUnsuitabilityAnalysis>();
        SfGrid<Models.ContractUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        private int? selectedTimeIndex { get; set; }
        private bool isGridChecked = true;
        SfChart ChartInstance;
        bool VisibleSpinner = false;

        #endregion

        protected async override void OnInitialized()
        {
            

            datacontunsuitability = await FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);

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
                case 0: startDate = DateTime.Today.AddDays(-365); ;break;
                case 1: startDate = DateTime.Today.AddDays(-273); ; break;
                case 2: startDate = DateTime.Today.AddDays(-181); ; break;
                case 3: startDate = DateTime.Today.AddDays(-90); ; break;
                case 4: startDate = DateTime.Today.AddDays(-60); ; break;
                case 5: startDate = DateTime.Today.AddDays(-30); ; break;
                case 6: startDate = DateTime.Today.AddDays(-7); ; break;
                default:break;
            }
           
            #endregion

            datacontunsuitability = await FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);
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

        private async void OnDetailButtonClicked(int cariID, int total)
        {
            VisibleSpinner = true;

            if (selectedTimeIndex == null)
            {
                selectedTimeIndex = 3;
            }
             NavigationManager.NavigateTo("/admin/contract-unsuitability-analysis/details" + "/" + cariID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd") + "/" + selectedTimeIndex.ToString() + "/" + total.ToString()); 
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
