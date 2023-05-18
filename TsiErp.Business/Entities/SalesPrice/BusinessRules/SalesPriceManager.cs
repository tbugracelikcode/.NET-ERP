using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPrice;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Localizations.Resources.SalesPrices.Page;

namespace TsiErp.Business.Entities.SalesPrice.BusinessRules
{
    public class SalesPriceManager
    {
        public async Task CodeControl(ISalesPricesRepository _repository, string code, IStringLocalizer<SalesPricesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ISalesPricesRepository _repository, string code, Guid id, SalesPrices entity, IStringLocalizer<SalesPricesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ISalesPricesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            //if (lineDelete)
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id, t => t.SalesPriceLines);

            //    var line = entity.SalesPriceLines.Where(t => t.Id == lineId).FirstOrDefault();
            //}
            //else
            //{
            //    var entity = await _repository.GetAsync(t => t.Id == id);
            //}
        }
    }
}
