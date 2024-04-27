using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;

namespace TsiErp.Business.Entities.ProductProperty.Services
{
    public interface IProductPropertiesAppService : ICrudAppService<SelectProductPropertiesDto, ListProductPropertiesDto, CreateProductPropertiesDto, UpdateProductPropertiesDto, ListProductPropertiesParameterDto>
    {
        Task<IDataResult<IList<ListProductPropertiesDto>>> GetListByProductGroupAsync(Guid ProductGroupID);
    }
}
