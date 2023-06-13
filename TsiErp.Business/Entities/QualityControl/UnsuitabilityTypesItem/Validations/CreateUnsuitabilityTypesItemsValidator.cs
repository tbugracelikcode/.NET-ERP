using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Validations
{
    public class CreateUnsuitabilityTypesItemsValidator : TsiAbstractValidatorBase<CreateUnsuitabilityTypesItemsDto>
    {
        public CreateUnsuitabilityTypesItemsValidator()
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
