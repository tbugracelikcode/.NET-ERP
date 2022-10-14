using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.MachineAnalysis
{
    public partial class AdminMachineAnalysis
    {
        List<StationOEEAnalysis> dataoee = new List<StationOEEAnalysis>();
        List<AdminMachineChart> datachart = new List<AdminMachineChart>();
        SfGrid<StationOEEAnalysis> Grid;

        #region Değişkenler

       DateTime startDate = DateTime.Today.AddDays(-(365 + DateTime.Today.Day));

        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int threshold = 75;
        private double thresholddouble = 0.75;
        private int frequencyChart;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] {};


        #endregion

        protected async override void OnInitialized()
        {
            dataoee = await IstasyonOEEService.GetStationOEEAnalysis(startDate, endDate);
            datachart = await IstasyonOEEService.GetAdminMachineChart(startDate, endDate, 0);


        }

        #region Component Metotları

        private async void OnDateButtonClicked()
        {
            VisibleSpinner = true;
            await Task.Delay(100);
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

            thresholddouble = Convert.ToDouble(threshold) / 100;

            datachart = await IstasyonOEEService.GetAdminMachineChart(startDate, endDate, frequencyChart);
            dataoee = await IstasyonOEEService.GetStationOEEAnalysis(startDate, endDate);

            await ChartInstance.RefreshAsync();
            VisibleSpinner = false;
            StateHasChanged();
        }

        private void OnDetailButtonClicked(int stationID)
        {
            VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/machine-analysis/details" + "/" + stationID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd")); ;
        }

        private void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = true; }
            else { dataLabels = false; }
        }

        private async void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {

            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;
            StateHasChanged();
        }

        private async void OnCompareButtonClicked()
        {
            ShowCompareModal();
        }

        private async void ShowCompareModal ()
        {
            compareModalVisible = true;
        }

        private async void HideCompareModal()
        {
            compareModalVisible = false;
            MultiSelectVal = null;
        }

        

        #endregion

        public void CellInfoHandler(QueryCellInfoEventArgs<StationOEEAnalysis> Args)
        {
            if (Args.Column.Field == "OEE")
            {
                if (Args.Data.OEE < Convert.ToDecimal(thresholddouble))
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #DF0000; color: white; " });
                }
                else
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #37CB00; color: white;" });
                }
            }
            StateHasChanged();
        }

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
