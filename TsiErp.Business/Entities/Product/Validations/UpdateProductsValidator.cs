using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Product.Dtos;

namespace TsiErp.Business.Entities.Product.Validations
{
    public class UpdateProductsValidator : TsiAbstractValidatorBase<UpdateProductsDto>
    {
        public UpdateProductsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen ürün kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Ürün kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen ürün adını yazın.")
                .MaximumLength(200)
                .WithMessage("Ürün adı 200 karakterden fazla olamaz.");

            RuleFor(x => x.ProductGrpID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün grubunu seçin.");

            RuleFor(x => x.UnitSetID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen birim setini seçin.");
        }
    }
}
