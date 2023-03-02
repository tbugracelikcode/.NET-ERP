using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Localizations.Resources.PurchasingUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.BusinessRules
{
    public class PurchasingUnsuitabilityItemManager
    {
        public async Task CodeControl(IPurchasingUnsuitabilityItemsRepository _repository, string code, IStringLocalizer<PurchasingUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPurchasingUnsuitabilityItemsRepository _repository, string code, Guid id, PurchasingUnsuitabilityItems entity, IStringLocalizer<PurchasingUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPurchasingUnsuitabilityItemsRepository _repository, Guid id)
        {
        }
    }
}
