using Microsoft.AspNetCore.Components;
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


        protected override async void OnInitialized()
        {
            GridList = (await OpenOrderAnalysisAppService.GetCurrentBalanceAndQuantityListAsync()).ToList();

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
