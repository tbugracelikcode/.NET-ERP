using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IStokService
    {
        Task<List<AdminProductChart>> GetProductChart(DateTime startDate, DateTime endDate, int frequency, int? productionSelection);

        Task<List<ProductGroupsAnalysis>> GetProductGroupsAnalysis(DateTime startDate, DateTime endDate);

        Task<List<ProductGroupsAnalysis>> GetProductGroupsComboboxAnalysis(DateTime startDate, DateTime endDate);
    }
}
