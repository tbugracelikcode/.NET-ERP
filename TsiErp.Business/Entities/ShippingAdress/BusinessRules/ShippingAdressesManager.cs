using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShippingAdress;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Localizations.Resources.ShippingAdresses.Page;

namespace TsiErp.Business.Entities.ShippingAdress.BusinessRules
{
    public class ShippingAdressesManager
    {
        public async Task CodeControl(IShippingAdressesRepository _repository, string code, IStringLocalizer<ShippingAdressesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IShippingAdressesRepository _repository, string code, Guid id, ShippingAdresses entity, IStringLocalizer<ShippingAdressesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IShippingAdressesRepository _repository, Guid id, IStringLocalizer<ShippingAdressesResource> L)
        {
            //if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.ShippingAdressID == id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
            //if (await _repository.AnyAsync(t => t.CurrentAccountCards.ShippingAdresses.Any(x=>x.Id==id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
        }
    }
}
