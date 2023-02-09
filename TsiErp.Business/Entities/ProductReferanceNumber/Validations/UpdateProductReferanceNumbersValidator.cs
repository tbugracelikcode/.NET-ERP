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
                .WithMessage("Lütfen ürün referans numarasını yazın.")
                .MaximumLength(17)
                .WithMessage("Ürün referans numarası, 17 karakterden fazla olamaz.");

            RuleFor(x => x.ProductID)
                 .Must(x => x.HasValue && x.Value != Guid.Empty)
                .WithMessage("Lütfen ürün seçin.");

            RuleFor(x => x.CurrentAccountCardID)
               .Must(x => x.HasValue && x.Value != Guid.Empty)
              .WithMessage("Lütfen cari hesap kartı seçin.");

        }
    }
}
