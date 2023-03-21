using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Validations
{
    public class CreateUnitSetsValidator : TsiAbstractValidatorBase<CreateUnitSetsDto>
    {
        public CreateUnitSetsValidator()
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
