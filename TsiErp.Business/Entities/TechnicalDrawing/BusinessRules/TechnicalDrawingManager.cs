using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Localizations.Resources.TechnicalDrawings.Page;

namespace TsiErp.Business.Entities.TechnicalDrawing.BusinessRules
{
    public class TechnicalDrawingManager
    {
        public async Task CodeControl(ITechnicalDrawingsRepository _repository, string revisionNo, IStringLocalizer<TechnicalDrawingsResource> L)
        {
            if (await _repository.AnyAsync(t => t.RevisionNo == revisionNo))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ITechnicalDrawingsRepository _repository, string revisionNo, Guid id, TechnicalDrawings entity, IStringLocalizer<TechnicalDrawingsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.RevisionNo == revisionNo) && entity.RevisionNo != revisionNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ITechnicalDrawingsRepository _repository, Guid id)
        {
            
        }
    }
}
