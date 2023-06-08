using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Services
{
    public interface IProductReferanceNumbersAppService : ICrudAppService<SelectProductReferanceNumbersDto, ListProductReferanceNumbersDto, CreateProductReferanceNumbersDto, UpdateProductReferanceNumbersDto, ListProductReferanceNumbersParameterDto>
    {
        Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId);
    }
}
