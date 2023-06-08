using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;

namespace TsiErp.Business.Entities.Product.Validations
{
    public class CreateProductsValidator : TsiAbstractValidatorBase<CreateProductsDto>
    {
        public CreateProductsValidator()
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

            RuleFor(x => x.ProductGrpID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductGroupID");

            RuleFor(x => x.UnitSetID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUnitSetID");
        }
    }
}
