using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Localizations.Resources.PurchaseRequests.Page;

namespace TsiErp.Business.Entities.PurchaseRequest.BusinessRules
{
    public class PurchaseRequestManager
    {
        public async Task CodeControl(IPurchaseRequestsRepository _repository, string ficheNo, IStringLocalizer<PurchaseRequestsResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPurchaseRequestsRepository _repository, string ficheNo, Guid id, PurchaseRequests entity, IStringLocalizer<PurchaseRequestsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPurchaseRequestsRepository _repository, Guid id, Guid lineId, bool lineDelete, IStringLocalizer<PurchaseRequestsResource> L)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.PurchaseRequestLines);

                var line = entity.PurchaseRequestLines.Where(t => t.Id == lineId).FirstOrDefault();

                if (line != null)
                {
                    if (line.PurchaseRequestLineState == TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı)
                    {
                        throw new Exception(L["DeletePurchaseRequestLineManager"]);
                    }
                }
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

                if (entity.PurchaseRequestState == TsiErp.Entities.Enums.PurchaseRequestStateEnum.Onaylandı)
                {
                    throw new Exception(L["DeletePurchaseRequestManager"]);
                }

                if (entity.PurchaseRequestState == TsiErp.Entities.Enums.PurchaseRequestStateEnum.SatinAlma)
                {
                    throw new Exception(L["DeletePurchaseRequestConvertManager"]);
                }

                if (entity.PurchaseRequestState == TsiErp.Entities.Enums.PurchaseRequestStateEnum.KismiSatinAlma)
                {
                    throw new Exception(L["DeletePurchaseRequestConvertManager"]);
                }
            }
        }
    }
}
