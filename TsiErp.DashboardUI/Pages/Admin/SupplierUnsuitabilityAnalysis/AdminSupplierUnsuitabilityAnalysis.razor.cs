using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.SupplierUnsuitabilityAnalysis
{
    public partial class AdminSupplierUnsuitabilityAnalysis
    {
        List<TsiErp.DashboardUI.Models.SupplierUnsuitabilityAnalysis> datasuppunsuitability = new List<TsiErp.DashboardUI.Models.SupplierUnsuitabilityAnalysis>();
        List<TsiErp.DashboardUI.Models.AdminSupplierUnsuitabilityAnalysisChart> datachart = new List<TsiErp.DashboardUI.Models.AdminSupplierUnsuitabilityAnalysisChart>();
        SfGrid<TsiErp.DashboardUI.Models.SupplierUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        private int? selectedTimeIndex { get; set; }
        int? selectedactionID = 1;
        string chartTitle = "Tedarikçi ile İrtibat Sayısı Grafiği";
        SfChart ChartInstance;
        bool VisibleSpinner = false;

        #endregion

        protected override void OnInitialized()
        {
            datasuppunsuitability = TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);
        }


        #region Component Metotları

        private void OnDateButtonClicked()
        {
            endDate = DateTime.Today;

            #region Zaman Seçimi

            if (selectedTimeIndex == 0)
            {
                startDate = DateTime.Today.AddDays(-365);
            }
            else if (selectedTimeIndex == 1)
            {
                startDate = DateTime.Today.AddDays(-273);
            }
            else if (selectedTimeIndex == 2)
            {
                startDate = DateTime.Today.AddDays(-181);
            }
            else if (selectedTimeIndex == 3)
            {
                startDate = DateTime.Today.AddDays(-90);
            }
            else if (selectedTimeIndex == 4)
            {
                startDate = DateTime.Today.AddDays(-60);
            }
            else if (selectedTimeIndex == 5)
            {
                startDate = DateTime.Today.AddDays(-30);
            }
            else if (selectedTimeIndex == 6)
            {
                startDate = DateTime.Today.AddDays(-7);
            }

            #endregion

            #region Aksiyon Seçimi
            if (selectedactionID == 1)
            {
                chartTitle = "Tedarikçi ile İrtibat Sayısı Grafiği";
            }
            else if (selectedactionID == 2)
            {
                chartTitle = "Düzeltme Grafiği";
            }
            else if (selectedactionID == 3)
            {
                chartTitle = "Olduğu Gibi Kullanılacak Grafiği";
            }
            else if (selectedactionID == 4)
            {
                chartTitle = "Red Grafiği";
            }
            else if (selectedactionID == 5)
            {
                chartTitle = "Toplu Uygunsuzluk Grafiği";
            }
            #endregion

            Grid.Refresh();
            ChartInstance.RefreshAsync();
            datasuppunsuitability = TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);
            StateHasChanged();
        }

        //private void OnDetailButtonClicked(int errorID, int totalOrder)
        //{
        //    NavigationManager.NavigateTo("/admin/supplier-unsuitability-analysis/details" + "/" + errorID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd") + "/" + totalOrder.ToString());
        //}

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

        private List<ComboboxUnsuitability> unsuitabilities = new List<ComboboxUnsuitability>() {
        new ComboboxUnsuitability(){ TypeID= 1, TypeText= "Tedarikçi ile İrtibat" },
        new ComboboxUnsuitability(){ TypeID= 2, TypeText= "Düzeltme" },
        new ComboboxUnsuitability(){ TypeID= 3, TypeText= "Olduğu Gibi Kullanılacak" },
        new ComboboxUnsuitability(){ TypeID= 4, TypeText= "Red" },
        new ComboboxUnsuitability(){ TypeID= 5, TypeText= "Hepsini Göster" }
     };

        #endregion

    }
}
