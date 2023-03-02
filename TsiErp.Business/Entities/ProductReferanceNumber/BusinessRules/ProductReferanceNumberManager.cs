using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.Localizations.Resources.ProductReferanceNumbers.Page;

namespace TsiErp.Business.Entities.ProductReferanceNumber.BusinessRules
{
    public class ProductReferanceNumberManager
    {
        public async Task CodeControl(IProductReferanceNumbersRepository _repository, string referanceNo, IStringLocalizer<ProductReferanceNumbersResource> L)
        {
            if (await _repository.AnyAsync(t => t.ReferanceNo == referanceNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IProductReferanceNumbersRepository _repository, string referanceNo, Guid id, ProductReferanceNumbers entity, IStringLocalizer<ProductReferanceNumbersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.ReferanceNo == referanceNo) && entity.ReferanceNo != referanceNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IProductReferanceNumbersRepository _repository, Guid id)
        {
            
        }
    }
}
