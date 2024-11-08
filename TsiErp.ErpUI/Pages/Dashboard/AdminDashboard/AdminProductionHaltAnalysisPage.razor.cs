using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.XlsIO;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.Dashboard.AdminDashboard.AdminProductGroupAnalysisPage;
using BlazorBootstrap;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using static TsiErp.ErpUI.Pages.Dashboard.OperationalDashboard.Global;
using TsiErp.Business.Entities.SalesOrder.Services;
using TsiErp.ErpUI.Services.Dashboard;
using Syncfusion.Blazor.Cards;
using System;

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminProductionHaltAnalysisPage
    {
        List<AdminProductionHaltAnalysisChart> HaltAnalysisList = new List<AdminProductionHaltAnalysisChart>();

        List<AdminProductionHaltAnalysisPieChart> HaltAnalysisPieList = new List<AdminProductionHaltAnalysisPieChart>();


        public List<HaltReasonItem> HaltReasonItemList = new List<HaltReasonItem>();
        public Guid ComboBoxValue = Guid.Empty;

        [Inject]
        SpinnerService Spinner { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }


        #region Değişkenler
        int comboBox = 0;
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        DateTime dailyStartDate = new DateTime();
        DateTime dailyEndDate = new DateTime();
        DateTime weeklyStartDate = new DateTime();
        DateTime weeklyEndDate = new DateTime();
        private int? selectedTimeIndex { get; set; } = 0;
        private int threshold = 75;
        private double thresholddouble = 0.75;
        SfChart ChartInstance;
        string ChartTitle = string.Empty;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        string chartAverageLabel = string.Empty;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion


        public bool DetailModalVisible = false;

        public class HaltReasonItem
        {
            public string HaltReasonName { get; set; }

            public string ValuePercent { get; set; }
            public decimal TotalQuantity { get; set; }
        }

        protected override async void OnInitialized()
        {
            var today = GetSQLDateAppService.GetDateFromSQL().Date;


            startDate = today.AddDays(-(89));
            endDate = today;

            chartAverageLabel = L["ComboboxLast3Months"];

            HaltAnalysisList = (await AdminDashboardAppService.GetProductionHaltChart(startDate, endDate, 3));

            HaltAnalysisPieList = (await AdminDashboardAppService.GetProductionHaltPieChart(startDate, endDate, 3));


            if (HaltAnalysisList != null && HaltAnalysisList.Count > 0)
            {
                foreach (var halt in HaltAnalysisList)
                {
                    if (selectedTimeIndex != 6 && selectedTimeIndex != 5)
                    {
                        halt.TIME = L[halt.TIME];
                    }
                    else if (selectedTimeIndex == 5)
                    {
                        DateTime weeklyStartDate = DateTime.Today.AddDays(-7);
                        DateTime weeklyEndDate = DateTime.Today;

                        halt.TIME = weeklyStartDate.ToString("dd MMMM yyyy") + " - " + weeklyEndDate.ToString("dd MMMM yyyy"); 
                    }
                    else
                    {
                        DateTime dailyDate = DateTime.Today;
                        halt.TIME = L[halt.TIME];
                    }
                   

                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyList2Message"]);
            }

            foreach (var item in timeperiods)
            {
                item.TimeText = L[item.TimeText];
            }


            HaltReasonGetList();
            await InvokeAsync(StateHasChanged);


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

                case 0: startDate = today.AddDays(-(364)); comboBox = 3; chartAverageLabel = L["ChartAverageLabelAnnual"]; break;
                case 1: startDate = today.AddDays(-(272)); comboBox = 3; chartAverageLabel = L["ChartAverageLabel9Months"]; break;
                case 2: startDate = today.AddDays(-(180)); comboBox = 3; chartAverageLabel = L["ChartAverageLabel6Months"]; break;
                case 3: startDate = today.AddDays(-(89)); comboBox = 3; chartAverageLabel = L["ChartAverageLabel3Months"]; break;
                case 4: startDate = today.AddDays(-(30)); comboBox = 3; chartAverageLabel = L["ChartAverageLabel1Month"]; break;
                case 5: startDate = today.AddDays(-(7)); comboBox = 2; chartAverageLabel = L["ChartAverageLabel1Week"]; break;
                case 6: startDate = today.AddDays(-(1)); comboBox = 1; chartAverageLabel = L["ChartAverageLabelDaily"]; break;

                default: break;
            }

            #endregion

            HaltAnalysisList = (await AdminDashboardAppService.GetProductionHaltChart(startDate, endDate, comboBox));

            HaltAnalysisPieList = (await AdminDashboardAppService.GetProductionHaltPieChart(startDate, endDate, comboBox));


            if (HaltAnalysisList != null && HaltAnalysisList.Count > 0)
            {
                foreach (var halt in HaltAnalysisList)
                {
                    if (comboBox == 3)
                    {
                        halt.TIME = L[halt.TIME];

                    }
                    else if (comboBox == 2)
                    {
                        DateTime weeklyStartDate = DateTime.Today.AddDays(-7);
                        DateTime weeklyEndDate = DateTime.Today;

                        halt.TIME = weeklyStartDate.ToString("dd MMMM yyyy") + " - " + weeklyEndDate.ToString("dd MMMM yyyy");

                    }
                    else
                    {
                        DateTime dailyDate = DateTime.Today;
                        halt.TIME = L[halt.TIME];
                    }

                }
                Spinner.Hide();
                await ChartInstance.RefreshAsync();
            }
            else
            {
                Spinner.Hide();
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyList2Message"]);
            }


            await InvokeAsync(StateHasChanged);

        }

        public async void HaltReasonGetList()
        {

            HaltAnalysisPieList = (await AdminDashboardAppService.GetProductionHaltPieChart(startDate, endDate, comboBox)).ToList();
            var haltreasonList = HaltAnalysisPieList.GroupBy(t => t.HALTTYPE).ToList();

            decimal allQuantities = HaltAnalysisPieList.Sum(t => t.HALTTIME);

            foreach (var item in haltreasonList)
            {

                HaltReasonItem haltReasonModel = new HaltReasonItem
                {
                    HaltReasonName = item.Select(t => t.HALTTYPE).FirstOrDefault(),
                    TotalQuantity = item.Sum(t => t.HALTTIME),
                    ValuePercent = (allQuantities == 0 ? 0 : ((item.Sum(t => t.HALTTIME) / allQuantities) * 100)).ToString("N1") + "%"
                };


                HaltReasonItemList.Add(haltReasonModel);
            }

        }

        public void ShowDetailButtonClickedAsync()
        {
            DetailModalVisible = true;

        }
        public void HideDetailPopup()
        {
            DetailModalVisible = false;
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
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "ComboboxLast3Months" },
        new ComboboxTimePeriods(){ TimeID= 5, TimeText= "ComboboxLast1Month" }, //3
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "ComboboxWeekly" }, // 2
        new ComboboxTimePeriods(){ TimeID= 7, TimeText= "ComboboxDaily" } // 1
     };

        #endregion
    }
}
