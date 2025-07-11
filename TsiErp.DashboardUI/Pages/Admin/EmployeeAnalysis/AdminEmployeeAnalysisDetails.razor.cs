﻿using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services;

namespace TsiErp.DashboardUI.Pages.Admin.EmployeeAnalysis
{
    public partial class AdminEmployeeAnalysisDetails
    {
        List<EmployeeDetailedHaltAnalysis> dataemployeehalt = new List<EmployeeDetailedHaltAnalysis>();
        List<EmployeeDetailedChart> dataemployeechart = new List<EmployeeDetailedChart>();

        SfGrid<EmployeeDetailedHaltAnalysis> EmployeeGrid;
        SfChart ChartInstance;

        #region Değişkenler

        bool VisibleSpinner = false;
        double columnwidth;
        private bool isGridChecked = true;
        private bool isEmployeeChecked = false;
        string employeeName = string.Empty;

        [Parameter]
        public DateTime startDate { get; set; }

        [Parameter]
        public DateTime endDate { get; set; }

        [Parameter]
        public int employeeID { get; set; }
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion


        protected override async void OnInitialized()
        {

            dataemployeehalt = await PersonelDetayService.GetStationDetailedHaltAnalysis(employeeID, startDate, endDate);
            dataemployeechart = await PersonelDetayService.GetEmployeeDetailedtChart(employeeID, startDate, endDate);
            employeeName = dataemployeehalt.Select(t => t.EmployeeName).FirstOrDefault();

            #region Sütun Genişlikleri

            if (dataemployeechart.Count() <= 3)
            {
                columnwidth = 0.1;
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

            NavigationManager.NavigateTo("/admin/employee-analysis");
        }

    }
}
