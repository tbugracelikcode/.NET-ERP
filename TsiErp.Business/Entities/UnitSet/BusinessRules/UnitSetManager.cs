using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnitSet;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Localizations.Resources.UnitSets.Page;

namespace TsiErp.Business.Entities.UnitSet.BusinessRules
{
    public class UnitSetManager
    {
        public async Task CodeControl(IUnitSetsRepository _repository, string code, IStringLocalizer<UnitSetsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IUnitSetsRepository _repository, string code, Guid id, UnitSets entity, IStringLocalizer<UnitSetsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IUnitSetsRepository _repository, Guid id, IStringLocalizer<UnitSetsResource> L)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositionLines.Any(x => x.UnitSetID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            if (await _repository.AnyAsync(t => t.Products.Any(x => x.UnitSetID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
