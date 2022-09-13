using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.ProductionUnsuitabilityAnalysis
{
    public partial class AdminProductionUnsuitabilityAnalysisDetails
    {
        List<ProductionUnsuitabilityDetailedStation> dataunsuitabilitystation = new List<ProductionUnsuitabilityDetailedStation>();
        List<ProductionUnsuitabilityDetailedEmployee> dataunsuitabilityemployee = new List<ProductionUnsuitabilityDetailedEmployee>();
        List<ProductionUnsuitabilityDetailedProduct> dataunsuitabilityproduct = new List<ProductionUnsuitabilityDetailedProduct>();
        AdminProductionUnsuitabilityAnalysis _labelreason = new AdminProductionUnsuitabilityAnalysis();

        SfGrid<ProductionUnsuitabilityDetailedEmployee> EmployeeGrid;
        SfGrid<ProductionUnsuitabilityDetailedStation> StationGrid;
        SfGrid<ProductionUnsuitabilityDetailedProduct> ProductGrid;

        #region Değişkenler

        bool VisibleSpinner = false;
        string unsuitabilityName = string.Empty;
        double columnwidth1;
        double columnwidth2;
        double columnwidth3;
        string actionName = string.Empty;
        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public string unsuitabilityCode { get; set; }

        [Parameter]
        public int selectedActionID { get; set; }

        #endregion




        protected override void OnInitialized()
        {
            dataunsuitabilitystation = UretimUygunsuzlukDetayService.GetProductionUnsuitabilityDetailedStationAnalysis(unsuitabilityCode, startDate, endDate, selectedActionID);
            dataunsuitabilityemployee = UretimUygunsuzlukDetayService.GetProductionUnsuitabilityDetailedEmployeeAnalysis(unsuitabilityCode, startDate, endDate, selectedActionID);
            dataunsuitabilityproduct = UretimUygunsuzlukDetayService.GetProductionUnsuitabilityDetailedProductAnalysis(unsuitabilityCode, startDate, endDate, selectedActionID);
            unsuitabilityName = UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate).Where(t => t.Code == unsuitabilityCode).Select(t => t.UnsuitabilityReason).FirstOrDefault();

            #region Sütun Genişlikleri
            if (dataunsuitabilitystation.Count() <= 3)
            {
                columnwidth1 = 0.1;
            }
            if (dataunsuitabilityemployee.Count() <= 3)
            {
                columnwidth2 = 0.1;
            }
            if (dataunsuitabilityproduct.Count() <= 3)
            {
                columnwidth3 = 0.1;
            }
            #endregion

            #region Aksiyon Başlık
            if (selectedActionID == 1)
            {
                actionName = "Hurda";
            }
            else if (selectedActionID == 2)
            {
                actionName = "Düzeltme";
            }
            else if (selectedActionID == 3)
            {
                actionName = "Olduğu Gibi Kullanılacak";
            }
            else if (selectedActionID == 4)
            {
                actionName = "Toplu Uygunsuzluk";
            }
            #endregion
        }



        private void OnBackButtonClicked()
        {
            this.VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/production-unsuitability-analysis");
        }


    }
}
