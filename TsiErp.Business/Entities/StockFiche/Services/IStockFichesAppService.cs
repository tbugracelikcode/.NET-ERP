using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockFiche.Dtos;

namespace TsiErp.Business.Entities.StockFiche.Services
{
    public interface IStockFichesAppService : ICrudAppService<SelectStockFichesDto, ListStockFichesDto, CreateStockFichesDto, UpdateStockFichesDto, ListStockFichesParameterDto>
    {
    }
}
