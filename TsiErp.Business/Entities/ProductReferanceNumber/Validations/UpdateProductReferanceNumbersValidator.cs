using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Validations
{
    public class UpdateProductReferanceNumbersValidator : TsiAbstractValidatorBase<UpdateProductReferanceNumbersDto>
    {
        public UpdateProductReferanceNumbersValidator()
        {
            RuleFor(x => x.ReferanceNo)
                 .NotEmpty()
                 .WithMessage("ValidatorCodeEmpty")
                 .MaximumLength(17)
                 .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.ProductID)
                 .Must(x => x.HasValue && x.Value != Guid.Empty)
                .WithMessage("ValidatorProductID");

            RuleFor(x => x.CurrentAccountCardID)
               .Must(x => x.HasValue && x.Value != Guid.Empty)
              .WithMessage("ValidatorCurrentAccountID");

        }
    }
}
