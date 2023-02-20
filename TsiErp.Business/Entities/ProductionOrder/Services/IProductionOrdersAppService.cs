using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    public interface IProductionOrdersAppService : ICrudAppService<SelectProductionOrdersDto, ListProductionOrdersDto, CreateProductionOrdersDto, UpdateProductionOrdersDto, ListProductionOrdersParameterDto>
    {
    }
}
