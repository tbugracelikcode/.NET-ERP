using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockShelf.Dtos;

namespace TsiErp.Business.Entities.StockShelf.Services
{
    public interface IStockShelfsAppService : ICrudAppService<SelectStockShelfsDto, ListStockShelfsDto, CreateStockShelfsDto, UpdateStockShelfsDto, ListStockShelfsParameterDto>
    {
    }
}
