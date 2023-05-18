using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CurrentAccountCard;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Localizations.Resources.CurrentAccountCards.Page;

namespace TsiErp.Business.Entities.CurrentAccountCard.BusinessRules
{
    public class CurrentAccountCardManager
    {
        public async Task CodeControl(ICurrentAccountCardsRepository _repository, string code, IStringLocalizer<CurrentAccountCardsResource>L )
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ICurrentAccountCardsRepository _repository, string code, Guid id, CurrentAccountCards entity, IStringLocalizer<CurrentAccountCardsResource>L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ICurrentAccountCardsRepository _repository, Guid id, IStringLocalizer<CurrentAccountCardsResource>L)
        {
            //if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.CurrentAccountCardID == id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
            //if (await _repository.AnyAsync(t => t.ShippingAdresses.Any(x => x.CustomerCardID == id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
        }
    }
}
