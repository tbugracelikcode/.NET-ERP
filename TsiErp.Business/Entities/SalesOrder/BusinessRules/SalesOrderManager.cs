using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Enums;
using TsiErp.Localizations.Resources.SalesOrders.Page;

namespace TsiErp.Business.Entities.SalesOrder.BusinessRules
{
    public class SalesOrderManager
    {
        public async Task CodeControl(ISalesOrdersRepository _repository, string ficheNo, IStringLocalizer<SalesOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ISalesOrdersRepository _repository, string ficheNo, Guid id, SalesOrders entity, IStringLocalizer<SalesOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ISalesOrdersRepository _repository, Guid id, Guid lineId, bool lineDelete, IStringLocalizer<SalesOrdersResource> L)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.SalesOrderLines);

                var line = entity.SalesOrderLines.Where(t => t.Id == lineId).FirstOrDefault();

                if (line != null)
                {
                    if (line.SalesOrderLineStateEnum == SalesOrderLineStateEnum.Onaylandı)
                    {
                        throw new Exception(L["DeleteSalesOrderLineManager"]);
                    }
                }
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

                if (entity.SalesOrderState == SalesOrderStateEnum.Onaylandı)
                {
                    throw new Exception(L["DeleteSalesOrderManager"]);
                }

                if (entity.SalesOrderState == SalesOrderStateEnum.KismiUretimeVerildi)
                {
                    throw new Exception(L["DeleteSalesOrderConvertPartialProductionManager"]);
                }

                if (entity.SalesOrderState == SalesOrderStateEnum.UretimeVerildi)
                {
                    throw new Exception(L["DeleteSalesOrderConvertProductionManager"]);
                }
            }
        }
    }
}
