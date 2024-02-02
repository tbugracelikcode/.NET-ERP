using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos;

namespace TsiErp.Business.Entities.PurchaseManagement.PurchaseRequest.Reports
{
    public interface IPurchaseRequestReportsAppService
    {

        Task<List<PurchaseRequestListReportDto>> GetPurchaseRequestListReport(PurchaseRequestListReportParameterDto filters, object reportLocalizer, object productLocalizer);
    }
}
