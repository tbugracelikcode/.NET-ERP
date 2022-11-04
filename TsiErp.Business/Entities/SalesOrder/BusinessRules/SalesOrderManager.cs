using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.SalesOrder.BusinessRules
{
    public class SalesOrderManager
    {
        public async Task CodeControl(ISalesOrdersRepository _repository, string ficheNo)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ISalesOrdersRepository _repository, string ficheNo, Guid id, SalesOrders entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ISalesOrdersRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.SalesOrderLines);

                var line = entity.SalesOrderLines.Where(t => t.Id == lineId).FirstOrDefault();

                if (line != null)
                {
                    if (line.SalesOrderLineStateEnum == SalesOrderLineStateEnum.Onaylandı)
                    {
                        throw new Exception("Onaylanan satış teklifi satırları silinemez.");
                    }
                }
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

                if (entity.SalesOrderState == SalesOrderStateEnum.Onaylandı)
                {
                    throw new Exception("Onaylanan satış siparişleri silinemez.");
                }

                if (entity.SalesOrderState == SalesOrderStateEnum.KismiUretimeVerildi)
                {
                    throw new Exception("Kısmi üretime verilen satış siparişleri silinemez.");
                }

                if (entity.SalesOrderState == SalesOrderStateEnum.UretimeVerildi)
                {
                    throw new Exception("Üretime verilen satış siparişleri silinemez.");
                }
            }
        }
    }
}
