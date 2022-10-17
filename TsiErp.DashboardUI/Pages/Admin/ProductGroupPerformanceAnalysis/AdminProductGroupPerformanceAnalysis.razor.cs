using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using System.Dynamic;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.ProductGroupPerformanceAnalysis
{
    public partial class AdminProductGroupPerformanceAnalysis
    {
        List<Models.ProductGroupPerformanceAnalysis> dataproductperformance2 = new List<Models.ProductGroupPerformanceAnalysis>();
        List<ExpandoObject> dataproductperformance = new List<ExpandoObject>();
        List<AdminProductGroupPerformanceAnalysisChart> datachart = new List<AdminProductGroupPerformanceAnalysisChart>();
        List<ProductGroupsAnalysis> dataproductgroupcombobox = new List<ProductGroupsAnalysis>();
        public List<ExpandoObject> GridMonthsExpando = new List<ExpandoObject>();

        SfGrid<ExpandoObject> Grid;


        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int? selectedProductIndex { get; set; }
        int? selectedproductID = 9;
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
        string chartAverageLabel = "Yıllık Ortalama Değer :";

        #endregion

        protected override async void OnInitialized()
        {

            //dataproductperformance = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysis(startDate, endDate, 9, 0);
            datachart = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysisChart(startDate, endDate, 0, 9);

            dataproductperformance = GenerateNewColumn();
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
        }

        public List<ExpandoObject> GenerateNewColumn()
        {
            var data = new List<ExpandoObject>();

            int colCount = datachart.Count;

            string[] ColNames = datachart.Select(t => t.Month).ToArray();
            int[] ColMonthindex = datachart.Select(t => t.THmonth).ToArray();
            int[] ColYearindex = datachart.Select(t => t.Year).ToArray();

            var operationList = DBHelper.GetOperationLinesQuery(startDate,endDate).Where(t=>t.URUNGRPID == selectedproductID).ToList();
            var istList = operationList.Select(t=>t.ISTASYONID).Distinct().ToList();

            for (int i = 0; i < istList.Count; i++)
            {
                int istID = istList[i];

                string istKodu = operationList.Where(t=>t.ISTASYONID == istID).Select(t=>t.MAKINEKODU).FirstOrDefault();

                var obj = new ExpandoObject() as IDictionary<string, object>;

                obj.Add("İSTASYON", istKodu);
                var OprLines = DBHelper.GetOperationLinesStationQuery(istID, startDate, endDate).Where(t => t.URUNGRPID == selectedproductID).ToList();

                for (int a = 0; a < colCount; a++)
                {
                    //var tempOprLines = OprLines.GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t =>
                    //{
                    //    decimal planlananBirimSure = t.Sum(t => t.PLANLANANOPRSURESI);
                    //    decimal gerceklesenBirimSure = t.Sum(t => t.BIRIMSURE);
                    //    decimal performans = gerceklesenBirimSure > 0 ? planlananBirimSure / gerceklesenBirimSure : 0;
                    //    return performans;
                    //}).ToList();

                    var tempOprLines = OprLines.Where(t => t.TARIH.Month == (ColMonthindex[a]) && t.TARIH.Year == ColYearindex[a]).ToList();

                    decimal planlananBirimSure = tempOprLines.Sum(t => t.PLANLANANOPRSURESI);
                    decimal gerceklesenBirimSure = tempOprLines.Sum(t => t.BIRIMSURE);
                    decimal performans = gerceklesenBirimSure > 0 ? planlananBirimSure / gerceklesenBirimSure : 0;
                    obj.Add(ColNames[a], performans);

                    Models.ProductGroupPerformanceAnalysis model = new Models.ProductGroupPerformanceAnalysis()
                    {
                        Month = ColNames[a],
                        Performance = performans,
                        StationCode = istKodu
                    };
                    dataproductperformance2.Add(model);
                }                

                data.Add(obj as dynamic);

            }

            return data;

        }

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
                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); frequencyChart = 0; chartAverageLabel = "Yıllık Ortalama Değer: "; break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); frequencyChart = 1; chartAverageLabel = "9 Aylık Ortalama Değer: "; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); frequencyChart = 2; chartAverageLabel = "6 Aylık Ortalama Değer: "; break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); frequencyChart = 3; chartAverageLabel = "3 Aylık Ortalama Değer: "; break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); frequencyChart = 4; chartAverageLabel = "2 Aylık Ortalama Değer: "; break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); frequencyChart = 5; chartAverageLabel = "1 Aylık Ortalama Değer: "; break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); frequencyChart = 6; chartAverageLabel = "1 Haftalık Ortalama Değer: "; break;
                default: break;
            }

            #endregion

            thresholddouble = Convert.ToDouble(threshold) / 100;
            //dataproductperformance = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysis(startDate, endDate, selectedproductID, frequencyChart);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            datachart = await UrunGrubuPerformansService.GetProductGroupPerformanceAnalysisChart(startDate, endDate, frequencyChart, selectedproductID);
            dataproductperformance = GenerateNewColumn();
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
