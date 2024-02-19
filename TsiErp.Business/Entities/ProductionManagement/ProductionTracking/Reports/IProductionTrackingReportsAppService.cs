using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.ReportDtos.ProductionTrackingListReport;

namespace TsiErp.Business.Entities.ProductionManagement.ProductionTracking.Reports
{
    public interface IProductionTrackingReportsAppService
    {
        Task<List<ProductionTrackingListReportDto>> GetProductionTrackingListReport(ProductionTrackingListReportParametersDto filters);
    }
}
