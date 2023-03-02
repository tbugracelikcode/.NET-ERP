using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Localizations.Resources.MaintenancePeriods.Page;

namespace TsiErp.Business.Entities.MaintenancePeriod.BusinessRules
{
    public class MaintenancePeriodManager
    {
        public async Task CodeControl(IMaintenancePeriodsRepository _repository, string code, IStringLocalizer<MaintenancePeriodsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IMaintenancePeriodsRepository _repository, string code, Guid id, MaintenancePeriods entity, IStringLocalizer<MaintenancePeriodsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IMaintenancePeriodsRepository _repository, Guid id)
        {
            
        }
    }
}
