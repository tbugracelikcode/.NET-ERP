using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos
{
    public class ProductListReportParametersDto
    {
        public List<Guid> ProductGroups { get; set; }

        public List<ProductTypeEnum> ProductTypes { get; set; }

        public List<ProductSupplyFormEnum> ProductSupplyForms { get; set; }


        public ProductListReportParametersDto()
        {
            ProductGroups= new List<Guid>();
            ProductTypes = new List<ProductTypeEnum>();
            ProductSupplyForms = new List<ProductSupplyFormEnum>();
        }
    }
}
