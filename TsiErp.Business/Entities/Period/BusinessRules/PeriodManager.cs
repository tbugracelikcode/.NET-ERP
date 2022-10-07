using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.Business.Entities.Period.BusinessRules
{
    public class PeriodManager
    {
        public async Task CodeControl(IPeriodsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IPeriodsRepository _repository, string code, Guid id, Periods entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IPeriodsRepository _repository, Guid id)
        {
        }
    }
}
