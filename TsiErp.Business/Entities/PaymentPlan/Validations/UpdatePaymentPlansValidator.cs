using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;

namespace TsiErp.Business.Entities.PaymentPlan.Validations
{
    public class UpdatePaymentPlansValidator : TsiAbstractValidatorBase<UpdatePaymentPlansDto>
    {
        public UpdatePaymentPlansValidator()
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
