using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;

namespace TsiErp.Business.Entities.Currency.Validations
{
    public class UpdateCurrenciesValidator : TsiAbstractValidatorBase<UpdateCurrenciesDto>
    {
        public UpdateCurrenciesValidator()
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
                .WithMessage("ValidatorNameMaxLenght"); ;
        }
    }
}
