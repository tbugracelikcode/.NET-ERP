using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder;

namespace TsiErp.Business.Entities.ProductionOrder.BusinessRules
{
    public class ProductionOrderManager
    {
        public async Task CodeControl(IProductionOrdersRepository _repository, string ficheNo)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IProductionOrdersRepository _repository, string ficheNo, Guid id, ProductionOrders entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IProductionOrdersRepository _repository, Guid id)
        {
        }
    }
}
