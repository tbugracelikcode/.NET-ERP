using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnitSet;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.UnitSet;

namespace TsiErp.Business.Entities.UnitSet.BusinessRules
{
    public class UnitSetManager
    {
        public async Task CodeControl(IUnitSetsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IUnitSetsRepository _repository, string code, Guid id, UnitSets entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IUnitSetsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositionLines.Any(x => x.UnitSetID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
            if (await _repository.AnyAsync(t => t.Products.Any(x => x.UnitSetID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
