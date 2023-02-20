using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Product.Dtos;

namespace TsiErp.Business.Entities.Product.Services
{
    public interface IProductsAppService : ICrudAppService<SelectProductsDto, ListProductsDto, CreateProductsDto, UpdateProductsDto, ListProductsParameterDto>
    {
    }
}
