using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos
{
    public class ProductWarehouseStatusReportParametersDto
    {
        public List<Guid> StockCards { get; set; }

        public List<Guid> Branches { get; set; }

        public List<Guid> Warehouses { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProductWarehouseStatusReportParametersDto()
        {
            StockCards = new List<Guid>();
            Branches = new List<Guid>();
            Warehouses = new List<Guid>(); 
        }

    }
}
