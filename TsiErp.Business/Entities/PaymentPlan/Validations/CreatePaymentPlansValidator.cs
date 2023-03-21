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
    public class CreatePaymentPlansValidator : TsiAbstractValidatorBase<CreatePaymentPlansDto>
    {
        public CreatePaymentPlansValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("ValidatorNameEmpty")
                .MaximumLength(200)
                .WithMessage("ValidatorNameMaxLenght");

            RuleFor(x => x.Days_).GreaterThanOrEqualTo(1).WithMessage("ValidatorDay");

        }
    }
}
