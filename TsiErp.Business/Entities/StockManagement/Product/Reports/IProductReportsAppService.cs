using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos;

namespace TsiErp.Business.Entities.StockManagement.Product.Reports
{
    public interface IProductReportsAppService
    {
        Task<List<ProductListReportDto>> GetProductListReport(ProductListReportParametersDto filters, object localizer);
        Task<List<ProductWarehouseStatusReportDto>> GetProductWarehouseStatusReport(ProductWarehouseStatusReportParametersDto filters, object localizer);
    }
}
