using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.TechnicalDrawing;

namespace TsiErp.Business.Entities.TechnicalDrawing.BusinessRules
{
    public class TechnicalDrawingManager
    {
        public async Task CodeControl(ITechnicalDrawingsRepository _repository, string revisionNo)
        {
            if (await _repository.AnyAsync(t => t.RevisionNo == revisionNo))
            {
                throw new DuplicateCodeException("Aynı revizyon numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ITechnicalDrawingsRepository _repository, string revisionNo, Guid id, TechnicalDrawings entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.RevisionNo == revisionNo) && entity.RevisionNo != revisionNo)
            {
                throw new DuplicateCodeException("Aynı revizyon numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ITechnicalDrawingsRepository _repository, Guid id)
        {
            
        }
    }
}
