using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using System.Dynamic;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.ProductGroupPerformanceAnalysis
{
    public partial class AdminProductGroupPerformanceAnalysis
    {
        List<Models.ProductGroupPerformanceAnalysis> dataproductperformance = new List<Models.ProductGroupPerformanceAnalysis>();
        List<AdminProductGroupPerformanceAnalysisChart> datachart = new List<AdminProductGroupPerformanceAnalysisChart>();
        List<ProductGroupsAnalysis> dataproductgroupcombobox = new List<ProductGroupsAnalysis>();
        //public List<ExpandoObject> GridMonthsExpando = new List<ExpandoObject>();

        SfGrid<Models.ProductGroupPerformanceAnalysis> Grid;


        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int? selectedProductIndex { get; set; }
        int? selectedproductID;
        private int threshold = 75;
        private double thresholddouble = 0.75;
        private int frequencyChart;
        SfChart ChartInstance;
        private bool isGridChecked = true;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] { };

        #endregion

        protected override async void OnInitialized()
        {

            dataproductperformance = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysis(startDate, endDate, 9, 0);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            datachart = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysisChart(startDate, endDate, 0, 9);
            //GenerateNewColumn();
        }

        //public  List<ExpandoObject> GenerateNewColumn()
        //{
        //    var data = new List<ExpandoObject>();

        //    int colCount = datachart.Count();

        //    string[] ColNames = new string[colCount];


        //    int a = 0;

        //    foreach (var item in datachart)
        //    {
        //        ColNames[a] = item.Month;

        //        dynamic month = new ExpandoObject();

        //        var dict = (IDictionary<string, object>)month;

        //        dict[ColNames[a]] = item.Month;

        //        data.Add(month);

        //        a++;
        //    }

        //    return data;

            
        //}

        #region Component Metotları

        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, ProductGroupsAnalysis> args)
        {
            selectedproductID = args.Value;

            StateHasChanged();
        }

        private async void OnDateButtonClicked()
        {
            VisibleSpinner = true;
            await Task.Delay(1);
            StateHasChanged();

            #region Zaman Seçimi
            switch (selectedTimeIndex)
            {
                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); frequencyChart = 0; break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); frequencyChart = 1; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); frequencyChart = 2; break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); frequencyChart = 3; break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); frequencyChart = 4; break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); frequencyChart = 5; break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); frequencyChart = 6; break;
                default: break;
            }

            #endregion

            thresholddouble = Convert.ToDouble(threshold) / 100;
            dataproductperformance = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysis(startDate, endDate, selectedproductID, frequencyChart);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            datachart = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysisChart(startDate, endDate, frequencyChart, selectedproductID);
            //GenerateNewColumn();
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

        private void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = true; }
            else { dataLabels = false; }
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

        public void CellInfoHandler(QueryCellInfoEventArgs<Models.ProductGroupPerformanceAnalysis> Args)
        {
            if (Args.Column.Field == "Performance")
            {
                if (Args.Data.Performance < Convert.ToDecimal(thresholddouble))
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
