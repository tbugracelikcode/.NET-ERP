using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IUrunGrubuPerformansService
    {
        Task<List<ProductGroupPerformanceAnalysis>> GetProductGroupPerformanceAnalysis(DateTime startDate, DateTime endDate);

        Task<List<AdminProductGroupPerformanceAnalysisChart>> GetProductGroupPerformanceAnalysisChart(DateTime startDate, DateTime endDate, int frequency);
    }
}
