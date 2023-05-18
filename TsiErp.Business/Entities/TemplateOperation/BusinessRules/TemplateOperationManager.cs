using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Localizations.Resources.TemplateOperations.Page;

namespace TsiErp.Business.Entities.TemplateOperation.BusinessRules
{
    public class TemplateOperationManager
    {
        public async Task CodeControl(ITemplateOperationsRepository _repository, string code, IStringLocalizer<TemplateOperationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ITemplateOperationsRepository _repository, string code, Guid id, TemplateOperations entity, IStringLocalizer<TemplateOperationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ITemplateOperationsRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            //if (lineDelete)
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id, t => t.TemplateOperationLines);

            //    var line = entity.TemplateOperationLines.Where(t => t.Id == lineId).FirstOrDefault();

            //}
            //else
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id);

            //}
        }
    }
}

