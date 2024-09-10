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

        private void CellClick(CellClickEventArgs args)
        {
           
        }
    }
}
