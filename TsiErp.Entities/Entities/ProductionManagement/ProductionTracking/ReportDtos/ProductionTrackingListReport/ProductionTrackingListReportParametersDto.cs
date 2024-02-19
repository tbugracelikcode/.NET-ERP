using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.ReportDtos.ProductionTrackingListReport
{
    public class ProductionTrackingListReportParametersDto
    {
        public List<Guid> ProductionOrders { get; set; }

        public List<Guid> Employees { get; set; }

        public List<Guid> Products { get; set; }

        public List<Guid> Stations { get; set; }

        public ProductionTrackingListReportParametersDto()
        {
            ProductionOrders = new List<Guid>();
            Employees = new List<Guid>();
            Products = new List<Guid>();
            Stations = new List<Guid>();
        }
    }
}
