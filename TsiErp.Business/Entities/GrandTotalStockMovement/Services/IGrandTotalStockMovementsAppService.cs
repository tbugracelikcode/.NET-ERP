using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GrandTotalStockMovement.Dtos;

namespace TsiErp.Business.Entities.GrandTotalStockMovement.Services
{
    public interface IGrandTotalStockMovementsAppService : ICrudAppService<SelectGrandTotalStockMovementsDto, ListGrandTotalStockMovementsDto, CreateGrandTotalStockMovementsDto, UpdateGrandTotalStockMovementsDto, ListGrandTotalStockMovementsParameterDto>
    {
    }
}
