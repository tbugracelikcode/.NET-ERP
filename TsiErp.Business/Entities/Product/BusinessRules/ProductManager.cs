using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Product;
using TsiErp.Entities.Entities.Product;
using TsiErp.Localizations.Resources.Products.Page;

namespace TsiErp.Business.Entities.Product.BusinessRules
{
    public class ProductManager
    {
        public async Task CodeControl(IProductsRepository _repository, string code, IStringLocalizer<ProductsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IProductsRepository _repository, string code, Guid id, Products entity, IStringLocalizer<ProductsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IProductsRepository _repository, Guid id)
        {
            
        }
    }
}
