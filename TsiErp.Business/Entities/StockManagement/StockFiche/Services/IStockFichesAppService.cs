using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;

namespace TsiErp.Business.Entities.StockFiche.Services
{
    public interface IStockFichesAppService : ICrudAppService<SelectStockFichesDto, ListStockFichesDto, CreateStockFichesDto, UpdateStockFichesDto, ListStockFichesParameterDto>
    {
        Task<List<SelectStockFicheLinesDto>> GetInputList(Guid productId, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<SelectStockFicheLinesDto>> GetOutputList(Guid productId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
