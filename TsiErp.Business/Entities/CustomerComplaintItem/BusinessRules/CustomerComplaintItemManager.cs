using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CustomerComplaintItem;

namespace TsiErp.Business.Entities.CustomerComplaintItem.BusinessRules
{
    public class CustomerComplaintItemManager
    {
        public async Task CodeControl(ICustomerComplaintItemsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ICustomerComplaintItemsRepository _repository, string code, Guid id, CustomerComplaintItems entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ICustomerComplaintItemsRepository _repository, Guid id)
        {
        }
    }
}
