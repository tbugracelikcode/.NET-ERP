using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.ReportDtos;

namespace TsiErp.Business.Entities.SalesManagement.SalesProposition.Reports
{
    public interface ISalesPropositionReportsAppService
    {
        Task<List<SalesPropositionListReportDto>> GetSalesPropositionListReport(SalesPropositionListReportParameterDto filters, object reportLocalizer);
    }
}
