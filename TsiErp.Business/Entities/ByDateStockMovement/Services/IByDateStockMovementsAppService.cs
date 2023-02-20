using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ByDateStockMovement.Dtos;

namespace TsiErp.Business.Entities.ByDateStockMovement.Services
{
    public interface IByDateStockMovementsAppService : ICrudAppService<SelectByDateStockMovementsDto, ListByDateStockMovementsDto, CreateByDateStockMovementsDto, UpdateByDateStockMovementsDto, ListByDateStockMovementsParameterDto>
    {
    }
}
