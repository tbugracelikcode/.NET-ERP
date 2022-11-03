using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Pages.Admin.SupplierUnsuitabilityAnalysis
{
    public partial class AdminSupplierUnsuitabilityAnalysis
    {
        List<Models.SupplierUnsuitabilityAnalysis> datasuppunsuitability = new List<Models.SupplierUnsuitabilityAnalysis>();
        List<Models.SupplierUnsuitabilityAnalysis> datasuppunsuitabilityChart = new List<Models.SupplierUnsuitabilityAnalysis>();
        SfGrid<Models.SupplierUnsuitabilityAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        int? selectedactionID = 1;
        private bool isGridChecked = true;
        string chartTitle = "Tedarikçi ile İrtibat Sayısı Grafiği";
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private int threshold = 50;
        private double thresholddouble = 0.50;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion

        protected override async void OnInitialized()
        {


            datasuppunsuitability = await TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);
            var chartList = datasuppunsuitability.Where(t => t.Percent > 0).ToList();
            foreach (var item in chartList)
            {
                if (item.SupplierName.Length > 15)
                {
                    item.SupplierShortName = item.SupplierName.Substring(0, 15) + "...";
                }
                else
                {
                    item.SupplierShortName = item.SupplierName;
                }
            }
            datasuppunsuitabilityChart = chartList;
        }

        public void CellInfoHandler(QueryCellInfoEventArgs<Models.SupplierUnsuitabilityAnalysis> Args)
        {
            if (Args.Column.Field == "Percent")
            {
                if (Args.Data.Percent < thresholddouble)
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #37CB00; color: white; " });
                }
                else
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #DF0000; color: white;" });
                }
            }
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
                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); ; break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); ; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); break;
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

            thresholddouble = Convert.ToDouble(threshold) / 100;

            datasuppunsuitability = await TedarikciUygunsuzlukService.GetSupplierUnsuitabilityAnalysis(startDate, endDate);
            var chartList = datasuppunsuitability.Where(t => t.Percent > 0).ToList();
            foreach (var item in chartList)
            {
                if (item.SupplierName.Length > 15)
                {
                    item.SupplierShortName = item.SupplierName.Substring(0, 15) + "...";
                }
                else
                {
                    item.SupplierShortName = item.SupplierName;
                }
            }
            datasuppunsuitabilityChart = chartList;

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
