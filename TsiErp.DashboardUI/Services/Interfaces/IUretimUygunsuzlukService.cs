using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IUretimUygunsuzlukService
    {
        Task<List<AdminProductionUnsuitabilityAnalysisChart>> GetProductionUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency, int? action);

        Task<List<ProductionUnsuitabilityAnalysis>> GetProductionUnsuitabilityAnalysis(DateTime startDate, DateTime endDate);
    }
}
