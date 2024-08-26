using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.ErpUI.Models.Dashboard;

namespace TsiErp.ErpUI.Pages.Dashboard
{
    public partial class ProductGroupScrapAnalysisPage
    {
        SfChart ChartInstance;

        SfGrid<ProductGroupsAnalysis> Grid;

        bool VisibleSpinner = false;

        DateTime StartDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime EndDate = DateTime.Today;
        private int FrequencyChart;

        List<ProductGroupsAnalysis> DataProductGroup = new List<ProductGroupsAnalysis>();
        List<AdminProductChart> DataChart = new List<AdminProductChart>();
        List<ProductGroupsAnalysis> DataProductGroupCombobox = new List<ProductGroupsAnalysis>();
        string ChartTitle = string.Empty;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };


        protected override async Task OnInitializedAsync()
        {
            DataProductGroupCombobox = await DashboardAppServices.GetProductGroupsComboboxAnalysis(StartDate, EndDate);
        }


        #region Detay Tablosu İşlemleri
        private bool DetailGridVisibility = true;

        private async void DetailGridVisibilityChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            DetailGridVisibility = Convert.ToBoolean(args.Value);

            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Chart Label İşlemleri
        private bool ChartLabelVisibility = true;


        private void ChartLabelVisibilityChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            ChartInstance.RefreshAsync();

            ChartLabelVisibility = Convert.ToBoolean(args.Value);
        }
        #endregion

        #region Zaman Periyodu İşlemleri
        private int? SelectedTimeIndex { get; set; }

        string ChartAverageLabel = "Yıllık Ortalama Değer :";

        private List<ComboboxTimePeriods> ComboboxTimePeriods = new List<ComboboxTimePeriods>() {
        new ComboboxTimePeriods(){ TimeID= 1, TimeText= "Yıllık" },
        new ComboboxTimePeriods(){ TimeID= 2, TimeText= "Son 9 Ay" },
        new ComboboxTimePeriods(){ TimeID= 3, TimeText= "Son 6 Ay" },
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "Son 3 Ay" },
        new ComboboxTimePeriods(){ TimeID= 5, TimeText= "Son 2 Ay" },
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "Son 1 Ay" },
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "Son 1 Hafta" }
     };
        #endregion

        #region Ürün Grubu İşlemleri
        private int? SelectedProductIndex { get; set; }

        private Guid? SelectedProductID { get; set; }


        private async void ProductGroupOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<Guid?, ProductGroupsAnalysis> args)
        {
            SelectedProductID = args.Value;

            await InvokeAsync(StateHasChanged);
        }
        #endregion

        private async void FilterClicked()
        {
            VisibleSpinner = true;
            await Task.Delay(1);
            await InvokeAsync(StateHasChanged);


            #region Zaman Seçimi
            switch (SelectedTimeIndex)
            {
                case 0:
                    StartDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); FrequencyChart = 0; ChartAverageLabel = "Yıllık Ortalama Değer: ";
                    break;
                case 1:
                    StartDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); FrequencyChart = 1; ChartAverageLabel = "9 Aylık Ortalama Değer: ";
                    break;
                case 2:
                    StartDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); FrequencyChart = 2; ChartAverageLabel = "6 Aylık Ortalama Değer: ";
                    break;
                case 3:
                    StartDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); FrequencyChart = 3; ChartAverageLabel = "3 Aylık Ortalama Değer: ";
                    break;
                case 4:
                    StartDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); FrequencyChart = 4; ChartAverageLabel = "2 Aylık Ortalama Değer: ";
                    break;
                case 5:
                    StartDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); FrequencyChart = 5; ChartAverageLabel = "1 Aylık Ortalama Değer: ";
                    break;
                case 6:
                    StartDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); FrequencyChart = 6; ChartAverageLabel = "1 Haftalık Ortalama Değer: ";
                    break;
                default:
                    break;
            }

            #endregion


            DataProductGroup = await DashboardAppServices.GetProductGroupsAnalysis(StartDate, EndDate);

            DataProductGroupCombobox = await DashboardAppServices.GetProductGroupsComboboxAnalysis(StartDate, EndDate);

            DataChart = await DashboardAppServices.GetProductChart(StartDate, EndDate, FrequencyChart, SelectedProductID);

            ChartTitle = DataProductGroup.Where(t => t.ProductGroupID == SelectedProductID).Select(t => t.ProductGroupName).FirstOrDefault() + " HURDA GRAFİĞİ";

            VisibleSpinner = false;

            await InvokeAsync(StateHasChanged);

            await Grid.Refresh();

            await ChartInstance.RefreshAsync();
        }

        private void OnDetailButtonClicked(Guid stationID)
        {
            VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/product-group-analysis/details" + "/" + stationID.ToString() + "/" + StartDate.ToString("yyyy, MM, dd") + "/" + EndDate.ToString("yyyy, MM, dd")); ;
        }
    }
}
