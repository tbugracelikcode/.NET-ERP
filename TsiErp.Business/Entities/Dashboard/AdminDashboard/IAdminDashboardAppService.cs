using TsiErp.Business.Models.AdminDashboard;

namespace TsiErp.Business.Entities.Dashboard.AdminDashboard
{
    public interface IAdminDashboardAppService
    {
        Task<List<AdminOveralOEEChart>> GetAdminOveralChart(DateTime startDate, DateTime endDate);
        Task<List<AdminMachineOEEChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate);
    }
}
