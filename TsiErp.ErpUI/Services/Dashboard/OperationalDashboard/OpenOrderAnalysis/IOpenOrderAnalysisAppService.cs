using Syncfusion.Blazor.Grids;
using System.Dynamic;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Models.Dashboard.OperationalDashboard.OpenOrderAnalysis;

namespace TsiErp.ErpUI.Services.Dashboard.OperationalDashboard.OpenOrderAnalysis
{
    public interface IOpenOrderAnalysisAppService
    {
        Task<List<CurrentBalanceAndQuantityTableDto>> GetCurrentBalanceAndQuantityListAsync();

        Task<List<ProductionOrdersDetailDto>> GetProductionOrdersDetailListAsync(string productGroupName, DateTime confirmedLoadingDate);
    }
}
