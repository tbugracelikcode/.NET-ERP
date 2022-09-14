using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IPersonelDetayService
    {
        Task<List<EmployeeDetailedChart>> GetEmployeeDetailedtChart(int calisanID, DateTime startDate, DateTime endDate);

        Task<List<EmployeeDetailedHaltAnalysis>> GetStationDetailedHaltAnalysis(int calisanID, DateTime startDate, DateTime endDate);
    }
}
