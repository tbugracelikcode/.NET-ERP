using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.ProductReferanceNumber;

namespace TsiErp.Business.Entities.ProductReferanceNumber.BusinessRules
{
    public class ProductReferanceNumberManager
    {
        public async Task CodeControl(IProductReferanceNumbersRepository _repository, string referanceNo)
        {
            if (await _repository.AnyAsync(t => t.ReferanceNo == referanceNo))
            {
                throw new DuplicateCodeException("Aynı referans numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IProductReferanceNumbersRepository _repository, string referanceNo, Guid id, ProductReferanceNumbers entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.ReferanceNo == referanceNo) && entity.ReferanceNo != referanceNo)
            {
                throw new DuplicateCodeException("Aynı referans numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IProductReferanceNumbersRepository _repository, Guid id)
        {
            
        }
    }
}
