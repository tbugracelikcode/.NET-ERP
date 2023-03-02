using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Localizations.Resources.ProductionOrders.Page;

namespace TsiErp.Business.Entities.ProductionOrder.BusinessRules
{
    public class ProductionOrderManager
    {
        public async Task CodeControl(IProductionOrdersRepository _repository, string ficheNo, IStringLocalizer<ProductionOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IProductionOrdersRepository _repository, string ficheNo, Guid id, ProductionOrders entity, IStringLocalizer<ProductionOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IProductionOrdersRepository _repository, Guid id)
        {
        }
    }
}
