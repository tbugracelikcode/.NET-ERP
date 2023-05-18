using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Localizations.Resources.PlannedMaintenances.Page;

namespace TsiErp.Business.Entities.PlannedMaintenance.BusinessRules
{
    public class PlannedMaintenanceManager
    {
        public async Task CodeControl(IPlannedMaintenancesRepository _repository, string registrationNo, IStringLocalizer<PlannedMaintenancesResource> L)
        {
            if (await _repository.AnyAsync(t => t.RegistrationNo == registrationNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPlannedMaintenancesRepository _repository, string registrationNo, Guid id, PlannedMaintenances entity, IStringLocalizer<PlannedMaintenancesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.RegistrationNo == registrationNo) && entity.RegistrationNo != registrationNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPlannedMaintenancesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            //if (lineDelete)
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id, t => t.PlannedMaintenanceLines);

            //    var line = entity.PlannedMaintenanceLines.Where(t => t.Id == lineId).FirstOrDefault();
            //}
            //else
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id);
            //}
        }
    }
}
