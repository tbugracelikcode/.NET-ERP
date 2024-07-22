using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos;

namespace TsiErp.Business.Entities.StockAddress.Services
{
    public interface IStockAddressesAppService : ICrudAppService<SelectStockAddressesDto, ListStockAddressesDto, CreateStockAddressesDto, UpdateStockAddressesDto, ListStockAddressesParameterDto>
    {
        Task<IDataResult<IList<SelectStockAddressLinesDto>>> GetStockAddressByStockIdAsync(Guid stockId);
    }
}
