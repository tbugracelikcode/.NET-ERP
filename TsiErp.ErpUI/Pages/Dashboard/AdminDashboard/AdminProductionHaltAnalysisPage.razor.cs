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
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Business.Entities.HaltReason.Services;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.CostManagement.CostPeriodLine.Dtos;
using DevExpress.ClipboardSource.SpreadsheetML;
using Microsoft.SqlServer.Management.Smo;
using DevExpress.Blazor.Popup.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminProductionHaltAnalysisPage
    {
        List<AdminProductionHaltAnalysisChart> HaltAnalysisList = new List<AdminProductionHaltAnalysisChart>();

        List<AdminProductionHaltAnalysisPieChart> HaltAnalysisPieList = new List<AdminProductionHaltAnalysisPieChart>();


        List<HaltReasonItem> HaltReasonItemList = new List<HaltReasonItem>();

        public List<HaltReasonItem> HaltReasonOperatorList = new List<HaltReasonItem>();
        public List<HaltReasonItem> HaltReasonMachineList = new List<HaltReasonItem>();
        public List<HaltReasonItem> HaltReasonManagementList = new List<HaltReasonItem>();

        //DENEME
        List<AdminProductionHaltAnalysisPieChartDetail> DistinctedDetailsList = new List<AdminProductionHaltAnalysisPieChartDetail>();
        List<AdminProductionHaltAnalysisPieChartDetail> DistinctedYearDetailsList = new List<AdminProductionHaltAnalysisPieChartDetail>();
        List<AdminProductionHaltAnalysisPieChartDetail> AllDetailsList = new List<AdminProductionHaltAnalysisPieChartDetail>();
        List<AdminProductionHaltAnalysisPieChartDetail> AllDetailsListList = new List<AdminProductionHaltAnalysisPieChartDetail>();


        private SfGrid<HaltReasonItem> _Grid;



        public Guid ComboBoxValue = Guid.Empty;

        [Inject]
        SpinnerService Spinner { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }


        #region Değişkenler
        int comboBox = 0;
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        //DateTime dailyStartDate = new DateTime();
        //DateTime dailyEndDate = new DateTime();
        //DateTime weeklyStartDate = new DateTime();
        //DateTime weeklyEndDate = new DateTime();
        private int? selectedTimeIndex { get; set; } = 3;
        private int threshold = 75;
        private double thresholddouble = 0.75;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        string chartAverageLabel = string.Empty;
        string chartPieDetailLabel = string.Empty;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion


        public bool DetailModalVisible = false;

        public class HaltReasonItem
        {
            public string HaltReasonName { get; set; }

            public string ValuePercent { get; set; }
            public decimal TotalQuantity { get; set; }

            public string TitleName { get; set; }
            public decimal Quantity { get; set; }
            public decimal Time_ { get; set; }
            public string TimePeriod { get; set; }
        }


        protected override async void OnInitialized()
        {
            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            startDate = today.AddDays(-(89));
            endDate = today;

            comboBox = 3;

            chartAverageLabel = L["ComboboxLast3Months"];

            HaltAnalysisList = (await AdminDashboardAppService.GetProductionHaltChart(startDate, endDate, 3));


            if (HaltAnalysisList != null && HaltAnalysisList.Count > 0)
            {

                foreach (var halt in HaltAnalysisList)
                {
                    if (selectedTimeIndex != 6 && selectedTimeIndex != 5)
                    {
                        halt.TIME = L[halt.TIME] + " " + halt.YEAR.ToString();
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


            HaltReasonGetList(3);
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

            HaltReasonGetList(comboBox);


            if (HaltAnalysisList != null && HaltAnalysisList.Count > 0)
            {
                foreach (var halt in HaltAnalysisList)
                {

                    if (comboBox == 3)
                    {
                        halt.TIME = L[halt.TIME] + " " + halt.YEAR.ToString();

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

        public async void HaltReasonGetList(int comboBox)
        {
            HaltAnalysisPieList.Clear();
            AllDetailsList.Clear();
            DistinctedDetailsList.Clear();
            HaltReasonItemList.Clear();
            DistinctedYearDetailsList.Clear();

            HaltAnalysisPieList = (await AdminDashboardAppService.GetProductionHaltPieChart(startDate, endDate, comboBox)).ToList();

            foreach (var haltlist in HaltAnalysisPieList)
            {
                DistinctedDetailsList.AddRange(haltlist.AdminProductionHaltAnalysisPieChartDetail);
                DistinctedYearDetailsList.AddRange(haltlist.AdminProductionHaltAnalysisPieChartDetail);
                AllDetailsList.AddRange(haltlist.AdminProductionHaltAnalysisPieChartDetail);
                AllDetailsListList.AddRange(haltlist.AdminProductionHaltAnalysisPieChartDetail);

            }

            DistinctedDetailsList = DistinctedDetailsList.DistinctBy(t => t.TIME).ToList();
            DistinctedYearDetailsList = DistinctedYearDetailsList.DistinctBy(t => t.YEAR).ToList();

            DistinctedDetailsList = DistinctedDetailsList.OrderBy(t => t.TIME).ToList();
            DistinctedYearDetailsList = DistinctedYearDetailsList.OrderBy(t => t.YEAR).ToList();

            foreach (var yearperiod in DistinctedYearDetailsList)
            {
                var yearList = DistinctedDetailsList.Where(t => t.YEAR == yearperiod.YEAR);

                if (yearList.Count() > 0)
                {
                    foreach (var timeperiod in yearList)
                    {
                        var periodsRecords = AllDetailsList.Where(t => t.TIME == timeperiod.TIME && t.YEAR == timeperiod.YEAR).ToList();

                        decimal allQuantities = periodsRecords.Sum(t => t.HALTTIME);

                        var managementList = periodsRecords.Where(t => t.HALTTYPE == L["ManagementHaltType"]).ToList();

                        var operatorList = periodsRecords.Where(t => t.HALTTYPE == L["OperatorHaltType"]).ToList();

                        var machineList = periodsRecords.Where(t => t.HALTTYPE == L["MachineHaltType"]).ToList();

                        if (comboBox == 3)
                        {
                            timeperiod.TIME = L[timeperiod.TIME] + " " + timeperiod.YEAR;
                        }

                        HaltReasonItem operatorhaltReasonModel = new HaltReasonItem
                        {
                            HaltReasonName = L["OperatorHaltType"],
                            TotalQuantity = operatorList.Sum(t => t.HALTTIME),
                            ValuePercent = (allQuantities == 0 ? 0 : ((operatorList.Sum(t => t.HALTTIME) / allQuantities) * 100)).ToString("N1") + "%",
                            TimePeriod = timeperiod.TIME,
                        };

                        HaltReasonItem machinehaltReasonModel = new HaltReasonItem
                        {
                            HaltReasonName = L["MachineHaltType"],
                            TotalQuantity = machineList.Sum(t => t.HALTTIME),
                            ValuePercent = (allQuantities == 0 ? 0 : ((machineList.Sum(t => t.HALTTIME) / allQuantities) * 100)).ToString("N1") + "%",
                            TimePeriod = timeperiod.TIME,
                        };

                        HaltReasonItem managementhaltReasonModel = new HaltReasonItem
                        {
                            HaltReasonName = L["ManagementHaltType"],
                            TotalQuantity = managementList.Sum(t => t.HALTTIME),
                            ValuePercent = (allQuantities == 0 ? 0 : ((managementList.Sum(t => t.HALTTIME) / allQuantities) * 100)).ToString("N1") + "%",
                            TimePeriod = timeperiod.TIME,
                        };

                        HaltReasonItemList.Add(operatorhaltReasonModel);
                        HaltReasonItemList.Add(machinehaltReasonModel);
                        HaltReasonItemList.Add(managementhaltReasonModel);

                    }
                }


            }

        }

        public async void ShowDetailButtonClickedAsync(string time)
        {
            HaltAnalysisPieList.Clear(); 
            AllDetailsListList.Clear();
            HaltReasonOperatorList.Clear();
            HaltReasonMachineList.Clear();
            HaltReasonManagementList.Clear();

            HaltAnalysisPieList = (await AdminDashboardAppService.GetProductionHaltPieChart(startDate, endDate, comboBox)).ToList();

            foreach (var haltlist in HaltAnalysisPieList)
            {
                AllDetailsListList.AddRange(haltlist.AdminProductionHaltAnalysisPieChartDetail);

            }


            if (comboBox == 3)
            {
                foreach (var alldetail in AllDetailsListList)
                {
                    alldetail.TIME = L[alldetail.TIME] + " " + alldetail.YEAR;
                }
            }
           
            chartPieDetailLabel = time;

            var relatedperiod = AllDetailsListList.Where(t => t.TIME == time).ToList();

            foreach (var item in relatedperiod)
            {
                var type = item.HALTTYPE;

                if (type == L["OperatorHaltType"])
                {
                    HaltReasonItem haltReasonModel = new HaltReasonItem
                    {
                        TitleName = item.TITLENAME,
                        Time_ = item.HALTTIME,
                        Quantity = item.QUANTITY,

                    };

                    HaltReasonOperatorList.Add(haltReasonModel);

                }
                else if (type == L["MachineHaltType"])
                {
                    HaltReasonItem haltReasonModel = new HaltReasonItem
                    {
                        TitleName = item.TITLENAME,
                        Time_ = item.HALTTIME,
                        Quantity = item.QUANTITY,
                    };

                    HaltReasonMachineList.Add(haltReasonModel);


                }
                else
                {
                    HaltReasonItem haltReasonModel = new HaltReasonItem
                    {

                        TitleName = item.TITLENAME,
                        Time_ = item.HALTTIME,
                        Quantity = item.QUANTITY,
                    };

                    HaltReasonManagementList.Add(haltReasonModel);
                }

            }

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
