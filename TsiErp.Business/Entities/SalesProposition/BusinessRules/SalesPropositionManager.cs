using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Enums;
using TsiErp.Localizations.Resources.SalesPropositions.Page;

namespace TsiErp.Business.Entities.SalesProposition.BusinessRules
{
    public class SalesPropositionManager
    {
        public async Task CodeControl(ISalesPropositionsRepository _repository, string ficheNo, IStringLocalizer<SalesPropositionsResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ISalesPropositionsRepository _repository, string ficheNo, Guid id, SalesPropositions entity, IStringLocalizer<SalesPropositionsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ISalesPropositionsRepository _repository, Guid id,  Guid lineId,bool lineDelete, IStringLocalizer<SalesPropositionsResource> L)
        {
            //if (lineDelete)
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id, t => t.SalesPropositionLines);

            //    var line = entity.SalesPropositionLines.Where(t => t.Id == lineId).FirstOrDefault();

            //    if(line!=null)
            //    {
            //        if (line.SalesPropositionLineState == SalesPropositionLineStateEnum.Onaylandı)
            //        {
            //            throw new Exception(L["DeleteSalesPropositionLineManager"]);
            //        }
            //    }
            //}
            //else
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id);

            //    if (entity.SalesPropositionState == SalesPropositionStateEnum.Onaylandı)
            //    {
            //        throw new Exception(L["DeleteSalesPropositionManager"]);
            //    }

            //    if (entity.SalesPropositionState == SalesPropositionStateEnum.Siparis)
            //    {
            //        throw new Exception(L["DeleteSalesPropositionConvertManager"]);
            //    }

            //    if (entity.SalesPropositionState == SalesPropositionStateEnum.KismiSiparis)
            //    {
            //        throw new Exception(L["DeleteSalesPropositionConvertManager"]);
            //    }
            //}
        }
    }
}
