using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.WorkOrder.ReportDtos.WorkOrderListReport
{
    public class WorkOrderListReportParameterDto
    {
        public List<Guid> ProductionOrders { get; set; }

        public List<Guid> Products { get; set; }

        public List<Guid> Stations { get; set; }

        public List<Guid> Orders { get; set; }

        public List<WorkOrderStateEnum> WorkOrderStates { get; set; }

        public WorkOrderListReportParameterDto()
        {
            ProductionOrders = new List<Guid>();
            Products = new List<Guid>();
            Stations = new List<Guid>();
            Orders = new List<Guid>();
            WorkOrderStates = new List<WorkOrderStateEnum>();
        }
    }
}
