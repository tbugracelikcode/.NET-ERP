using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;

namespace TsiErp.Business.Entities.PaymentPlan.Validations
{
    public class UpdatePaymentPlansValidator : TsiAbstractValidatorBase<UpdatePaymentPlansDto>
    {
        public UpdatePaymentPlansValidator()
        {


            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen ödeme planı kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Ödeme planı kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen ödeme planı adını yazın.")
                .MaximumLength(200)
                .WithMessage("Ödeme planı adı 200 karakterden fazla olamaz.");

            RuleFor(x => x.Days_).GreaterThanOrEqualTo(1).WithMessage("Ödeme planı vade günü, 0'dan büyük olmalıdır.");


        }
    }
}
