using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IGenelOEEService
    {
        Task<List<AdminMachineChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate);

        Task<List<StationOEEAnalysis>> GetStationOEEAnalysis(DateTime startDate, DateTime endDate);
    }
}
