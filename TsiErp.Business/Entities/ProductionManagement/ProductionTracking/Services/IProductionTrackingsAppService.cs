using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    public interface IProductionTrackingsAppService : ICrudAppService<SelectProductionTrackingsDto, ListProductionTrackingsDto, CreateProductionTrackingsDto, UpdateProductionTrackingsDto, ListProductionTrackingsParameterDto>
    {
    }
}
