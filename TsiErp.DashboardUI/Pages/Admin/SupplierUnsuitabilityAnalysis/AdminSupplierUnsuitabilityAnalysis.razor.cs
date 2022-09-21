using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.SupplierUnsuitabilityAnalysis
{
    public partial class AdminSupplierUnsuitabilityAnalysis
    {
        List<Models.SupplierUnsuitabilityAnalysis> datasuppunsuitability = new List<Models.SupplierUnsuitabilityAnalysis>();
        SfGrid<Models.SupplierUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-90);
        DateTime endDate = DateTime.Today;
        private int? selectedTimeIndex { get; set; }
        int? selectedactionID = 1;
        private bool isGridChecked = true;
        string chartTitle = "Tedarikçi ile İrtibat Sayısı Grafiği";
        SfChart ChartInstance;
        bool VisibleSpinner = false;

        #endregion

        protected override async void OnInitialized()
        {
            

            datasuppunsuitability = await TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);

        }


        #region Component Metotları

        private async void OnDateButtonClicked()
        {

            VisibleSpinner = true;
            await Task.Delay(1);
            StateHasChanged();

            endDate = DateTime.Today;

            #region Zaman Seçimi

            switch (selectedTimeIndex)
            {
                case 0: startDate = DateTime.Today.AddDays(-365); ; break;
                case 1: startDate = DateTime.Today.AddDays(-273); ; break;
                case 2: startDate = DateTime.Today.AddDays(-181); ; break;
                case 3: startDate = DateTime.Today.AddDays(-90); ; break;
                case 4: startDate = DateTime.Today.AddDays(-60); ; break;
                case 5: startDate = DateTime.Today.AddDays(-30); ; break;
                case 6: startDate = DateTime.Today.AddDays(-7); ; break;
                default: break;
            }

            #endregion

            #region Aksiyon Seçimi

            switch (selectedactionID)
            {
                case 1: chartTitle = "Tedarikçi ile İrtibat Sayısı Grafiği"; break;
                case 2: chartTitle = "Düzeltme Grafiği"; break;
                case 3: chartTitle = "Olduğu Gibi Kullanılacak Grafiği"; break;
                case 4: chartTitle = "Red Grafiği"; break;
                case 5: chartTitle = "Toplu Uygunsuzluk Grafiği"; break;
            }

            #endregion

            datasuppunsuitability = await TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);


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

        //private void OnDetailButtonClicked(int errorID, int totalOrder)
        //{
        //    VisibleSpinner = true;

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
