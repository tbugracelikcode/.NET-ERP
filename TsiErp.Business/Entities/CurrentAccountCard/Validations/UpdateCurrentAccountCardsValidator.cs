using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;

namespace TsiErp.Business.Entities.CurrentAccountCard.Validations
{
    public class UpdateCurrentAccountCardsValidator : TsiAbstractValidatorBase<UpdateCurrentAccountCardsDto>
    {
        public UpdateCurrentAccountCardsValidator()
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

            RuleFor(x => x.TaxAdministration)
                .NotEmpty()
                .WithMessage("ValidatorTaxAdminEmpty")
                .MaximumLength(75)
                .WithMessage("ValidatorTaxAdminMaxLenght");

            RuleFor(x => x.TaxNumber)
                .NotEmpty()
                .WithMessage("ValidatorTaxNoEmpty")
                .MaximumLength(10)
                .WithMessage("ValidatorTaxNoMaxLenght");

            RuleFor(x => x.CurrencyID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrencyID");


        }
    }
}
