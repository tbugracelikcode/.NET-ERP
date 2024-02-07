using TsiErp.Entities.Entities.SalesManagement.SalesOrder.ReportDtos;

namespace TsiErp.Business.Entities.SalesManagement.SalesOrder.Reports
{
    public interface ISalesOrderReportsAppService
    {
        Task<List<SalesOrderListReportDto>> GetSalesOrderListReport(SalesOrderListReportParameterDto filters, object reportLocalizer);
    }
}
