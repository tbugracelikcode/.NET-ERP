using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IStokDetayService
    {
        Task<List<ProductGroupDetailedChart>> GetProductGroupDetailedtChart(int productgroupID, DateTime startDate, DateTime endDate, int products);

        Task<List<ProductScrapAnalysis>> GetProductScrapAnalysis(int groupID, DateTime startDate, DateTime endDate);
    }
}
