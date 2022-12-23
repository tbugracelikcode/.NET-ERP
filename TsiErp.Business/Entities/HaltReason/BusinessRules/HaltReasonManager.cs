using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.Business.Entities.HaltReason.BusinessRules
{
    public class HaltReasonManager
    {
        public async Task CodeControl(IHaltReasonsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IHaltReasonsRepository _repository, string code, Guid id, HaltReasons entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IHaltReasonsRepository _repository, Guid id)
        {
        }
    }
}
