using Microsoft.Extensions.Localization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Net.Sockets;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.ByDateStockMovement.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.Warehouse.Services;
using TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.StockManagement.Product.Reports
{
    [ServiceRegistration(typeof(IProductReportsAppService), DependencyInjectionType.Scoped)]
    public class ProductReportsAppService : IProductReportsAppService
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IByDateStockMovementsAppService _byDateStockMovementsAppService;

        public ProductReportsAppService(IProductsAppService productsAppService, IByDateStockMovementsAppService byDateStockMovementsAppService)
        {
            _productsAppService = productsAppService;
            _byDateStockMovementsAppService = byDateStockMovementsAppService;
        }

        #region Product List Report
        public async Task<List<ProductListReportDto>> GetProductListReport(ProductListReportParametersDto filters, object localizer)
        {
            var loc = (IStringLocalizer)localizer;

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
                if(product.ProductType>0)
                {
                    var locKey = loc[Enum.GetName(typeof(ProductTypeEnum), product.ProductType)];

                    reportDatasource.Add(new ProductListReportDto
                    {
                        LineNr = lineNr,
                        ProductCode = product.Code,
                        ProductName = product.Name,
                        UnitSetCode = product.UnitSetCode,
                        ProductTypeName = loc[GetProductTypeEnumStringKey(locKey)]
                    });

                    lineNr++;
                }
            }
            return reportDatasource;
        }
        #endregion


        #region Product Warehouse Status Report
        public async Task<List<ProductWarehouseStatusReportDto>> GetProductWarehouseStatusReport(ProductWarehouseStatusReportParametersDto filters, object localizer)
        {
            var loc = (IStringLocalizer)localizer;

            List<ProductWarehouseStatusReportDto> reportDatasource = new List<ProductWarehouseStatusReportDto>();

            var lines = (await _byDateStockMovementsAppService.GetListAsync(new ListByDateStockMovementsParameterDto())).Data.AsQueryable();

            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (filters.StartDate.HasValue && filters.EndDate.HasValue)
            {
                lines = lines.Where(t => t.Date_ >= filters.StartDate.Value && filters.EndDate <= t.Date_).AsQueryable();
            }

            if (filters.Warehouses.Count > 0)
            {
                lines = lines.Where(t => filters.Warehouses.Contains(t.WarehouseID)).AsQueryable();
            }

            if (filters.Branches.Count > 0)
            {
                lines = lines.Where(t => filters.Branches.Contains(t.BranchID)).AsQueryable();
            }

            if (filters.StockCards.Count > 0)
            {
                lines = lines.Where(t => filters.StockCards.Contains(t.ProductID)).AsQueryable();
            }


            var lineList = lines.ToList();

            var groupedList = lineList.GroupBy(t => t.ProductID).ToList();

            foreach (var line in groupedList)
            {
                

            }

            //var product = products.FirstOrDefault(t => t.Id == line.ProductID);

            //var locKey = loc[Enum.GetName(typeof(ProductTypeEnum), product.ProductType)];

            //ProductWarehouseStatusReportDto productLine = new ProductWarehouseStatusReportDto()
            //{
            //    ProductCode = product.Code,
            //    ProductName = product.Name,
            //    ProductTypeName = loc[GetProductTypeEnumStringKey(locKey)],
            //    UnitSetCode = product.UnitSetCode
            //};

            return reportDatasource;
        }
        #endregion


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
