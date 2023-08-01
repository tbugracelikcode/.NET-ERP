using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;

namespace TsiErp.Business.Entities.QualityControl.ControlType.Validations
{
    public class CreateControlTypesValidator : TsiAbstractValidatorBase<CreateControlTypesDto>
    {
        public CreateControlTypesValidator()
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
