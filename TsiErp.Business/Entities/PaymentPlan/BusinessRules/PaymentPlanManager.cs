using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.PaymentPlan;

namespace TsiErp.Business.Entities.PaymentPlan.BusinessRules
{
    public class PaymentPlanManager
    {
        public async Task CodeControl(IPaymentPlansRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IPaymentPlansRepository _repository, string code, Guid id, PaymentPlans entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IPaymentPlansRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositionLines.Any(x => x.PaymentPlanID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }

            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.PaymentPlanID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
