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

        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        private int? selectedTimeIndex { get; set; }
        private int? selectedProductIndex { get; set; }
        private int threshold;
        int? selectedproductID;
        string chartTitle = string.Empty;
        private int frequencyChart;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;

        #endregion

        protected override void OnInitialized()
        {
            dataproductgroup = StokService.GetProductGroupsAnalysis(startDate, endDate);
            dataproductgroupcombobox = StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            chartTitle = dataproductgroup.Where(t => t.ProductGroupID == 9).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";
            datachart = StokService.GetProductChart(startDate, endDate, 3, 9);
        }
        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, ProductGroupsAnalysis> args)
        {
            selectedproductID = args.Value;

            StateHasChanged();
        }

        #region Component Metotları

        private void OnDateButtonClicked()
        {
            endDate = DateTime.Today;

            #region Zaman Seçimi

            if (selectedTimeIndex == 0)
            {
                startDate = DateTime.Today.AddDays(-365);
                frequencyChart = 0;
            }
            else if (selectedTimeIndex == 1)
            {
                startDate = DateTime.Today.AddDays(-273);
                frequencyChart = 1;
            }
            else if (selectedTimeIndex == 2)
            {
                startDate = DateTime.Today.AddDays(-181);
                frequencyChart = 2;
            }
            else if (selectedTimeIndex == 3)
            {
                startDate = DateTime.Today.AddDays(-90);
                frequencyChart = 3;
            }
            else if (selectedTimeIndex == 4)
            {
                startDate = DateTime.Today.AddDays(-60);
                frequencyChart = 4;
            }
            else if (selectedTimeIndex == 5)
            {
                startDate = DateTime.Today.AddDays(-30);
                frequencyChart = 5;
            }
            else if (selectedTimeIndex == 6)
            {
                startDate = DateTime.Today.AddDays(-7);
                frequencyChart = 6;
            }

            #endregion

            Grid.Refresh();
            ChartInstance.RefreshAsync();
            dataproductgroup = StokService.GetProductGroupsAnalysis(startDate, endDate);
            dataproductgroupcombobox = StokService.GetProductGroupsComboboxAnalysis(startDate, endDate);
            datachart = StokService.GetProductChart(startDate, endDate, frequencyChart, selectedproductID);
            chartTitle = dataproductgroup.Where(t => t.ProductGroupID == selectedproductID).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";
            StateHasChanged();
        }

        private void OnDetailButtonClicked(int stationID)
        {
            NavigationManager.NavigateTo("/admin/product-group-analysis/details" + "/" + stationID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd")); ;
        }

        private void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = true; }
            else { dataLabels = false; }
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
