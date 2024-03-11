using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockNumber.Dtos;

namespace TsiErp.Business.Entities.StockNumber.Services
{
    public interface IStockNumbersAppService : ICrudAppService<SelectStockNumbersDto, ListStockNumbersDto, CreateStockNumbersDto, UpdateStockNumbersDto, ListStockNumbersParameterDto>
    {
    }
}
