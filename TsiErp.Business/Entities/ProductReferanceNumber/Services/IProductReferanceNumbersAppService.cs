using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;
using Tsi.Core.Utilities.Results;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Services
{
    public interface IProductReferanceNumbersAppService : ICrudAppService<SelectProductReferanceNumbersDto, ListProductReferanceNumbersDto, CreateProductReferanceNumbersDto, UpdateProductReferanceNumbersDto, ListProductReferanceNumbersParameterDto>
    {
        Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId);
    }
}
