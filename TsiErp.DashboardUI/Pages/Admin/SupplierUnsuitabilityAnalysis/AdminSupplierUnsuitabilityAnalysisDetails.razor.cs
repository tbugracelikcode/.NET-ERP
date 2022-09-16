using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.SupplierUnsuitabilityAnalysis
{
    public partial class AdminSupplierUnsuitabilityAnalysisDetails
    {
        List<SupplierUnsuitabilityDetailedCustomer> dataunsuitabilitycustomer = new List<SupplierUnsuitabilityDetailedCustomer>();
        List<SupplierUnsuitabilityDetailedProduct> dataunsuitabilityproduct = new List<SupplierUnsuitabilityDetailedProduct>();
        List<Models.SupplierUnsuitabilityAnalysis> datasuppuns = new List<Models.SupplierUnsuitabilityAnalysis>();

        SfGrid<SupplierUnsuitabilityDetailedCustomer> CustomerGrid;
        SfGrid<SupplierUnsuitabilityDetailedProduct> ProductGrid;

        #region Değişkenler

        bool VisibleSpinner = false;
        string unsuitabilityName = string.Empty;
        double columnwidth1;
        private bool isGridChecked = true;
        double columnwidth2;

        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public int errorID { get; set; }

        [Parameter]
        public int total { get; set; }

        #endregion


        protected override async void OnInitialized()
        {

            dataunsuitabilitycustomer = await TedarikciUygunsuzlukDetayService.GetSupplierUnsuitabilityDetailedCustomerAnalysis(errorID, startDate, endDate);
            dataunsuitabilityproduct = await TedarikciUygunsuzlukDetayService.GetSupplierUnsuitabilityDetailedProductAnalysis(errorID, startDate, endDate);
            datasuppuns = await TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);
            unsuitabilityName = datasuppuns.Where(t => t.ErrorID == errorID).Select(t => t.UnsuitabilityReason).FirstOrDefault();

            #region Sütun Genişlikleri

            if (dataunsuitabilitycustomer.Count() <= 3)
            {
                columnwidth1 = 0.1;
            }
            if (dataunsuitabilityproduct.Count() <= 3)
            {
                columnwidth2 = 0.1;
            }

            #endregion

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

            NavigationManager.NavigateTo("/admin/supplier-unsuitability-analysis");
        }

    }
}
