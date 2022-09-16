using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IFasonUygunsuzlukDetayService
    {
        Task<List<ContractUnsuitabilityAnalysis>> GetContractUnsuitabilityDetailedChart(DateTime startDate, DateTime endDate, int frequency, int? action, int cariID, int total);

        Task<List<ContractUnsuitabilityAnalysis>> GetContractUnsuitabilityDetailed(DateTime startDate, DateTime endDate, int cariID);
    }
}
