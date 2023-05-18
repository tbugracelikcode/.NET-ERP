using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationGroup;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Localizations.Resources.StationGroups.Page;

namespace TsiErp.Business.Entities.StationGroup.BusinessRules
{
    public class StationGroupManager
    {
        public async Task CodeControl(IStationGroupsRepository _repository, string code, IStringLocalizer<StationGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IStationGroupsRepository _repository, string code, Guid id, StationGroups entity, IStringLocalizer<StationGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IStationGroupsRepository _repository, Guid id, IStringLocalizer<StationGroupsResource> L)
        {
            //if (await _repository.AnyAsync(t => t.Stations.Any(x => x.GroupID == id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
        }
    }
}
