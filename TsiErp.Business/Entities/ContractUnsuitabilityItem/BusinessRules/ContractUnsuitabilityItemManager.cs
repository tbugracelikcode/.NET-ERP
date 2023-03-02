using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Localizations.Resources.ContractUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.BusinessRules
{
    public class ContractUnsuitabilityItemManager
    {
        public async Task CodeControl(IContractUnsuitabilityItemsRepository _repository, string code, IStringLocalizer<ContractUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IContractUnsuitabilityItemsRepository _repository, string code, Guid id, ContractUnsuitabilityItems entity, IStringLocalizer<ContractUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IContractUnsuitabilityItemsRepository _repository, Guid id)
        {
            
        }
    }
}
