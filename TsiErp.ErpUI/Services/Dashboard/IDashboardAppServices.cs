using TsiErp.ErpUI.Models.Dashboard;

namespace TsiErp.ErpUI.Services.Dashboard
{
    public interface IDashboardAppServices
    {
        Task<List<AdminProductChart>> GetProductChart(DateTime startDate, DateTime endDate, int frequency, Guid? productionSelection);
        Task<List<ProductGroupsAnalysis>> GetProductGroupsAnalysis(DateTime startDate, DateTime endDate);
        Task<List<ProductGroupsAnalysis>> GetProductGroupsComboboxAnalysis(DateTime startDate, DateTime endDate);
    }
}
