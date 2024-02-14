using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos;

namespace TsiErp.Business.Entities.ProductCost.Services
{
    public interface IProductCostsAppService : ICrudAppService<SelectProductCostsDto, ListProductCostsDto, CreateProductCostsDto, UpdateProductCostsDto, ListProductCostsParameterDto>
    {
        Task<IDataResult<IList<ListProductCostsDto>>> GetListByProductIdAsync(Guid productId);

    }
}
