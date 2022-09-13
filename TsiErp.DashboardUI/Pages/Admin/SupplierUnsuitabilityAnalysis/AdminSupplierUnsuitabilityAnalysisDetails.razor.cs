using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.SupplierUnsuitabilityAnalysis
{
    public partial class AdminSupplierUnsuitabilityAnalysisDetails
    {
        List<SupplierUnsuitabilityDetailedCustomer> dataunsuitabilitycustomer = new List<SupplierUnsuitabilityDetailedCustomer>();
        List<SupplierUnsuitabilityDetailedProduct> dataunsuitabilityproduct = new List<SupplierUnsuitabilityDetailedProduct>();

        SfGrid<SupplierUnsuitabilityDetailedCustomer> CustomerGrid;
        SfGrid<SupplierUnsuitabilityDetailedProduct> ProductGrid;

        #region Değişkenler

        bool VisibleSpinner = false;
        string unsuitabilityName = string.Empty;
        double columnwidth1;
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


        protected override void OnInitialized()
        {
            dataunsuitabilitycustomer = TedarikciUygunsuzlukDetayService.GetSupplierUnsuitabilityDetailedCustomerAnalysis(errorID, startDate, endDate);
            dataunsuitabilityproduct = TedarikciUygunsuzlukDetayService.GetSupplierUnsuitabilityDetailedProductAnalysis(errorID, startDate, endDate);
            unsuitabilityName = TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate).Where(t => t.ErrorID == errorID).Select(t => t.UnsuitabilityReason).FirstOrDefault();

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

        private void OnBackButtonClicked()
        {
            this.VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/supplier-unsuitability-analysis");
        }

    }
}
