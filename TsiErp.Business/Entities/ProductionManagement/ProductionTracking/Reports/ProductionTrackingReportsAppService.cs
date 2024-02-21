using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Business.Entities.StockManagement.Product.Reports;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.ReportDtos.ProductionTrackingListReport;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos;

namespace TsiErp.Business.Entities.ProductionManagement.ProductionTracking.Reports
{
    [ServiceRegistration(typeof(IProductionTrackingReportsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingReportsAppService : IProductionTrackingReportsAppService
    {
        private readonly IProductionTrackingsAppService _productionTrackingsAppService;

        private readonly IProductsAppService _productsAppService;


        public ProductionTrackingReportsAppService(IProductionTrackingsAppService productionTrackingsAppService, IProductsAppService productsAppService)
        {
            _productionTrackingsAppService = productionTrackingsAppService;
            _productsAppService = productsAppService;
        }

        public async Task<List<ProductionTrackingListReportDto>> GetProductionTrackingListReport(ProductionTrackingListReportParametersDto filters)
        {
            List<ProductionTrackingListReportDto> reportDatasource = new List<ProductionTrackingListReportDto>();

            var lines = (await _productionTrackingsAppService.GetListAsync(new ListProductionTrackingsParameterDto())).Data.AsQueryable();



            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (filters.ProductionOrders.Count > 0)
            {
                lines = lines.Where(t => filters.ProductionOrders.Contains(t.ProductionOrderID)).AsQueryable();
            }

            if (filters.Employees.Count > 0)
            {
                lines = lines.Where(t => filters.Employees.Contains(t.EmployeeID)).AsQueryable();
            }

            if (filters.Stations.Count > 0)
            {
                lines = lines.Where(t => filters.Stations.Contains(t.StationID)).AsQueryable();
            }

            if (filters.Products.Count > 0)
            {
                lines = lines.Where(t => filters.Products.Contains(t.ProductID)).AsQueryable();
            }

            var lineList = lines.ToList();

            var groupedList = lineList.GroupBy(t => new { t.ProductID, t.ProductionOrderID }).ToList();

            foreach (var item in groupedList)
            {
                var productionTrackingList = item.ToList();
                var productId = item.Key.ProductID;

                var currentProduct = products.FirstOrDefault(t => t.Id == productId);

                if (currentProduct != null)
                {
                    ProductionTrackingListReportDto productLine = new ProductionTrackingListReportDto
                    {
                        ProductCode = currentProduct.Code,
                        ProductName = currentProduct.Name,
                        UnitSetCode = currentProduct.UnitSetCode
                    };

                    int lineNr = 1;

                    foreach (var pt in productionTrackingList)
                    {

                        ProductionTrackingListLines line = new ProductionTrackingListLines
                        {
                            LineNr = lineNr,
                            EmployeeName = pt.EmployeeName,
                            IsFinished = pt.IsFinished,
                            StationCode = pt.StationCode,
                            WorkOrderCode = pt.WorkOrderCode,
                            OperationName = pt.ProductOperationName,
                            PlannedQuantity = pt.PlannedQuantity,
                            ProducedQuantity = pt.ProducedQuantity,
                            ProductionOrderCode =pt.ProductionOrderCode,
                            StartDate = pt.OperationStartDate + " - " + pt.OperationStartTime,
                            EndDate = pt.OperationEndDate + " - " + pt.OperationEndTime
                        };

                        productLine.ProductionTrackingListLines.Add(line);

                        lineNr++;
                    }
                }
            }

            return reportDatasource;
        }
    }
}
