using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Localizations.Resources.HaltReasons.Page;

namespace TsiErp.Business.Entities.HaltReason.BusinessRules
{
    public class HaltReasonManager
    {
        public async Task CodeControl(IHaltReasonsRepository _repository, string code, IStringLocalizer<HaltReasonsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IHaltReasonsRepository _repository, string code, Guid id, HaltReasons entity, IStringLocalizer<HaltReasonsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IHaltReasonsRepository _repository, Guid id)
        {
        }
    }
}
