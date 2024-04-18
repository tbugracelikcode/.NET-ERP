using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;

namespace TsiErp.Business.Entities.StockManagement.ProductProperty.Validations
{
    public class UpdateProductPropertiesValidator : TsiAbstractValidatorBase<UpdateProductPropertiesDto>
    {
        public UpdateProductPropertiesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("ValidatorNameEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorNameMaxLenght");


        }
    }
}
