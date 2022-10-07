using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.SalesProposition.BusinessRules
{
    public class SalesPropositionManager
    {
        public async Task CodeControl(ISalesPropositionsRepository _repository, string ficheNo)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheNo))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ISalesPropositionsRepository _repository, string ficheNo, Guid id, SalesPropositions entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheNo) && entity.FicheNo != ficheNo)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ISalesPropositionsRepository _repository, Guid id)
        {
            var entity = await _repository.GetAsync(t=>t.Id==id, t=>t.SalesPropositionLines);

            if (entity.SalesPropositionState==SalesPropositionStateEnum.Onaylandı)
            {
                throw new Exception("Onaylanan satış teklifleri silinemez.");
            }

            if (entity.SalesPropositionState==SalesPropositionStateEnum.Siparis)
            {
                throw new Exception("Siparişe dönüşen satış teklifleri silinemez.");
            }

            if (entity.SalesPropositionState==SalesPropositionStateEnum.KismiSiparis)
            {
                throw new Exception("Siparişe dönüşen satış teklifleri silinemez.");
            }
        }
    }
}
