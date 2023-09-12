using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.Business.Entities.Product.Services
{
    public interface IProductsAppService : ICrudAppService<SelectProductsDto, ListProductsDto, CreateProductsDto, UpdateProductsDto, ListProductsParameterDto>
    {
        Task<IDataResult<SelectGrandTotalStockMovementsDto>> GetStockAmountAsync(Guid productid);
    }
}
