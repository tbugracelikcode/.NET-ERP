using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.ReportDtos.PurchaseOrderListReportDtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos;

namespace TsiErp.Business.Entities.PurchaseManagement.PurchaseOrder.Reports
{
    public interface IPurchaseOrderReportsAppService
    {
        Task<List<PurchaseOrderListReportDto>> GetPurchaseOrderListReport(PurchaseOrderListReportParameterDto filters, object reportLocalizer, object productLocalizer);
    }
}
