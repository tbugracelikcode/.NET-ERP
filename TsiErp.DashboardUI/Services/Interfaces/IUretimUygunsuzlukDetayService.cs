using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IUretimUygunsuzlukDetayService
    {
        Task<List<ProductionUnsuitabilityDetailedStation>> GetProductionUnsuitabilityDetailedStationAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID);

        Task<List<ProductionUnsuitabilityDetailedEmployee>> GetProductionUnsuitabilityDetailedEmployeeAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID);

        Task<List<ProductionUnsuitabilityDetailedProduct>> GetProductionUnsuitabilityDetailedProductAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID);
    }
}
