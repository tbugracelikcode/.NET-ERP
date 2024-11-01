using TsiErp.Business.Models.AdminDashboard;

namespace TsiErp.Business.Entities.Dashboard.AdminDashboard
{
    public interface IAdminDashboardAppService
    {
        Task<List<AdminOveralOEEChart>> GetAdminOveralChart(DateTime startDate, DateTime endDate);
        Task<List<AdminMachineOEEChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate);
        Task<List<AdminMachineOEEGrid>> GetAdminMachineGrid(DateTime startDate, DateTime endDate);
        Task<List<AdminEmployeeOEEChart>> GetAdminEmployeeChart(DateTime startDate, DateTime endDate);
        Task<List<AdminEmployeeOEEGrid>> GetAdminEmployeeGrid(DateTime startDate, DateTime endDate);
        Task<List<AdminProductGroupAnalysisChart>> GetAdminProductGroupChart(DateTime startDate, DateTime endDate, Guid productGroupID);
        Task<List<AdminProductGroupAnalysisBarChart>> GetAdminProductGroupBarChart(DateTime startDate, DateTime endDate, Guid productGroupID);
        Task<List<AdminContractUnsuitabilityChart>> GetAdminContractUnsuitabilityChart(DateTime startDate, DateTime endDate);
        Task<List<AdminContractUnsuitabilityGrid>> GetAdminContractUnsuitabilityGrid(DateTime startDate, DateTime endDate);
        Task<List<AdminPurchaseUnsuitabilityChart>> GetAdminPurchaseUnsuitabilityChart(DateTime startDate, DateTime endDate);
        Task<List<AdminPurchaseUnsuitabilityGrid>> GetAdminPurchaseUnsuitabilityGrid(DateTime startDate, DateTime endDate);
    }
}
