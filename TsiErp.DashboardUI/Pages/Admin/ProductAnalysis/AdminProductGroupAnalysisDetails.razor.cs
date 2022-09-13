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
        string productgroupName = string.Empty;

        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public int productgroupID { get; set; }
        #endregion


        protected override void OnInitialized()
        {
            dataproductscrap = StokDetayService.GetProductScrapAnalysis(productgroupID, startDate, endDate);
            dataproductgroupchart = StokDetayService.GetProductGroupDetailedtChart(productgroupID, startDate, endDate, 1);
            productgroupName = dataproductscrap.Select(t => t.ProductGroupName).FirstOrDefault();
            if (dataproductgroupchart.Count() < 3)
            {
                columnwidth = 0.1;
            }
        }

        private void OnChangeProductCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (isProductGroupsChecked) { productgroups = 2; }
            else { productgroups = 1; }
            dataproductgroupchart = StokDetayService.GetProductGroupDetailedtChart(productgroupID, startDate, endDate, productgroups);
            ChartInstance.RefreshAsync();
        }

        private void OnBackButtonClicked()
        {
            this.VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/product-group-analysis");
        }
    }
}
