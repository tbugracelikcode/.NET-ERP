using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IFasonUygunsuzlukService
    {
        Task<List<ContractUnsuitabilityAnalysis>> GetContractUnsuitabilityAnalysis(DateTime startDate, DateTime endDate);

    }
}
