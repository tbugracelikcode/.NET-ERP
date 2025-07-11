﻿using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.EmployeeAnalysis
{
    public partial class AdminEmployeeAnalysis
    {
        List<EmployeeGeneralAnalysis> dataemployee = new List<EmployeeGeneralAnalysis>();
        List<AdminEmployeeChart> datachart = new List<AdminEmployeeChart>();
        SfGrid<EmployeeGeneralAnalysis> Grid;

        #region Değişkenler

        DateTime startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day));
        DateTime endDate = DateTime.Today.AddDays(-(DateTime.Today.Day));
        private int? selectedTimeIndex { get; set; }
        private int threshold = 75;
        private double thresholddouble = 0.75;
        private int frequencyChart;
        SfChart ChartInstance;
        private bool isGridChecked = true;
        bool VisibleSpinner = false;
        private bool isLabelsChecked = true;
        private bool dataLabels = true;
        private bool compareModalVisible = false;
        public string[]? MultiSelectVal = new string[] { };
        string chartAverageLabel = "Yıllık Ortalama Değer :";
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion

        protected override async void OnInitialized()
        {
            //var a = PersonelDetayService.GetStationDetailedHaltAnalysis(1,startDate,endDate);
            

            dataemployee = await PersonelService.GetEmployeeGeneralAnalysis(startDate, endDate);
            datachart = await PersonelService.GetEmployeeChart(startDate, endDate, 0);

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
                case 0: startDate = DateTime.Today.AddDays(-(364 + DateTime.Today.Day)); frequencyChart = 0; chartAverageLabel = "Yıllık Ortalama Değer: "; break;
                case 1: startDate = DateTime.Today.AddDays(-(272 + DateTime.Today.Day)); frequencyChart = 1; chartAverageLabel = "9 Aylık Ortalama Değer: "; break;
                case 2: startDate = DateTime.Today.AddDays(-(180 + DateTime.Today.Day)); frequencyChart = 2; chartAverageLabel = "6 Aylık Ortalama Değer: "; break;
                case 3: startDate = DateTime.Today.AddDays(-(89 + DateTime.Today.Day)); frequencyChart = 3; chartAverageLabel = "3 Aylık Ortalama Değer: "; break;
                case 4: startDate = DateTime.Today.AddDays(-(59 + DateTime.Today.Day)); frequencyChart = 4; chartAverageLabel = "2 Aylık Ortalama Değer: "; break;
                case 5: startDate = DateTime.Today.AddDays(-(29 + DateTime.Today.Day)); frequencyChart = 5; chartAverageLabel = "1 Aylık Ortalama Değer: "; break;
                case 6: startDate = DateTime.Today.AddDays(-(6 + DateTime.Today.Day)); frequencyChart = 6; chartAverageLabel = "1 Haftalık Ortalama Değer: "; break;
                default: break;
            }

            #endregion

            thresholddouble = Convert.ToDouble(threshold) / 100;
            dataemployee = await PersonelService.GetEmployeeGeneralAnalysis(startDate, endDate);
            datachart = await PersonelService.GetEmployeeChart(startDate, endDate, frequencyChart);
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
        private void OnDetailButtonClicked(int employeeID)
        {
            VisibleSpinner = true;

            NavigationManager.NavigateTo("/admin/employee-analysis/details" + "/" + employeeID.ToString() + "/" + startDate.ToString("yyyy, MM, dd") + "/" + endDate.ToString("yyyy, MM, dd"));

        }

        private void OnChangeLabelCheck(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            ChartInstance.RefreshAsync();
            if (isLabelsChecked) { dataLabels = true; }
            else { dataLabels = false; }
        }

        private async void OnCompareButtonClicked()
        {
            ShowCompareModal();
        }

        private async void ShowCompareModal()
        {
            compareModalVisible = true;
        }

        private async void HideCompareModal()
        {
            compareModalVisible = false;
            MultiSelectVal = null;
        }

        #endregion

        public void CellInfoHandler(QueryCellInfoEventArgs<EmployeeGeneralAnalysis> Args)
        {
            if (Args.Column.Field == "OEE")
            {
                if (Args.Data.OEE < Convert.ToDecimal(thresholddouble))
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #DF0000; color: white; " });
                }
                else
                {
                    Args.Cell.AddStyle(new string[] { "background-color: #37CB00; color: white;" });
                }
            }
            StateHasChanged();
        }

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

        #endregion
    }
}
