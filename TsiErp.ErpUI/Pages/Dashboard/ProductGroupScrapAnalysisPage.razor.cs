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
            ChartAverageLabel = L["ChartAverageLabelAnnual"];

            foreach(var item in ComboboxTimePeriods)
            {
                item.TimeText = L[item.TimeText];
            }
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

        string ChartAverageLabel = string.Empty;

        private List<ComboboxTimePeriods> ComboboxTimePeriods = new List<ComboboxTimePeriods>() {
        new ComboboxTimePeriods(){ TimeID= 1, TimeText= "ComboboxAnnual" },
        new ComboboxTimePeriods(){ TimeID= 2, TimeText= "ComboboxLast9Months" },
        new ComboboxTimePeriods(){ TimeID= 3, TimeText= "ComboboxLast6Months" },
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "ComboboxLast3Months" },
        new ComboboxTimePeriods(){ TimeID= 5, TimeText= "ComboboxLast2Months" },
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "ComboboxLast1Month" },
        new ComboboxTimePeriods(){ TimeID= 6, TimeText= "ComboboxLast1Week" }
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
                    StartDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); FrequencyChart = 0; ChartAverageLabel = L["ChartAverageLabelAnnual"];
                    break;
                case 1:
                    StartDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); FrequencyChart = 1; ChartAverageLabel = L["ChartAverageLabel9Months"];
                    break;
                case 2:
                    StartDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); FrequencyChart = 2; ChartAverageLabel = L["ChartAverageLabel6Months"];
                    break;
                case 3:
                    StartDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); FrequencyChart = 3; ChartAverageLabel = L["ChartAverageLabel3Months"];
                    break;
                case 4:
                    StartDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); FrequencyChart = 4; ChartAverageLabel = L["ChartAverageLabel2Months"];
                    break;
                case 5:
                    StartDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); FrequencyChart = 5; ChartAverageLabel = L["ChartAverageLabel1Month"];
                    break;
                case 6:
                    StartDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); FrequencyChart = 6; ChartAverageLabel = L["ChartAverageLabel1Week"];
                    break;
                default:
                    break;
            }

            #endregion


            DataProductGroup = await DashboardAppServices.GetProductGroupsAnalysis(StartDate, EndDate);

            DataProductGroupCombobox = await DashboardAppServices.GetProductGroupsComboboxAnalysis(StartDate, EndDate);

            DataChart = await DashboardAppServices.GetProductChart(StartDate, EndDate, FrequencyChart, SelectedProductID);

            ChartTitle = DataProductGroup.Where(t => t.ProductGroupID == SelectedProductID).Select(t => t.ProductGroupName).FirstOrDefault() + " " + L["ScrapChartTitleAddition"];

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
