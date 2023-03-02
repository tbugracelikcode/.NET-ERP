using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using TsiErp.Entities.Entities.Period;
using TsiErp.Localizations.Resources.Periods.Page;

namespace TsiErp.Business.Entities.Period.BusinessRules
{
    public class PeriodManager
    {
        public async Task CodeControl(IPeriodsRepository _repository, string code, IStringLocalizer<PeriodsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPeriodsRepository _repository, string code, Guid id, Periods entity, IStringLocalizer<PeriodsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPeriodsRepository _repository, Guid id)
        {
        }
    }
}
