﻿using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.PivotView;
using System.Dynamic;
using TsiErp.ErpUI.Models.Dashboard.OperationalDashboard.OpenOrderAnalysis;
using TsiErp.ErpUI.Services.Dashboard.OperationalDashboard.OpenOrderAnalysis;

namespace TsiErp.ErpUI.Pages.Dashboard.OperationalDashboard
{
    public partial class OpenOrderAnalysisPage
    {
        SfGrid<CurrentBalanceAndQuantityTableDto> Grid;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        List<CurrentBalanceAndQuantityTableDto> GridList = new List<CurrentBalanceAndQuantityTableDto>();

        List<GrandTotalBalanceChart> GrandTotalBalanceChartList = new List<GrandTotalBalanceChart>();

        public class GrandTotalBalanceChart
        {
            public string ProductGroupName { get; set; }
            public int Count { get; set; }
            public string ValuePercent { get; set; }
        }


        protected override async void OnInitialized()
        {
            GridList = (await OpenOrderAnalysisAppService.GetCurrentBalanceAndQuantityListAsync()).ToList();


            var distintedGridList = GridList.Select(t => t.ProductGroupName).Distinct().ToList();

            foreach (var productgrp in distintedGridList)
            {
                decimal count = GridList.Where(t => t.ProductGroupName == productgrp).Sum(t => t.PlannedQuantitySum);

                decimal total = GridList.Sum(t => t.PlannedQuantitySum);

                string percent = ((count / total) * 100).ToString("N1") + "%";

                GrandTotalBalanceChart grandTotalBalanceChartModel = new GrandTotalBalanceChart
                {
                    Count = (int)count,
                    ProductGroupName = productgrp,
                    ValuePercent = percent
                };

                GrandTotalBalanceChartList.Add(grandTotalBalanceChartModel);
            }

            await (InvokeAsync(StateHasChanged));
        }

        private async void CellClick(CellClickEventArgs args)
        {
            if (args.Data != null)
            {
                if (args.Data.Axis == "row")
                {
                    return;
                }

                if (args.Data.Axis == "column")
                {
                    return;
                }

                if (args.Data.Axis == "value" && args.Data.RowHeaders == "Grand Total")
                {
                    return;
                }

                if (args.Data.Axis == "value" && args.Data.ColumnHeaders == "Grand Total")
                {
                    return;
                }

                ProductionOrdersDetailList.Clear();

                ProductionOrdersDetailPopupVisible = true;

                ProductionOrdersDetailList = (await OpenOrderAnalysisAppService.GetProductionOrdersDetailListAsync(Convert.ToString(args.Data.RowHeaders), Convert.ToDateTime(args.Data.ColumnHeaders))).ToList();

            }
        }

        #region Production Orders Detail Modal


        SfGrid<ProductionOrdersDetailDto> ProductionOrdersGrid;

        bool ProductionOrdersDetailPopupVisible = false;

        List<ProductionOrdersDetailDto> ProductionOrdersDetailList = new List<ProductionOrdersDetailDto>();



        public async void HideProductionOrdersDetailPage()
        {
            ProductionOrdersDetailPopupVisible = false;
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
