using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos
{
    public class ProductListReportDto
    {
        public int LineNr { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitSetCode { get; set; }

        public string ProductTypeName { get; set; }
    }
}
