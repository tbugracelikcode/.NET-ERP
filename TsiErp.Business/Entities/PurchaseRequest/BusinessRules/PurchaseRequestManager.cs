using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequest;

namespace TsiErp.Business.Entities.PurchaseRequest.BusinessRules
{
    public class PurchaseRequestManager
    {
        public async Task CodeControl(IPurchaseRequestsRepository _repository, string ficheNo)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IPurchaseRequestsRepository _repository, string ficheNo, Guid id, PurchaseRequests entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IPurchaseRequestsRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.PurchaseRequestLines);

                var line = entity.PurchaseRequestLines.Where(t => t.Id == lineId).FirstOrDefault();

                if (line != null)
                {
                    if (line.PurchaseRequestLineState == TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı)
                    {
                        throw new Exception("Onaylanan satın alma talep satırları silinemez.");
                    }
                }
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

                if (entity.PurchaseRequestState == TsiErp.Entities.Enums.PurchaseRequestStateEnum.Onaylandı)
                {
                    throw new Exception("Onaylanan satın alma talepleri silinemez.");
                }

                if (entity.PurchaseRequestState == TsiErp.Entities.Enums.PurchaseRequestStateEnum.SatinAlma)
                {
                    throw new Exception("Satın almaya dönüşen satın alma talepleri silinemez.");
                }

                if (entity.PurchaseRequestState == TsiErp.Entities.Enums.PurchaseRequestStateEnum.KismiSatinAlma)
                {
                    throw new Exception("Satın almaya dönüşen satın alma talepleri silinemez.");
                }
            }
        }
    }
}
