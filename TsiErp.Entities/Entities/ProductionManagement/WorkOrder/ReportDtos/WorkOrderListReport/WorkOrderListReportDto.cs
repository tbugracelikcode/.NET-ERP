using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ProductionManagement.WorkOrder.ReportDtos.WorkOrderListReport
{
    public class WorkOrderListReportDto
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitSetCode { get; set; }

        public List<WorkOrderListLines> WorkOrderListLines { get; set; }

        public WorkOrderListReportDto()
        {
            WorkOrderListLines = new List<WorkOrderListLines>();
        }
    }

    public class WorkOrderListLines
    {
        public int LineNr { get; set; }

        public string WorkOrderCode { get; set; }

        public string StationCode { get; set; }

        public string OperationName { get; set; }

        public string ProductionOrderCode { get; set; }

        public decimal PlannedQuantity { get; set; }

        public decimal ProducedQuantity { get; set; }

        public string WorkOrderState { get; set; }
    }
}
