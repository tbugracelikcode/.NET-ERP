using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;

namespace TsiErp.Business.Entities.CurrentAccountCard.Validations
{
    public class CreateCurrentAccountCardsValidator : TsiAbstractValidatorBase<CreateCurrentAccountCardsDto>
    {
        public CreateCurrentAccountCardsValidator()
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

            

            RuleFor(x => x.CurrencyID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrencyID");

            When(x => x.SoleProprietorship, () =>
            {
                RuleFor(x => x.TaxAdministration)
                .NotEmpty()
                .WithMessage("ValidatorTaxAdminEmpty")
                .MaximumLength(75)
                .WithMessage("ValidatorTaxAdminMaxLenght");

                RuleFor(x => x.IDnumber)
               .NotEmpty()
               .WithMessage("ValidatorIDnumberEmpty");
            }
          );


            When(x => !x.SoleProprietorship, () =>
            {
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

            }
            );


        }
    }
}
