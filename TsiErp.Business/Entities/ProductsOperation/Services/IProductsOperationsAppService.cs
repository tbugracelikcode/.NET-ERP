using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;

namespace TsiErp.Business.Entities.ProductsOperation.Services
{
    public interface IProductsOperationsAppService : ICrudAppService<SelectProductsOperationsDto, ListProductsOperationsDto, CreateProductsOperationsDto, UpdateProductsOperationsDto, ListProductsOperationsParameterDto>
    {
    }
}
