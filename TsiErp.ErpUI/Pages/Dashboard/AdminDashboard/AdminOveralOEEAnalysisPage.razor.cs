using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using System.Data;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminOveralOEEAnalysisPage
    {

        List<AdminOveralOEEChart> OveralOEEList = new List<AdminOveralOEEChart>();

        [Inject]
        SpinnerService Spinner {  get; set; }
        [Inject]
        ModalManager ModalManager { get; set; }

        SfGrid<AdminOveralOEEChart> Grid;

        #region Değişkenler

        DateTime startDate =new DateTime();
        DateTime endDate = new DateTime();
        private int? selectedTimeIndex { get; set; } = 0;
        private int threshold = 75;
        private double thresholddouble = 0.75;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        string chartAverageLabel = string.Empty;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion


        protected override async void OnInitialized()
        {
            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            startDate = today.AddDays(-(364 + today.Day));
            endDate = today.AddDays(-(today.Day));

            chartAverageLabel = L["ChartAverageLabelAnnual"];

            OveralOEEList = (await AdminDashboardAppService.GetAdminMachineChart(startDate, endDate));

            if(OveralOEEList != null && OveralOEEList.Count >  0)
            {
                foreach (var oee in OveralOEEList)
                {
                    oee.MONTH = L[oee.MONTH] + " " + oee.YEAR.ToString();
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }

            isGridChecked = false;

            foreach (var item in timeperiods)
            {
                item.TimeText = L[item.TimeText];
            }
        }

        #region Component Metotları
        private async void OnDateButtonClicked()
        {
            Spinner.Show();

            await Task.Delay(100);

            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            #region Zaman Seçimi
            switch (selectedTimeIndex)
            {

                case 0: startDate = today.AddDays(-(364 + today.Day)); chartAverageLabel = L["ChartAverageLabelAnnual"]; break;
                case 1: startDate = today.AddDays(-(272 + today.Day)); chartAverageLabel = L["ChartAverageLabel9Months"]; break;
                case 2: startDate = today.AddDays(-(180 + today.Day)); chartAverageLabel = L["ChartAverageLabel6Months"]; break;
                case 3: startDate = today.AddDays(-(89 + today.Day)); chartAverageLabel = L["ChartAverageLabel3Months"]; break;
                default: break;
            }

            #endregion


            thresholddouble = Convert.ToDouble(threshold) / 100;

            OveralOEEList = (await AdminDashboardAppService.GetAdminMachineChart(startDate, endDate));

            if (OveralOEEList != null && OveralOEEList.Count > 0)
            {

                foreach (var oee in OveralOEEList)
                {
                    oee.MONTH = L[oee.MONTH] + " " + oee.YEAR.ToString();
                }
                Spinner.Hide();

                await Grid.Refresh();
                await ChartInstance.RefreshAsync();
            }
            else
            {
                Spinner.Hide();
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }

            await InvokeAsync(StateHasChanged);

        }

        private async void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            await ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = false; }
            else { dataLabels = true; }
        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }

        public void CellInfoHandler(QueryCellInfoEventArgs<AdminOveralOEEChart> Args)
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
            else if (Args.Column.Field == "DIFFOEE")
            {
                if (Args.Data.DIFFOEE < 0)
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

        #endregion

        #region Combobox

        public class ComboboxTimePeriods
        {
            public int TimeID { get; set; }
            public string TimeText { get; set; }
        }

        private List<ComboboxTimePeriods> timeperiods = new List<ComboboxTimePeriods>() {
        new ComboboxTimePeriods(){ TimeID= 1, TimeText= "ComboboxAnnual" },
        new ComboboxTimePeriods(){ TimeID= 2, TimeText= "ComboboxLast9Months" },
        new ComboboxTimePeriods(){ TimeID= 3, TimeText= "ComboboxLast6Months" },
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "ComboboxLast3Months" }
     };

        #endregion
    }
}
