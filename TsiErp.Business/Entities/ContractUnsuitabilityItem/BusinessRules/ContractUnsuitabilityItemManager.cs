using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.BusinessRules
{
    public class ContractUnsuitabilityItemManager
    {
        public async Task CodeControl(IContractUnsuitabilityItemsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IContractUnsuitabilityItemsRepository _repository, string code, Guid id, ContractUnsuitabilityItems entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IContractUnsuitabilityItemsRepository _repository, Guid id)
        {
            
        }
    }
}
