using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;
using TsiErp.Localizations.Resources.ProductionOrderChangeItems.Page;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.BusinessRules
{
    public class ProductionOrderChangeItemManager
    {
        public async Task CodeControl(IProductionOrderChangeItemsRepository _repository, string code, IStringLocalizer<ProductionOrderChangeItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IProductionOrderChangeItemsRepository _repository, string code, Guid id, ProductionOrderChangeItems entity, IStringLocalizer<ProductionOrderChangeItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IProductionOrderChangeItemsRepository _repository, Guid id)
        {
        }
    }
}
