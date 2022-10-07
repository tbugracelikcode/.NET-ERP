using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.BusinessRules
{
    public class ProductionOrderChangeItemManager
    {
        public async Task CodeControl(IProductionOrderChangeItemsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IProductionOrderChangeItemsRepository _repository, string code, Guid id, ProductionOrderChangeItems entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IProductionOrderChangeItemsRepository _repository, Guid id)
        {
        }
    }
}
