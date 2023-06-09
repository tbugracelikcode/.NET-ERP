using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeItem.Dtos;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.Services
{
    public interface IProductionOrderChangeItemsAppService : ICrudAppService<SelectProductionOrderChangeItemsDto, ListProductionOrderChangeItemsDto, CreateProductionOrderChangeItemsDto, UpdateProductionOrderChangeItemsDto, ListProductionOrderChangeItemsParameterDto>
    {
    }
}
