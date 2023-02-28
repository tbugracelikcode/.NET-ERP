using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Branch.BusinessRules
{
    public class BranchesManager 
    {
        public async Task CodeControl(IBranchesRepository _repository, string code, IStringLocalizer<BranchesResource>L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IBranchesRepository _repository, string code,Guid id, Branches entity, IStringLocalizer<BranchesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code==code) && entity.Code!=code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IBranchesRepository _repository, Guid id, IStringLocalizer<BranchesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Periods.Any(x => x.BranchID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }

            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.BranchID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
