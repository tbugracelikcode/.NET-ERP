using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockColumn.Dtos;

namespace TsiErp.Business.Entities.StockColumn.Services
{
    public interface IStockColumnsAppService : ICrudAppService<SelectStockColumnsDto, ListStockColumnsDto, CreateStockColumnsDto, UpdateStockColumnsDto, ListStockColumnsParameterDto>
    {
    }
}
