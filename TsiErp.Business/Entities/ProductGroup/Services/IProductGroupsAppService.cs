using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    public interface IProductGroupsAppService : ICrudAppService<SelectProductGroupsDto, ListProductGroupsDto, CreateProductGroupsDto, UpdateProductGroupsDto, ListProductGroupsParameterDto>
    {
    }
}
