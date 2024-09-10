using DevExpress.Export;
using DevExpress.Utils.Filtering;
using Syncfusion.Blazor.Grids;
using System.Dynamic;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Models.Dashboard.OperationalDashboard.OpenOrderAnalysis;

namespace TsiErp.ErpUI.Services.Dashboard.OperationalDashboard.OpenOrderAnalysis
{
    public class OpenOrderAnalysisAppService : IOpenOrderAnalysisAppService
    {
        private readonly IProductionOrdersAppService _productionOrdersAppService;

        public OpenOrderAnalysisAppService(IProductionOrdersAppService productionOrdersAppService)
        {
            _productionOrdersAppService = productionOrdersAppService;
        }

        public async Task<List<CurrentBalanceAndQuantityTableDto>> GetCurrentBalanceAndQuantityListAsync()
        {
            List<CurrentBalanceAndQuantityTableDto> result = new List<CurrentBalanceAndQuantityTableDto>();

            List<ProductionOrderStateEnum> orderStates = new List<ProductionOrderStateEnum>()
            {
                ProductionOrderStateEnum.Baslamadi,
                ProductionOrderStateEnum.Durduruldu,
                ProductionOrderStateEnum.Iptal,
                ProductionOrderStateEnum.DevamEdiyor,
                ProductionOrderStateEnum.Tamamlandi
            };


            var productionOrders = (await _productionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto()))
                .Data
                .Where(t => t.ProductType == ProductTypeEnum.MM && orderStates.Contains(t.ProductionOrderState))
                .ToList();



            for (int i = 0; i < productionOrders.Count; i++)
            {
                CurrentBalanceAndQuantityTableDto dto = new CurrentBalanceAndQuantityTableDto
                {
                    ProductGroupName = productionOrders[i].ProductGroupName,
                    LoadingDate = productionOrders[i].ConfirmedLoadingDate.ToShortDateString(),
                    Value = (int)productionOrders[i].PlannedQuantity
                };

                result.Add(dto);

            }

            await Task.CompletedTask;
            return result;
        }
    }
}
