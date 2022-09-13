using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IIstasyonOEEService
    {
        Task<List<AdminMachineChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate, int frequency);

        Task<List<StationOEEAnalysis>> GetStationOEEAnalysis(DateTime startDate, DateTime endDate);
    }
}
