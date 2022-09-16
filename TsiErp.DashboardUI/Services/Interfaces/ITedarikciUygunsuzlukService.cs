using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface ITedarikciUygunsuzlukService
    {
        Task<List<AdminSupplierUnsuitabilityAnalysisChart>> GetSupplierUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency, int? action);

        Task<List<SupplierUnsuitabilityAnalysis>> GetSupplierUnsuitabilityAnalysis(DateTime startDate, DateTime endDate);
    }
}
