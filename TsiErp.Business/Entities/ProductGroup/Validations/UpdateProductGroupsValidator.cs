using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;

namespace TsiErp.Business.Entities.ProductGroup.Validations
{
    public class UpdateProductGroupsValidator : TsiAbstractValidatorBase<UpdateProductGroupsDto>
    {
        public UpdateProductGroupsValidator()
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
