using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Branch.BusinessRules
{
    public class BranchesManager 
    {

        //public async Task UpdateControl(List<Branches> branches, string code,Guid id, Branches entity, IStringLocalizer<BranchesResource> L)
        //{
        //    if (branches.Any(t => t.Id != id && t.Code==code) && entity.Code!=code)
        //    {
        //        throw new DuplicateCodeException(L["UpdateControlManager"]);
        //    }
        //}
    }
}
