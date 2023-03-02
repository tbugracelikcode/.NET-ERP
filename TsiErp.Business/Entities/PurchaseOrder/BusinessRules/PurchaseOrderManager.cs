using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Localizations.Resources.PurchaseOrders.Page;

namespace TsiErp.Business.Entities.PurchaseOrder.BusinessRules
{
    public class PurchaseOrderManager
    {
        public async Task CodeControl(IPurchaseOrdersRepository _repository, string ficheNo, IStringLocalizer<PurchaseOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPurchaseOrdersRepository _repository, string ficheNo, Guid id, PurchaseOrders entity, IStringLocalizer<PurchaseOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPurchaseOrdersRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.PurchaseOrderLines);

                var line = entity.PurchaseOrderLines.Where(t => t.Id == lineId).FirstOrDefault();

               
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

            }
        }
    }
}
