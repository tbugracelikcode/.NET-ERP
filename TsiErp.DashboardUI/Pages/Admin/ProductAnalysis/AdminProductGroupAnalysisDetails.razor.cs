using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.ProductAnalysis
{
    public partial class AdminProductGroupAnalysisDetails
    {
        List<ProductScrapAnalysis> dataproductscrap = new List<ProductScrapAnalysis>();
        List<ProductGroupDetailedChart> dataproductgroupchart = new List<ProductGroupDetailedChart>();
        SfGrid<ProductScrapAnalysis> ProductGroupGrid;
        SfChart ChartInstance;

        #region Değişkenler

        bool VisibleSpinner = false;
        private bool isProductGroupsChecked = false;
        int productgroups = 1;
        double columnwidth;
        private bool isGridChecked = true;
        string productgroupName = string.Empty;

        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public int productgroupID { get; set; }
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };
        #endregion


        protected override async void OnInitialized()
        {

            dataproductscrap = await StokDetayService.GetProductScrapAnalysis(productgroupID, startDate, endDate);
            dataproductgroupchart = await StokDetayService.GetProductGroupDetailedtChart(productgroupID, startDate, endDate, 1);
            productgroupName = dataproductscrap.Select(t => t.ProductGroupName).FirstOrDefault();
            if (dataproductgroupchart.Count() < 3)
            {
                columnwidth = 0.1;
            }
        }

        private async void OnChangeProductCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (isProductGroupsChecked) { productgroups = 2; }
            else { productgroups = 1; }
            dataproductgroupchart = await StokDetayService.GetProductGroupDetailedtChart(productgroupID, startDate, endDate, productgroups);
            await ChartInstance.RefreshAsync();
        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }
        private void OnBackButtonClicked()
        {
            this.VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/product-group-analysis");
        }
    }
}
