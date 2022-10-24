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
        List<Models.ProductionUnsuitabilityAnalysis> dataprouns = new List<Models.ProductionUnsuitabilityAnalysis>();

        SfGrid<ProductionUnsuitabilityDetailedEmployee> EmployeeGrid;
        SfGrid<ProductionUnsuitabilityDetailedStation> StationGrid;
        SfGrid<ProductionUnsuitabilityDetailedProduct> ProductGrid;

        #region Değişkenler

        bool visibleMachineTab = true;
        bool visibleEmployeeTab = true;
        bool VisibleSpinner = false;
        string unsuitabilityName = string.Empty;
        double columnwidth1;
        double columnwidth2;
        private bool isGridChecked = true;
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


        protected override async void OnInitialized()
        {

            dataunsuitabilitystation = await UretimUygunsuzlukDetayService.GetProductionUnsuitabilityDetailedStationAnalysis(unsuitabilityCode, startDate, endDate, selectedActionID);
            dataunsuitabilityemployee = await UretimUygunsuzlukDetayService.GetProductionUnsuitabilityDetailedEmployeeAnalysis(unsuitabilityCode, startDate, endDate, selectedActionID);
            dataunsuitabilityproduct = await UretimUygunsuzlukDetayService.GetProductionUnsuitabilityDetailedProductAnalysis(unsuitabilityCode, startDate, endDate, selectedActionID);
            dataprouns = await UretimUygunsuzlukService.GetProductionUnsuitabilityAnalysis(startDate, endDate);

            unsuitabilityName = dataprouns.Where(t => t.Code == unsuitabilityCode).Select(t => t.UnsuitabilityReason).FirstOrDefault();

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
                actionName = "Genel Uygunsuzluk";
            }
            #endregion

            #region PKD Tab Visible
            if (dataunsuitabilitystation.Count() == 0)
            {
                visibleMachineTab = false;
            }
            else
            {
                visibleMachineTab = true;
            }

            if(dataunsuitabilityemployee.Count() == 0)
            {
                visibleEmployeeTab = false;
            }
            else
            {
                visibleEmployeeTab = true;
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

            NavigationManager.NavigateTo("/admin/production-unsuitability-analysis");
        }


    }
}
