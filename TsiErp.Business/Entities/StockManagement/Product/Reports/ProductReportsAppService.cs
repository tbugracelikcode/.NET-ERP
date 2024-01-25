using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos;

namespace TsiErp.Business.Entities.StockManagement.Product.Reports
{
    [ServiceRegistration(typeof(IProductReportsAppService), DependencyInjectionType.Scoped)]
    public class ProductReportsAppService : IProductReportsAppService
    {
        private readonly IProductsAppService _productsAppService;

        public ProductReportsAppService(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
        }

        public async Task<List<ProductListReportDto>> GetProductListReport(ProductListReportParametersDto filters)
        {
            List<ProductListReportDto> reportDatasource = new List<ProductListReportDto>();

            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (filters.ProductGroups.Count > 0)
            {
                products = products.Where(t => filters.ProductGroups.Contains(t.ProductGrpID)).AsQueryable();
            }

            if (filters.ProductTypes.Count > 0)
            {
                products = products.Where(t => filters.ProductTypes.Contains(t.ProductType)).AsQueryable();
            }

            if (filters.ProductSupplyForms.Count > 0)
            {
                products = products.Where(t => filters.ProductSupplyForms.Contains(t.SupplyForm)).AsQueryable();
            }


            var productList = products.ToList();

            int lineNr = 1;

            foreach (var product in productList)
            {
                reportDatasource.Add(new ProductListReportDto
                {
                    LineNr = lineNr,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    UnitSetCode = product.UnitSetCode,
                    ProductTypeName = ""
                });

                lineNr++;
            }


            return reportDatasource;
        }
    }
}
