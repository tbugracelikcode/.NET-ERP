using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Services
{
    public interface IProductReferanceNumbersAppService : ICrudAppService<SelectProductReferanceNumbersDto, ListProductReferanceNumbersDto, CreateProductReferanceNumbersDto, UpdateProductReferanceNumbersDto, ListProductReferanceNumbersParameterDto>
    {
        Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId);
    }
}
