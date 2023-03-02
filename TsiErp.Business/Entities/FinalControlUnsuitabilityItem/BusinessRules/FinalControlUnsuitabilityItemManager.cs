using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;
using TsiErp.Localizations.Resources.FinalControlUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.BusinessRules
{
    public class FinalControlUnsuitabilityItemManager
    {
        public async Task CodeControl(IFinalControlUnsuitabilityItemsRepository _repository, string code, IStringLocalizer<FinalControlUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IFinalControlUnsuitabilityItemsRepository _repository, string code, Guid id, FinalControlUnsuitabilityItems entity, IStringLocalizer<FinalControlUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IFinalControlUnsuitabilityItemsRepository _repository, Guid id)
        {
        }
    }
}
