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

        DateTime startDate = DateTime.Today.AddDays(-(365 + DateTime.Today.Day));
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

        #endregion

        protected override async void OnInitialized()
        {

            dataproductgroup = await StokService.GetProductGroupsAnalysis(startDate, endDate);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            chartTitle =  dataproductgroup.Where(t => t.ProductGroupID == 9).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";
            datachart = await StokService.GetProductChart(startDate, endDate, 0, 9);

        }
        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, ProductGroupsAnalysis> args)
        {
            selectedproductID = args.Value;

            StateHasChanged();
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


            dataproductgroup = await StokService.GetProductGroupsAnalysis(startDate, endDate);
            dataproductgroupcombobox = await StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            datachart = await StokService.GetProductChart(startDate, endDate, frequencyChart, selectedproductID);
            chartTitle = dataproductgroup.Where(t => t.ProductGroupID == selectedproductID).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";
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
