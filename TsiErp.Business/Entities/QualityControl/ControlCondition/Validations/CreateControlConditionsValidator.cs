using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;

namespace TsiErp.Business.Entities.QualityControl.ControlCondition.Validations
{
    public class CreateControlConditionsValidator : TsiAbstractValidatorBase<CreateControlConditionsDto>
    {
        public CreateControlConditionsValidator()
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
