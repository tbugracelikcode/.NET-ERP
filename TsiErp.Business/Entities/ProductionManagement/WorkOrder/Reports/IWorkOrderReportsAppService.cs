using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.ReportDtos.WorkOrderListReport;

namespace TsiErp.Business.Entities.ProductionManagement.WorkOrder.Reports
{
    public interface IWorkOrderReportsAppService
    {
        Task<List<WorkOrderListReportDto>> GetWorkOrderListReport(WorkOrderListReportParameterDto filters);
    }
}
