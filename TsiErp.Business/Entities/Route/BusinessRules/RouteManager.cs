using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route;
using TsiErp.Entities.Entities.Route;
using TsiErp.Localizations.Resources.Routes.Page;

namespace TsiErp.Business.Entities.Route.BusinessRules
{
    public class RouteManager
    {
        public async Task CodeControl(IRoutesRepository _repository, string code, IStringLocalizer<RoutesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IRoutesRepository _repository, string code, Guid id, Routes entity, IStringLocalizer<RoutesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IRoutesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            //if (lineDelete)
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id, t => t.RouteLines);

            //    var line = entity.RouteLines.Where(t => t.Id == lineId).FirstOrDefault();

            //}
            //else
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id);

            //}
        }
    }
}
