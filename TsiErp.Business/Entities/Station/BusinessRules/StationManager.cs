using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.Entities.Entities.Station;
using TsiErp.Localizations.Resources.Stations.Page;

namespace TsiErp.Business.Entities.Station.BusinessRules
{
    public class StationManager
    {
        public async Task CodeControl(IStationsRepository _repository, string code, IStringLocalizer<StationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IStationsRepository _repository, string code, Guid id, Stations entity, IStringLocalizer<StationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IStationsRepository _repository, Guid id, IStringLocalizer<StationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.TemplateOperationLines.Any(x => x.StationID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
