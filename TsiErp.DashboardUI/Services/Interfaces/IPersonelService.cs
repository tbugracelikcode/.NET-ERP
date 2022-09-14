using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services.Interfaces
{
    public interface IPersonelService
    {
        Task<List<AdminEmployeeChart>> GetEmployeeChart(DateTime startDate, DateTime endDate, int frequency);

        Task<List<EmployeeGeneralAnalysis>> GetEmployeeGeneralAnalysis(DateTime startDate, DateTime endDate);
    }
}
