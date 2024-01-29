using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.Employee.Reports
{
    public interface IEmployeeReportsAppService
    {
        Task<List<EmployeeListReportDto>> GetEmployeeListReport(EmployeeListReportParameterDto filters, object localizer);
    }
}
