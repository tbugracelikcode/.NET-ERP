using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductGroup.Dtos;

namespace TsiErp.Business.Entities.ProductGroup.Validations
{
    public class CreateProductGroupsValidator : TsiAbstractValidatorBase<CreateProductGroupsDto>
    {
        public CreateProductGroupsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen kodu yazın.")
                .MaximumLength(17)
                .WithMessage("Kod, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen açıklama yazın.")
                .MaximumLength(200)
                .WithMessage("Açıklama, 200 karakterden fazla olamaz."); ;

        }
    }
}
