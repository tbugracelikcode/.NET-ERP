using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.PurchaseManagement.PurchaseRequest.Reports
{
    [ServiceRegistration(typeof(IPurchaseRequestReportsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseRequestReportsAppService : IPurchaseRequestReportsAppService
    {
        private readonly IPurchaseRequestsAppService _purchaseRequestsAppService;
        private readonly IProductsAppService _productsAppService;

        public PurchaseRequestReportsAppService(IPurchaseRequestsAppService purchaseRequestsAppService, IProductsAppService productsAppService)
        {
            _purchaseRequestsAppService = purchaseRequestsAppService;
            _productsAppService = productsAppService;
        }

        public async Task<List<PurchaseRequestListReportDto>> GetPurchaseRequestListReport(PurchaseRequestListReportParameterDto filters, object reportLocalizer, object productLocalizer)
        {
            var reportloc = (IStringLocalizer)reportLocalizer;

            var productloc = (IStringLocalizer)productLocalizer;

            List<PurchaseRequestListReportDto> reportDatasource = new List<PurchaseRequestListReportDto>();


            var lines = (await _purchaseRequestsAppService.GetLineListAsync()).Data.AsQueryable();

            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();


            if (filters.StartDate.HasValue && filters.EndDate.HasValue)
            {
                lines = lines.Where(t => t.Date_ >= filters.StartDate.Value && t.Date_ <= filters.EndDate).AsQueryable();
            }

            if (filters.CurrentAccounts.Count > 0)
            {
                lines = lines.Where(t => filters.CurrentAccounts.Contains(t.CurrentAccountCardID)).AsQueryable();
            }

            if (filters.PurchaseRequestLineState.Count > 0)
            {
                lines = lines.Where(t => filters.PurchaseRequestLineState.Contains(t.PurchaseRequestLineState)).AsQueryable();
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

                if (currentProduct.ProductType > 0)
                {
                    var locKey = productloc[Enum.GetName(typeof(ProductTypeEnum), currentProduct.ProductType)];

                    PurchaseRequestListReportDto productLine = new PurchaseRequestListReportDto()
                    {
                        ProductCode = currentProduct.Code,
                        ProductName = currentProduct.Name,
                        ProductTypeName = productloc[GetProductTypeEnumStringKey(locKey)],
                        UnitSetCode = currentProduct.UnitSetCode
                    };

                    int lineNr = 1;

                    foreach (var product in productList)
                    {
                        var currentPurchaseRequest = (await _purchaseRequestsAppService.GetAsync(product.PurchaseRequestID.GetValueOrDefault())).Data;

                        PurchaseRequestLines purchaseRequestLine = new PurchaseRequestLines()
                        {
                            CurrentAcountCardCode = product.CurrentAccountCardCode,
                            CurrentAcountCardName = product.CurrentAccountCardName,
                            DiscountAmount = product.DiscountAmount,
                            DiscountRate = product.DiscountRate,
                            ExchangeRate = product.ExchangeRate,
                            FicheDate = product.Date_,
                            FicheNumber = currentPurchaseRequest.FicheNo,
                            LineAmount = product.LineAmount,
                            LineNr = lineNr,
                            LineTotalAmount = product.LineTotalAmount,
                            Quantity = product.Quantity,
                            UnitPrice = product.UnitPrice,
                            VATamount = product.VATamount,
                            VATrate = product.VATrate
                        };

                        productLine.PurchaseRequestLines.Add(purchaseRequestLine);

                        lineNr++;
                    }

                    reportDatasource.Add(productLine);
                }
            }

            return reportDatasource;
        }

        public static string GetProductTypeEnumStringKey(LocalizedString stateString)
        {
            string result = "";

            switch (stateString)
            {
                case "TM":
                    result = "EnumCommercialProduct";
                    break;
                case "HM":
                    result = "EnumMaterial";
                    break;
                case "YM":
                    result = "EnumSemiProduct";
                    break;
                case "MM":
                    result = "EnumProduct";
                    break;
                case "BP":
                    result = "EnumSparePart";
                    break;
                case "TK":
                    result = "EnumKit";
                    break;
                case "KLP":
                    result = "EnumMold";
                    break;
                case "APRT":
                    result = "EnumAparatus";
                    break;
            }

            return result;
        }
    }
}
