using DevExpress.Utils;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;


namespace TsiErp.DashboardUI.Pages.Admin.ContractUnsuitabilityAnalysis
{
    public partial class AdminContractUnsuitabilityAnalysis
    {
        List<Models.ContractUnsuitabilityAnalysis> datacontunsuitability = new List<Models.ContractUnsuitabilityAnalysis>();
        List<Models.ContractUnsuitabilityAnalysis> datacontunsuitabilityChart = new List<Models.ContractUnsuitabilityAnalysis>();
        SfGrid<Models.ContractUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private bool isGridChecked = true;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private int threshold = 50;
        private double thresholddouble = 0.50;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion

        protected async override void OnInitialized()
        {
            

            datacontunsuitability = await FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);
            var chartList = datacontunsuitability.Where(t => t.Total > 0).ToList();
            foreach (var item in chartList)
            {
                if (item.ContractSupplier.Length > 15)
                {
                    item.ContractShortSupplier = item.ContractSupplier.Substring(0, 15) + "...";
                }
                else
                {
                    item.ContractShortSupplier = item.ContractSupplier;
                }
            }
            datacontunsuitabilityChart = chartList;
        }

        public void CellInfoHandler(QueryCellInfoEventArgs<Models.ContractUnsuitabilityAnalysis> Args)
        {
            if (Args.Column.Field == "Percent")
            {
                if (Args.Data.Percent < thresholddouble)
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #37CB00; color: white; " });
                }
                else
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #DF0000; color: white;" });
                }
            }
            StateHasChanged();
        }

        #region Component Metotları

        private async void OnDateButtonClicked()
        {
            VisibleSpinner = true;
            await Task.Delay(1);
            StateHasChanged();


            #region Zaman Seçimi
            switch(selectedTimeIndex)
            {
                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); ;break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); ; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); break;
                default:break;
            }

            #endregion

            thresholddouble = Convert.ToDouble(threshold) / 100;

            datacontunsuitability = await FasonUygunsuzlukService.GetContractUnsuitabilityAnalysis(startDate, endDate);
            var chartList = datacontunsuitability.Where(t => t.Total > 0).ToList();
            foreach (var item in chartList)
            {
                if (item.ContractSupplier.Length > 15)
                {
                    item.ContractShortSupplier = item.ContractSupplier.Substring(0, 15) + "...";
                }
                else
                {
                    item.ContractShortSupplier = item.ContractSupplier;
                }
            }
            datacontunsuitabilityChart = chartList;

            
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

        private async void OnDetailButtonClicked(int cariID, int total)
        {
            VisibleSpinner = true;

            if (selectedTimeIndex == null)
            {
                selectedTimeIndex = 0;
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
