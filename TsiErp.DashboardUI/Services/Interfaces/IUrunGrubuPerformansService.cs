using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IUrunGrubuPerformansService
    {
        Task<List<ProductGroupPerformanceAnalysis>> GetProductGroupPerformanceAnalysis(DateTime startDate, DateTime endDate, int? productionSelection, int frequency);

        Task<List<AdminProductGroupPerformanceAnalysisChart>> GetProductGroupPerformanceAnalysisChart(DateTime startDate, DateTime endDate, int frequency, int? productionSelection);

        Task<List<ProductGroupsAnalysis>> GetProductGroupsComboboxAnalysis(DateTime startDate, DateTime endDate);
    }
}
