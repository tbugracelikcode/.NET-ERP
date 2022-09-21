using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IIstasyonService
    {
        Task<List<StationGeneralAnalysis>> GetStationGeneralAnalyies(DateTime startDate, DateTime endDate);
    }
}
