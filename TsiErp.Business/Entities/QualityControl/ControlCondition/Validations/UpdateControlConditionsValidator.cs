using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;

namespace TsiErp.Business.Entities.QualityControl.ControlCondition.Validations
{
    public class UpdateControlConditionsValidator : TsiAbstractValidatorBase<UpdateControlConditionsDto>
    {
        public UpdateControlConditionsValidator()
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
