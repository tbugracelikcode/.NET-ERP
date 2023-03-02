using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Localizations.Resources.OperationUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.BusinessRules
{
    public class OperationUnsuitabilityItemManager
    {
        public async Task CodeControl(IOperationUnsuitabilityItemsRepository _repository, string code, IStringLocalizer<OperationUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IOperationUnsuitabilityItemsRepository _repository, string code, Guid id, OperationUnsuitabilityItems entity, IStringLocalizer<OperationUnsuitabilityItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IOperationUnsuitabilityItemsRepository _repository, Guid id)
        {
        }
    }
}
