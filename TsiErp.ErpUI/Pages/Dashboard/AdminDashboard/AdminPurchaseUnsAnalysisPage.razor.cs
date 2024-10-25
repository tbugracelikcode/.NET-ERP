using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminPurchaseUnsAnalysisPage : IDisposable
    {
        [Inject]
        SpinnerService Spinner { get; set; }
        [Inject]
        ModalManager ModalManager { get; set; }

        SfGrid<AdminPurchaseUnsuitabilityGrid> Grid;


        List<AdminPurchaseUnsuitabilityChart> PurchaseUnsuitabilityChartList = new List<AdminPurchaseUnsuitabilityChart>();

        List<AdminPurchaseUnsuitabilityGrid> PurchaseUnsuitabilityGridList = new List<AdminPurchaseUnsuitabilityGrid>();

        List<AdminPurchaseUnsuitabilityGrid> PurchaseUnsuitabilityGridDisplayList = new List<AdminPurchaseUnsuitabilityGrid>();

        List<ComboboxTimePeriods> ComboboxTimePeriodsList = new List<ComboboxTimePeriods>();

        #region Değişkenler

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        string chartAverageLabel = string.Empty;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };
        ComboboxTimePeriods selectedTimePeriod;

        #endregion

        protected override async void OnInitialized()
        {
            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            startDate = today.AddDays(-(364 + today.Day));
            endDate = today.AddDays(-(today.Day));

            chartAverageLabel = L["ChartAverageLabelAnnual"];

            PurchaseUnsuitabilityChartList = (await AdminDashboardAppService.GetAdminPurchaseUnsuitabilityChart(startDate, endDate));

            if (PurchaseUnsuitabilityChartList != null && PurchaseUnsuitabilityChartList.Count > 0)
            {
                foreach (var unsChart in PurchaseUnsuitabilityChartList)
                {
                    unsChart.MONTH = L[unsChart.MONTH] + " " + unsChart.YEAR.ToString();
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }

            PurchaseUnsuitabilityGridList = (await AdminDashboardAppService.GetAdminPurchaseUnsuitabilityGrid(startDate, endDate));

            if (PurchaseUnsuitabilityGridList != null && PurchaseUnsuitabilityGridList.Count > 0)
            {
                foreach (var unsGrid in PurchaseUnsuitabilityGridList)
                {

                    if (!ComboboxTimePeriodsList.Any(t => t.TimeText == L[unsGrid.MONTH] + " " + unsGrid.YEAR.ToString()))
                    {
                        ComboboxTimePeriods timePeriod = new ComboboxTimePeriods
                        {
                            TimeID = ComboboxTimePeriodsList.Count + 1,
                            Month = unsGrid.MONTH,
                            Year = unsGrid.YEAR,
                            TimeText = L[unsGrid.MONTH] + " " + unsGrid.YEAR.ToString()
                        };

                        ComboboxTimePeriodsList.Add(timePeriod);
                    }


                    unsGrid.MONTH = L[unsGrid.MONTH] + " " + unsGrid.YEAR.ToString();

                }

                string firstMonth = PurchaseUnsuitabilityGridList.Select(t => t.MONTH).FirstOrDefault();

                PurchaseUnsuitabilityGridDisplayList = PurchaseUnsuitabilityGridList.Where(t => t.MONTH == firstMonth).ToList();

                selectedTimePeriod = ComboboxTimePeriodsList.FirstOrDefault();
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }

            isGridChecked = false;
        }

        #region Component Metotları
        private async void OnDateButtonClicked()
        {
            Spinner.Show();

            await Task.Delay(100);

            var today = GetSQLDateAppService.GetDateFromSQL().Date;


            PurchaseUnsuitabilityGridDisplayList = PurchaseUnsuitabilityGridList.Where(t => t.MONTH == selectedTimePeriod.TimeText).ToList();

            Spinner.Hide();

            await Grid.Refresh();
            await ChartInstance.RefreshAsync();


            await InvokeAsync(StateHasChanged);

        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }


        #endregion

        #region Combobox

        public class ComboboxTimePeriods
        {
            public int TimeID { get; set; }
            public string Month { get; set; }
            public int Year { get; set; }
            public string TimeText { get; set; }
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
