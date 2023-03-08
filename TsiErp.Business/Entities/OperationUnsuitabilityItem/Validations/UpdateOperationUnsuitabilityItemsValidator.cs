using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Validations
{
    public class UpdateOperationUnsuitabilityItemsValidator : TsiAbstractValidatorBase<UpdateOperationUnsuitabilityItemsDto>
    {
        public UpdateOperationUnsuitabilityItemsValidator()
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
