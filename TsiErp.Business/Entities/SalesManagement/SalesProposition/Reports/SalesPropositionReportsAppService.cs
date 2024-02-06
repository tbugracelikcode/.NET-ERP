using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.ReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.Business.Entities.SalesManagement.SalesProposition.Reports
{
    [ServiceRegistration(typeof(ISalesPropositionReportsAppService), DependencyInjectionType.Scoped)]
    public class SalesPropositionReportsAppService : ISalesPropositionReportsAppService
    {

        private readonly ISalesPropositionsAppService _salesPropositionsAppService;
        private readonly IProductsAppService _productsAppService;

        public SalesPropositionReportsAppService(ISalesPropositionsAppService salesPropositionsAppService, IProductsAppService productsAppService)
        {
            _salesPropositionsAppService = salesPropositionsAppService;
            _productsAppService = productsAppService;
        }

        public async Task<List<SalesPropositionListReportDto>> GetSalesPropositionListReport(SalesPropositionListReportParameterDto filters, object reportLocalizer)
        {
            var reportloc = (IStringLocalizer)reportLocalizer;

            List<SalesPropositionListReportDto> reportDatasource = new List<SalesPropositionListReportDto>();

            var lines = (await _salesPropositionsAppService.GetLineListAsync()).Data.AsQueryable();

            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (filters.StartDate.HasValue && filters.EndDate.HasValue)
            {
                lines = lines.Where(t => t.Date_ >= filters.StartDate.Value && t.Date_ <= filters.EndDate).AsQueryable();
            }

            if (filters.CurrentAccounts.Count > 0)
            {
                lines = lines.Where(t => filters.CurrentAccounts.Contains(t.CurrentAccountCardID)).AsQueryable();
            }

            if (filters.SalesPropositionLineState.Count > 0)
            {
                lines = lines.Where(t => filters.SalesPropositionLineState.Contains(t.SalesPropositionLineState)).AsQueryable();
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

                    SalesPropositionListReportDto productLine = new SalesPropositionListReportDto()
                    {
                        ProductCode = currentProduct.Code,
                        ProductName = currentProduct.Name,
                        UnitSetCode = currentProduct.UnitSetCode
                    };

                    int lineNr = 1;

                    foreach (var product in productList)
                    {
                        var currentSalesProposition = (await _salesPropositionsAppService.GetAsync(product.SalesPropositionID)).Data;

                        SalesPropositionLines SalesPropositionLine = new SalesPropositionLines()
                        {
                            CurrentAcountCardCode = product.CurrentAccountCardCode,
                            CurrentAcountCardName = product.CurrentAccountCardName,
                            DiscountAmount = product.DiscountAmount,
                            DiscountRate = product.DiscountRate,
                            ExchangeRate = product.ExchangeRate,
                            FicheDate = product.Date_,
                            FicheNumber = currentSalesProposition.FicheNo,
                            LineAmount = product.LineAmount,
                            LineNr = lineNr,
                            LineTotalAmount = product.LineTotalAmount,
                            Quantity = product.Quantity,
                            UnitPrice = product.UnitPrice,
                            VATamount = product.VATamount,
                            VATrate = product.VATrate
                        };

                        productLine.SalesPropositionLines.Add(SalesPropositionLine);

                        lineNr++;
                    }

                    reportDatasource.Add(productLine);
                
            }

            return reportDatasource;
        }
    }
}
