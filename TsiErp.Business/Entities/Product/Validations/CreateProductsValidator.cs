using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Product.Dtos;

namespace TsiErp.Business.Entities.Product.Validations
{
    public class CreateProductsValidator : TsiAbstractValidatorBase<CreateProductsDto>
    {
        public CreateProductsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen stok kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Stok kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen stok açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Stok açıklaması 200 karakterden fazla olamaz.");

            RuleFor(x => x.ProductGrpID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün grubunu seçin.");

            RuleFor(x => x.UnitSetID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen birim setini seçin.");
        }
    }
}
