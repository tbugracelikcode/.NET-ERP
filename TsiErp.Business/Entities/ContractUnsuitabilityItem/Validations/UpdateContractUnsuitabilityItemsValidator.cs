using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Validations
{
    public class UpdateContractUnsuitabilityItemsValidator : TsiAbstractValidatorBase<UpdateContractUnsuitabilityItemsDto>
    {
        public UpdateContractUnsuitabilityItemsValidator()
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

        }
    }
}
