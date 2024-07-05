using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    public interface IProductionOrdersAppService : ICrudAppService<SelectProductionOrdersDto, ListProductionOrdersDto, CreateProductionOrdersDto, UpdateProductionOrdersDto, ListProductionOrdersParameterDto>
    {
        Task<IDataResult<SelectProductionOrdersDto>> ConverttoProductionOrder(CreateProductionOrdersDto input);

        Task<IDataResult<IList<ListProductionOrdersDto>>> GetNotCanceledListAsync(ListProductionOrdersParameterDto input);

        Task<IDataResult<IList<ListProductionOrdersDto>>> GetCanceledListAsync(ListProductionOrdersParameterDto input);
    }
}
