using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Localizations.Resources.PaymentPlans.Page;

namespace TsiErp.Business.Entities.PaymentPlan.BusinessRules
{
    public class PaymentPlanManager
    {
        public async Task CodeControl(IPaymentPlansRepository _repository, string code, IStringLocalizer<PaymentPlansResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPaymentPlansRepository _repository, string code, Guid id, PaymentPlans entity, IStringLocalizer<PaymentPlansResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPaymentPlansRepository _repository, Guid id, IStringLocalizer<PaymentPlansResource> L)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositionLines.Any(x => x.PaymentPlanID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }

            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.PaymentPlanID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
