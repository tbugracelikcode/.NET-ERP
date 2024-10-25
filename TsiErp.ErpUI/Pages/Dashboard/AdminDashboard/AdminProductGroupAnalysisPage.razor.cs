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

namespace TsiErp.ErpUI.Pages.Dashboard.AdminDashboard
{
    public partial class AdminProductGroupAnalysisPage
    {
        List<AdminProductGroupAnalysisChart> ProductGroupList = new List<AdminProductGroupAnalysisChart>();

        List<AdminProductGroupAnalysisGrid> ProductGroupGridList = new List<AdminProductGroupAnalysisGrid>();


        public List<ListProductionTrackingsDto> ProductGrp = new List<ListProductionTrackingsDto>();
        public List<string> ProductGrpNameList = new List<string>();

        List<AdminProductGroupAnalysisGrid> dataproductgroup = new List<AdminProductGroupAnalysisGrid>();
        List<AdminProductGroupAnalysisGrid> dataproductgroupcombobox = new List<AdminProductGroupAnalysisGrid>();

        [Inject]
        SpinnerService Spinner { get; set; }

        [Inject]
        ModalManager ModalManager { get; set; }


        #region Değişkenler

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        private int? selectedTimeIndex { get; set; } = 0;
        private int? selectedProductIndex { get; set; }
        int? selectedproductID;
        private int threshold = 75;
        private double thresholddouble = 0.75;
        SfChart ChartInstance;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool isGridChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        string chartAverageLabel = string.Empty;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion


        protected override async void OnInitialized()
        {
            var today = GetSQLDateAppService.GetDateFromSQL().Date;

            startDate = today.AddDays(-(364 + today.Day));
            endDate = today.AddDays(-(today.Day));

            chartAverageLabel = L["ChartAverageLabelAnnual"];

            ProductGroupList = (await AdminDashboardAppService.GetAdminProductGroupChart(startDate, endDate));

            ProductGrp = (await ProductionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.ToList();
            ProductGroupGridList = (await AdminDashboardAppService.GetAdminProductGroupGrid(startDate, endDate));

            var productGroupList = ProductGroupGridList.Select(t => t.PRODUCTGROUPID).Distinct().ToList();
            foreach (var groupId in productGroupList)
            {
                string groupname = ProductGrp.Where(t => t.Id == groupId).Select(t => t.ProductGroupName).FirstOrDefault();
                ProductGrpNameList.Add(groupname);
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


            thresholddouble = Convert.ToDouble(threshold) / 100;


            await InvokeAsync(StateHasChanged);

        }

        private async void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            await ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = false; }
            else { dataLabels = true; }
        }

        private void OnCheckedChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);
            isGridChecked = argsValue;

            StateHasChanged();
        }

        private void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int?, AdminProductGroupAnalysisGrid> args)
        {
            selectedproductID = args.Value;

            StateHasChanged();
        }
        private async void OnCompareButtonClicked()
        {
            ShowCompareModal();
        }

        private async void ShowCompareModal()
        {
            compareModalVisible = true;
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
