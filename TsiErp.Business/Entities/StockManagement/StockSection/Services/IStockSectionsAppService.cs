using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockSection.Dtos;

namespace TsiErp.Business.Entities.StockSection.Services
{
    public interface IStockSectionsAppService : ICrudAppService<SelectStockSectionsDto, ListStockSectionsDto, CreateStockSectionsDto, UpdateStockSectionsDto, ListStockSectionsParameterDto>
    {
    }
}
