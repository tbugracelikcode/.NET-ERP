using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;

namespace TsiErp.Business.Entities.Product.Services
{
    public interface IProductsAppService : ICrudAppService<SelectProductsDto, ListProductsDto, CreateProductsDto, UpdateProductsDto, ListProductsParameterDto>
    {
        Task<IResult> DeleteProductRelatedPropertiesAsync(Guid productId,Guid productGroupId);

        Task<IDataResult<IList<ListProductRelatedProductPropertiesDto>>> GetProductRelatedPropertiesAsync(Guid productId,Guid productGroupId);

        Task<IDataResult<SelectProductsDto>> GetbyProductIDAsync(Guid productId);
    }
}
