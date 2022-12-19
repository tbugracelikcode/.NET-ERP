using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.BusinessRules
{
    public class OperationUnsuitabilityItemManager
    {
        public async Task CodeControl(IOperationUnsuitabilityItemsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IOperationUnsuitabilityItemsRepository _repository, string code, Guid id, OperationUnsuitabilityItems entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IOperationUnsuitabilityItemsRepository _repository, Guid id)
        {
        }
    }
}
