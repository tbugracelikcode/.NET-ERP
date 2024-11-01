using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using Syncfusion.XlsIO;
using TsiErp.Business.Entities.Dashboard.AdminDashboard;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminContUnsAnalysisPage : IDisposable
    {
        [Inject]
        SpinnerService Spinner { get; set; }
        [Inject]
        ModalManager ModalManager { get; set; }

        SfGrid<AdminContractUnsuitabilityGrid> Grid;


        List<AdminContractUnsuitabilityChart> ContractUnsuitabilityChartList = new List<AdminContractUnsuitabilityChart>();

        List<AdminContractUnsuitabilityGrid> ContractUnsuitabilityGridList = new List<AdminContractUnsuitabilityGrid>();

        List<AdminContractUnsuitabilityGrid> ContractUnsuitabilityGridDisplayList = new List<AdminContractUnsuitabilityGrid>();

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

            ContractUnsuitabilityChartList = (await AdminDashboardAppService.GetAdminContractUnsuitabilityChart(startDate, endDate));

            if (ContractUnsuitabilityChartList != null && ContractUnsuitabilityChartList.Count > 0)
            {
                foreach (var unsChart in ContractUnsuitabilityChartList)
                {
                    unsChart.MONTH = L[unsChart.MONTH] + " " + unsChart.YEAR.ToString();
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }

            ContractUnsuitabilityGridList = (await AdminDashboardAppService.GetAdminContractUnsuitabilityGrid(startDate, endDate));

            if (ContractUnsuitabilityGridList != null && ContractUnsuitabilityGridList.Count > 0)
            {
                foreach (var unsGrid in ContractUnsuitabilityGridList)
                {

                    if(!ComboboxTimePeriodsList.Any(t=>t.TimeText == L[unsGrid.MONTH] + " " + unsGrid.YEAR.ToString()))
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

                string firstMonth = ContractUnsuitabilityGridList.Select(t => t.MONTH).FirstOrDefault();

                ContractUnsuitabilityGridDisplayList = ContractUnsuitabilityGridList.Where(t => t.MONTH == firstMonth).ToList();

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


            ContractUnsuitabilityGridDisplayList = ContractUnsuitabilityGridList.Where(t => t.MONTH == selectedTimePeriod.TimeText).ToList();

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
