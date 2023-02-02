using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenance;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.UnplannedMaintenance;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.UnplannedMaintenance.BusinessRules
{
    public class UnplannedMaintenanceManager
    {
        public async Task CodeControl(IUnplannedMaintenancesRepository _repository, string registrationNo)
        {
            if (await _repository.AnyAsync(t => t.RegistrationNo == registrationNo))
            {
                throw new DuplicateCodeException("Aynı kayıt numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IUnplannedMaintenancesRepository _repository, string registrationNo, Guid id, UnplannedMaintenances entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.RegistrationNo == registrationNo) && entity.RegistrationNo != registrationNo)
            {
                throw new DuplicateCodeException("Aynı kayıt numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IUnplannedMaintenancesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.UnplannedMaintenanceLines);

                var line = entity.UnplannedMaintenanceLines.Where(t => t.Id == lineId).FirstOrDefault();
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);
            }
        }
    }
}
