using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IIstasyonDetayService
    {
        Task<List<StationDetailedHaltAnalysis>> GetStationDetailedHaltAnalysis(int makineID, DateTime startDate, DateTime endDate);

        Task<List<StationDetailedHaltAnalysis>> GetStationDetailedHaltAnalysisChart(int makineID, DateTime startDate, DateTime endDate);

        Task<List<StationDetailedHaltAnalysisAll>> GetStationDetailedHaltAnalysisAll(int makineID, DateTime startDate, DateTime endDate);

        Task<List<StationDetailedHaltAnalysisAll>> GetStationDetailedHaltAnalysisAllChart(int makineID, DateTime startDate, DateTime endDate);


        Task<List<StationDetailedProductChart>> GetStationDetailedProductChart(int makineID, DateTime startDate, DateTime endDate, int products);

        Task<List<StationDetailedProductAnalysis>> GetStationDetailedProductAnalysis(int makineID, DateTime startDate, DateTime endDate);

        Task<List<StationDetailedEmployeeAnalysis>> GetStationDetailedEmployeeAnalysis(int makineID, DateTime startDate, DateTime endDate);
    }
}
