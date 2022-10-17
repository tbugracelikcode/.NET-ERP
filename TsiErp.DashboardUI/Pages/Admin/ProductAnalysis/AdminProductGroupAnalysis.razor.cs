using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.ProductAnalysis
{
    public partial class AdminProductGroupAnalysis
    {
        List<ProductGroupsAnalysis> dataproductgroup = new List<ProductGroupsAnalysis>();
        List<ProductGroupsAnalysis> dataproductgroupcombobox = new List<ProductGroupsAnalysis>();
        List<AdminProductChart> datachart = new List<AdminProductChart>();
        SfGrid<ProductGroupsAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int? selectedProductIndex { get; set; }
        int? selectedproductID;
        string chartTitle = string.Empty;
        private bool isGridChecked = true;
        private int frequencyChart;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] { };
        string chartAverageLabel = "Yıllık Ortalama Değer :";

        #endregion

        protected override async void OnInitialized()
        {

            dataproductgroup = await StokService.GetProductGroupsAnalysis(startDate, endDate);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            chartTitle =  dataproductgroup.Where(t => t.ProductGroupID == 9).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";
            datachart = await StokService.GetProductChart(startDate, endDate, 0, 9);

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


            dataproductgroup = await StokService.GetProductGroupsAnalysis(startDate, endDate);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            datachart = await StokService.GetProductChart(startDate, endDate, frequencyChart, selectedproductID);
            chartTitle = dataproductgroup.Where(t => t.ProductGroupID == selectedproductID).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";
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

        private void OnDetailButtonClicked(int stationID)
        {
            VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/product-group-analysis/details" + "/" + stationID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd")); ;
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
