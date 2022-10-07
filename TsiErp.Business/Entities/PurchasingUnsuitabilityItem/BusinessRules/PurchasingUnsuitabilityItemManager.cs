using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.BusinessRules
{
    public class PurchasingUnsuitabilityItemManager
    {
        public async Task CodeControl(IPurchasingUnsuitabilityItemsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IPurchasingUnsuitabilityItemsRepository _repository, string code, Guid id, PurchasingUnsuitabilityItems entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IPurchasingUnsuitabilityItemsRepository _repository, Guid id)
        {
        }
    }
}
