using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.Overall_OEE
{
    public partial class OverallOEEAnalysis
    {
        List<StationOEEAnalysis> dataoee = new List<StationOEEAnalysis>();
        List<AdminMachineChart> datachart = new List<AdminMachineChart>();
        SfGrid<StationOEEAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(365 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; } = 0;
        private int threshold = 75;
        private double thresholddouble = 0.75;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] { };

        #endregion

        protected override async void OnInitialized()
        {

            dataoee = await GenelOEEService.GetStationOEEAnalysis(startDate, endDate);
            datachart = await GenelOEEService.GetAdminMachineChart(startDate, endDate);

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
                case 0: startDate = DateTime.Today.AddDays(-365); ; break;
                case 1: startDate = DateTime.Today.AddDays(-273); ; break;
                case 2: startDate = DateTime.Today.AddDays(-181); ; break;
                case 3: startDate = DateTime.Today.AddDays(-90); ; break;
                default: break;
            }

            #endregion
            

            thresholddouble = Convert.ToDouble(threshold) / 100;
            dataoee = await GenelOEEService.GetStationOEEAnalysis(startDate, endDate);
            datachart = await GenelOEEService.GetAdminMachineChart(startDate, endDate);
            await Grid.Refresh();
            await ChartInstance.RefreshAsync();
            VisibleSpinner = false;
            StateHasChanged();
        }

       

        //private void OnDetailButtonClicked(int stationID)
        //{
        //    VisibleSpinner = true;

        //    NavigationManager.NavigateTo("/admin/machine-analysis/details" + "/" + stationID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd")); ;
        //}

        private async void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            await ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = true; }
            else { dataLabels = false; }
        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
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
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "Son 3 Ay" }
     };

        #endregion

    }
}
