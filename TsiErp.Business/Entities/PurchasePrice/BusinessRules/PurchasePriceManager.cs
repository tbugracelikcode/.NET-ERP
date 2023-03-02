using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Localizations.Resources.PurchasePrices.Page;

namespace TsiErp.Business.Entities.PurchasePrice.BusinessRules
{
    public class PurchasePriceManager
    {
        public async Task CodeControl(IPurchasePricesRepository _repository, string code, IStringLocalizer<PurchasePricesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPurchasePricesRepository _repository, string code, Guid id, PurchasePrices entity, IStringLocalizer<PurchasePricesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPurchasePricesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.PurchasePriceLines);

                var line = entity.PurchasePriceLines.Where(t => t.Id == lineId).FirstOrDefault();
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);
            }
        }
    }
}
