using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.SalesOrder.Services;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.ReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.Business.Entities.SalesManagement.SalesOrder.Reports
{
    [ServiceRegistration(typeof(ISalesOrderReportsAppService), DependencyInjectionType.Scoped)]
    public class SalesOrderReportsAppService : ISalesOrderReportsAppService
    {

        private readonly ISalesOrdersAppService _salesOrdersAppService;
        private readonly IProductsAppService _productsAppService;

        public SalesOrderReportsAppService(ISalesOrdersAppService salesOrdersAppService, IProductsAppService productsAppService)
        {
            _salesOrdersAppService = salesOrdersAppService;
            _productsAppService = productsAppService;
        }

        public async Task<List<SalesOrderListReportDto>> GetSalesOrderListReport(SalesOrderListReportParameterDto filters, object reportLocalizer)
        {
            var reportloc = (IStringLocalizer)reportLocalizer;

            List<SalesOrderListReportDto> reportDatasource = new List<SalesOrderListReportDto>();

            var lines = (await _salesOrdersAppService.GetLineSelectListAsync()).Data.AsQueryable();

            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (filters.StartDate.HasValue && filters.EndDate.HasValue)
            {
                lines = lines.Where(t => t.Date_ >= filters.StartDate.Value && t.Date_ <= filters.EndDate).AsQueryable();
            }

            if (filters.CurrentAccounts.Count > 0)
            {
                lines = lines.Where(t => filters.CurrentAccounts.Contains(t.CurrentAccountCardID)).AsQueryable();
            }

            if (filters.SalesOrderLineState.Count > 0)
            {
                lines = lines.Where(t => filters.SalesOrderLineState.Contains(t.SalesOrderLineStateEnum)).AsQueryable();
            }

            if (filters.Products.Count > 0)
            {
                lines = lines.Where(t => filters.Products.Contains(t.ProductID.GetValueOrDefault())).AsQueryable();
            }

            var lineList = lines.ToList();

            var groupedList = lineList.GroupBy(t => t.ProductID).ToList();

            foreach (var item in groupedList)
            {
                var productList = item.ToList();
                var productId = item.Key;

                var currentProduct = products.FirstOrDefault(t => t.Id == productId);

                SalesOrderListReportDto productLine = new SalesOrderListReportDto()
                {
                    ProductCode = currentProduct.Code,
                    ProductName = currentProduct.Name,
                    UnitSetCode = currentProduct.UnitSetCode
                };

                int lineNr = 1;

                foreach (var product in productList)
                {
                    var currentSalesOrder = (await _salesOrdersAppService.GetAsync(product.SalesOrderID)).Data;

                    SalesOrderLines SalesOrderLine = new SalesOrderLines()
                    {
                        CurrentAcountCardCode = product.CurrentAccountCardCode,
                        CurrentAcountCardName = product.CurrentAccountCardName,
                        DiscountAmount = product.DiscountAmount,
                        DiscountRate = product.DiscountRate,
                        ExchangeRate = product.ExchangeRate,
                        FicheDate = product.Date_,
                        FicheNumber = currentSalesOrder.FicheNo,
                        LineAmount = product.LineAmount,
                        LineNr = lineNr,
                        LineTotalAmount = product.LineTotalAmount,
                        Quantity = product.Quantity,
                        UnitPrice = product.UnitPrice,
                        VATamount = product.VATamount,
                        VATrate = product.VATrate
                    };

                    productLine.SalesOrderLines.Add(SalesOrderLine);

                    lineNr++;
                }

                reportDatasource.Add(productLine);

            }

            return reportDatasource;
        }
    }
}
