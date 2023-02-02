using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenance;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.PlannedMaintenance.BusinessRules
{
    public class PlannedMaintenanceManager
    {
        public async Task CodeControl(IPlannedMaintenancesRepository _repository, string registrationNo)
        {
            if (await _repository.AnyAsync(t => t.RegistrationNo == registrationNo))
            {
                throw new DuplicateCodeException("Aynı kayıt numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IPlannedMaintenancesRepository _repository, string registrationNo, Guid id, PlannedMaintenances entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.RegistrationNo == registrationNo) && entity.RegistrationNo != registrationNo)
            {
                throw new DuplicateCodeException("Aynı kayıt numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IPlannedMaintenancesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.PlannedMaintenanceLines);

                var line = entity.PlannedMaintenanceLines.Where(t => t.Id == lineId).FirstOrDefault();
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);
            }
        }
    }
}
