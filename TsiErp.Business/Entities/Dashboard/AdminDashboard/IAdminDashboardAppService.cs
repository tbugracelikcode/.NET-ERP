using TsiErp.Business.Models.AdminDashboard;

namespace TsiErp.Business.Entities.Dashboard.AdminDashboard
{
    public interface IAdminDashboardAppService
    {
        Task<List<AdminOveralOEEChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate);
    }
}
