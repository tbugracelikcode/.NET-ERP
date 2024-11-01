using Microsoft.AspNetCore.Components;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.ErpUI.Components.Commons.Spinner;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using System.Data;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using Syncfusion.XlsIO;
using TsiErp.Business.Entities.Dashboard.AdminDashboard;
using TsiErp.Business.Entities.StationGroup.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using DevExpress.XtraCharts.Native;
using TsiErp.Business.Entities.SalesOrder.Services;

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminProductGroupAnalysisPage
    {
        List<AdminProductGroupAnalysisChart> ProductGroupList = new List<AdminProductGroupAnalysisChart>();

        List<AdminProductGroupAnalysisBarChart> ProductGroupBarList = new List<AdminProductGroupAnalysisBarChart>();


        public List<ListProductGroupsDto> ProductGrp = new List<ListProductGroupsDto>();
        public List<ProductGroupItem> ProductGrpNameList = new List<ProductGroupItem>();

        public Guid ComboBoxValue = Guid.Empty;

        [Inject]
        SpinnerService Spinner { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }


        #region Değişkenler

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        Guid productGroupID = Guid.Empty;
        private int? selectedTimeIndex { get; set; } = 0;

        public decimal PlannedQuantity = 0;
        public decimal ProducedQuantity = 0;
        public decimal FaultyQuantity = 0;
        private int? selectedProductIndex { get; set; }
        private string chartTitle;
        private string barchartTitle;
        int? selectedproductID;
        SfChart ChartInstance;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        string chartAverageLabel = string.Empty;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };


        public List<ChartData> UnsuitabilityItem = new List<ChartData>();

        #endregion


        public class ChartData
        {
            public string UnsuitabilityItems { get; set; }
            public double Quantity { get; set; }
        }

        public class ProductGroupItem
        {
            public string ProductGroupName { get; set; }

            public Guid ProductGroupID { get; set; }
        }

        protected override async void OnInitialized()
        {
            Spinner.Show();
            await Task.Delay(100);

            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            startDate = today.AddDays(-(364 + today.Day));
            endDate = today.AddDays(-(today.Day));
            chartAverageLabel = L["ChartAverageLabelAnnual"];

            ProductGrp = (await ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Data.Where(t=>t.isDashBoardData==true).ToList();

            productGroupID = ProductGrp.Where(t => t.Code == "ROT BAŞI").Select(t => t.Id).FirstOrDefault();

            chartTitle = ProductGrp.Where(t => t.Id == productGroupID).Select(t => t.Name).FirstOrDefault() + " " + L["ScrapTitle"];

            barchartTitle = ProductGrp.Where(t => t.Id == productGroupID).Select(t => t.Name).FirstOrDefault() + " " + L["PPMTitle"];

            ProductGroupList = (await AdminDashboardAppService.GetAdminProductGroupChart(startDate, endDate, productGroupID));


            ProductGroupBarList = (await AdminDashboardAppService.GetAdminProductGroupBarChart(startDate, endDate, productGroupID));


            foreach (var group in ProductGrp)
            {

                ProductGrpNameList.Add(new ProductGroupItem
                {
                    ProductGroupName = group.Name,
                    ProductGroupID = group.Id
                });
            }

            if (ProductGroupList != null && ProductGroupList.Count > 0)
            {
                foreach (var oee in ProductGroupList)
                {
                    oee.MONTH = L[oee.MONTH] + " " + oee.YEAR.ToString();
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }

            isGridChecked = false;

            foreach (var item in timeperiods)
            {
                item.TimeText = L[item.TimeText];
            }

            #region Quantities

            PlannedQuantity = ProductGroupList.Select(t => t.PLANNEDQUANTITY).FirstOrDefault();
            ProducedQuantity = ProductGroupList.Sum(t => t.PRODUCEDQUANTITY);
            FaultyQuantity = ProductGroupList.Sum(t => t.SCRAPQUANTITY);


            #endregion
            Spinner.Hide();
            await InvokeAsync(StateHasChanged);
        }

        #region Component Metotları
        private async void OnDateButtonClicked()
        {
            Spinner.Show();

            await Task.Delay(100);

            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            #region Zaman Seçimi
            switch (selectedTimeIndex)
            {

                case 0: startDate = today.AddDays(-(364 + today.Day)); chartAverageLabel = L["ChartAverageLabelAnnual"]; break;
                case 1: startDate = today.AddDays(-(272 + today.Day)); chartAverageLabel = L["ChartAverageLabel9Months"]; break;
                case 2: startDate = today.AddDays(-(180 + today.Day)); chartAverageLabel = L["ChartAverageLabel6Months"]; break;
                case 3: startDate = today.AddDays(-(89 + today.Day)); chartAverageLabel = L["ChartAverageLabel3Months"]; break;
                default: break;
            }

            #endregion


            ProductGroupList = (await AdminDashboardAppService.GetAdminProductGroupChart(startDate, endDate, ComboBoxValue));

            ProductGroupBarList = (await AdminDashboardAppService.GetAdminProductGroupBarChart(startDate, endDate, ComboBoxValue));

            if (ProductGroupList != null && ProductGroupList.Count > 0)
            {
                foreach (var oee in ProductGroupList)
                {
                    oee.MONTH = L[oee.MONTH] + " " + oee.YEAR.ToString();
                }
            }
            else
            {
                Spinner.Hide();
                await ModalManager.MessagePopupAsync(L["UIMessageEmptyListTitle"], L["UIMessageEmptyListMessage"]);
            }



            chartTitle = ProductGrp.Where(t => t.Id == ComboBoxValue).Select(t => t.Name).FirstOrDefault() + " " + L["ScrapTitle"];

            barchartTitle = ProductGrp.Where(t => t.Id == ComboBoxValue).Select(t => t.Name).FirstOrDefault() + " " + L["PPMTitle"];


            await ChartInstance.RefreshAsync();
            Spinner.Hide();
            await InvokeAsync(StateHasChanged);

        }


        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }

        #endregion

        #region Combobox

        public class ComboboxTimePeriods
        {
            public int TimeID { get; set; }
            public string TimeText { get; set; }
        }

        private List<ComboboxTimePeriods> timeperiods = new List<ComboboxTimePeriods>() {
        new ComboboxTimePeriods(){ TimeID= 1, TimeText= "ComboboxAnnual" },
        new ComboboxTimePeriods(){ TimeID= 2, TimeText= "ComboboxLast9Months" },
        new ComboboxTimePeriods(){ TimeID= 3, TimeText= "ComboboxLast6Months" },
        new ComboboxTimePeriods(){ TimeID= 4, TimeText= "ComboboxLast3Months" }
     };

        #endregion
    }
}
