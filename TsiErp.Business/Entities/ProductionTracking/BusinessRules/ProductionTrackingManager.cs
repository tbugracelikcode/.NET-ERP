using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Localizations.Resources.ProductionTrackings.Page;

namespace TsiErp.Business.Entities.ProductionTracking.BusinessRules
{
    public class ProductionTrackingManager
    {
        public async Task CodeControl(IProductionTrackingsRepository _repository, string code, IStringLocalizer<ProductionTrackingsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IProductionTrackingsRepository _repository, string code, Guid id, ProductionTrackings entity, IStringLocalizer<ProductionTrackingsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IProductionTrackingsRepository _repository, Guid id)
        {
        }
    }
}
