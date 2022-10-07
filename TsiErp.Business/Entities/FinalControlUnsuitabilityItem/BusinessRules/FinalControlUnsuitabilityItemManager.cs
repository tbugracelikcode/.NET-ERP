using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.BusinessRules
{
    public class FinalControlUnsuitabilityItemManager
    {
        public async Task CodeControl(IFinalControlUnsuitabilityItemsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IFinalControlUnsuitabilityItemsRepository _repository, string code, Guid id, FinalControlUnsuitabilityItems entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IFinalControlUnsuitabilityItemsRepository _repository, Guid id)
        {
        }
    }
}
