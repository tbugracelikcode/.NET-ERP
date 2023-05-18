using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductGroup;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Localizations.Resources.ProductGroups.Page;

namespace TsiErp.Business.Entities.ProductGroup.BusinessRules
{
    public class ProductGroupManager
    {
        public async Task CodeControl(IProductGroupsRepository _repository, string code, IStringLocalizer<ProductGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IProductGroupsRepository _repository, string code, Guid id, ProductGroups entity, IStringLocalizer<ProductGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IProductGroupsRepository _repository, Guid id, IStringLocalizer<ProductGroupsResource> L)
        {
            //if (await _repository.AnyAsync(t => t.Products.Any(x => x.ProductGrpID == id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
        }
    }
}
