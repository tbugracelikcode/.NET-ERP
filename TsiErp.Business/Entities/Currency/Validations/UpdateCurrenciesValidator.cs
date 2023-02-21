using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Currency.Dtos;

namespace TsiErp.Business.Entities.Currency.Validations
{
    public class UpdateCurrenciesValidator : TsiAbstractValidatorBase<UpdateCurrenciesDto>
    {
        public UpdateCurrenciesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen para birimi kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Para birimi kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen para birimi  açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Para birimi  açıklaması 200 karakterden fazla olamaz."); ;

        }
    }
}
