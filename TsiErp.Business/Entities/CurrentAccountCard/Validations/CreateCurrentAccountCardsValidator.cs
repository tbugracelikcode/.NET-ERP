using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;

namespace TsiErp.Business.Entities.CurrentAccountCard.Validations
{
    public class CreateCurrentAccountCardsValidator : TsiAbstractValidatorBase<CreateCurrentAccountCardsDto>
    {
        public CreateCurrentAccountCardsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen cari kodu yazın.")
                .MaximumLength(17)
                .WithMessage("Cari kod, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen cari ünvanı yazın.")
                .MaximumLength(200)
                .WithMessage("Cari ünvan, 200 karakterden fazla olamaz.");

            RuleFor(x => x.TaxAdministration)
                .NotEmpty()
                .WithMessage("Lütfen vergi dairesini yazın.")
                .MaximumLength(75)
                .WithMessage("Vergi dairesi, 75 karakterden fazla olamaz.");

            RuleFor(x => x.TaxNumber)
                .NotEmpty()
                .WithMessage("Lütfen vergi numarasını yazın.")
                .MaximumLength(10)
                .WithMessage("Vergi numarası, 10 karakterden fazla olamaz.");


        }
    }
}
