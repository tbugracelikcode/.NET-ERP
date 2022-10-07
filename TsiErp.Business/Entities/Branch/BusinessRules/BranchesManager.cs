using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;

namespace TsiErp.Business.Entities.Branch.BusinessRules
{
    public class BranchesManager
    {
        public async Task CodeControl(IBranchesRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IBranchesRepository _repository, string code,Guid id, Branches entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code==code) && entity.Code!=code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IBranchesRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.Periods.Any(x => x.BranchID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }

            if (await _repository.AnyAsync(t => t.SalesPropositionLines.Any(x => x.BranchID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }

            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.BranchID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
