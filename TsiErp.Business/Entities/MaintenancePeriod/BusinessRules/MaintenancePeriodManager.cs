using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenancePeriod;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.MaintenancePeriod;

namespace TsiErp.Business.Entities.MaintenancePeriod.BusinessRules
{
    public class MaintenancePeriodManager
    {
        public async Task CodeControl(IMaintenancePeriodsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IMaintenancePeriodsRepository _repository, string code, Guid id, MaintenancePeriods entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IMaintenancePeriodsRepository _repository, Guid id)
        {
            
        }
    }
}
