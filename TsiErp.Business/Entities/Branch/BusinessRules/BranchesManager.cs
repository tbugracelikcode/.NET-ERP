using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;

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
    }
}
