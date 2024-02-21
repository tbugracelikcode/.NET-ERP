using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.ReportDtos.WorkOrderListReport;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.ProductionManagement.WorkOrder.Reports
{
    [ServiceRegistration(typeof(IWorkOrderReportsAppService), DependencyInjectionType.Scoped)]
    public class WorkOrderReportsAppService : IWorkOrderReportsAppService
    {
        private readonly IWorkOrdersAppService _workOrdersAppService;
        private readonly IProductsAppService _productsAppService;

        public WorkOrderReportsAppService(IWorkOrdersAppService workOrdersAppService, IProductsAppService productsAppService)
        {
            _workOrdersAppService = workOrdersAppService;
            _productsAppService = productsAppService;
        }

        public async Task<List<WorkOrderListReportDto>> GetWorkOrderListReport(WorkOrderListReportParameterDto filters)
        {

            List<WorkOrderListReportDto> reportDatasource = new List<WorkOrderListReportDto>();

            var lines = (await _workOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.AsQueryable();
            var products = (await _productsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (filters.ProductionOrders.Count > 0)
            {
                lines = lines.Where(t => filters.ProductionOrders.Contains(t.ProductionOrderID.GetValueOrDefault())).AsQueryable();
            }

            if (filters.Stations.Count > 0)
            {
                lines = lines.Where(t => filters.Stations.Contains(t.StationID.GetValueOrDefault())).AsQueryable();
            }

            if (filters.Products.Count > 0)
            {
                lines = lines.Where(t => filters.Products.Contains(t.ProductID.GetValueOrDefault())).AsQueryable();
            }

            if (filters.Orders.Count > 0)
            {
                lines = lines.Where(t => filters.Orders.Contains(t.OrderID)).AsQueryable();
            }

            if (filters.WorkOrderStates.Count > 0)
            {
                lines = lines.Where(t => filters.WorkOrderStates.Contains(t.WorkOrderState)).AsQueryable();
            }

            var lineList = lines.ToList();

            var groupedList = lineList.GroupBy(t => new { t.ProductID, t.ProductionOrderID }).ToList();

            foreach (var item in groupedList)
            {
                var workOrderList = item.ToList();
                var productId = item.Key.ProductID;

                var currentProduct = products.FirstOrDefault(t => t.Id == productId);
                if (currentProduct != null)
                {
                    WorkOrderListReportDto productLine = new WorkOrderListReportDto
                    {
                        ProductCode = currentProduct.Code,
                        ProductName = currentProduct.Name,
                        UnitSetCode = currentProduct.UnitSetCode
                    };

                    int lineNr = 1;

                    foreach (var pt in workOrderList)
                    {

                        WorkOrderListLines line = new WorkOrderListLines
                        {
                            LineNr = lineNr,
                            StationCode = pt.StationCode,
                            WorkOrderCode = pt.WorkOrderNo,
                            OperationName = pt.ProductsOperationName,
                            PlannedQuantity = pt.PlannedQuantity,
                            ProducedQuantity = pt.ProducedQuantity,
                            ProductionOrderCode = pt.ProductionOrderFicheNo,
                            WorkOrderState = GetWorkOrderEnumStringKey(pt.WorkOrderState)
                        };

                        productLine.WorkOrderListLines.Add(line);

                        lineNr++;
                    }
                }
            }

            return reportDatasource;
        }

        public static string GetWorkOrderEnumStringKey(WorkOrderStateEnum stateEnum)
        {
            string result = "";

            switch (stateEnum)
            {
                case WorkOrderStateEnum.Baslamadi:
                    result = "Başlamadı";
                    break;
                case WorkOrderStateEnum.Durduruldu:
                    result = "Durduruldu";
                    break;
                case WorkOrderStateEnum.Iptal:
                    result = "İptal";
                    break;
                case WorkOrderStateEnum.DevamEdiyor:
                    result = "Devam Ediyor";
                    break;
                case WorkOrderStateEnum.Tamamlandi:
                    result = "Tamamlandı";
                    break;
                case WorkOrderStateEnum.FasonaGonderildi:
                    result = "Fasona Gönderildi";
                    break;
            }

            return result;
        }
    }
}
