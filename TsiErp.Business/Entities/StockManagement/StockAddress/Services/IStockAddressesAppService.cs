using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;

namespace TsiErp.Business.Entities.StockAddress.Services
{
    public interface IStockAddressesAppService : ICrudAppService<SelectStockAddressesDto, ListStockAddressesDto, CreateStockAddressesDto, UpdateStockAddressesDto, ListStockAddressesParameterDto>
    {
    }
}
