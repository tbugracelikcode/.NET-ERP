using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.ReportDtos.ProductionTrackingListReport
{
    public class ProductionTrackingListReportDto
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitSetCode { get; set; }

        public List<ProductionTrackingListLines> ProductionTrackingListLines { get; set; }

        public ProductionTrackingListReportDto()
        {
            ProductionTrackingListLines= new List<ProductionTrackingListLines>();
        }
    }

    public class ProductionTrackingListLines
    {
        public int LineNr { get; set; }

        public string StationCode { get; set; }

        public string OperationName { get; set; }

        public string EmployeeName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsFinished { get; set; }

        public string WorkOrderCode { get; set; }

        public string ProductionOrderCode { get; set; }

        public decimal PlannedQuantity { get; set; }

        public decimal ProducedQuantity { get; set; }

    }
}
