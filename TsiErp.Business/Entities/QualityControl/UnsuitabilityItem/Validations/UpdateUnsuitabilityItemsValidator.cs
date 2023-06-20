using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Validations
{
    public class UpdateUnsuitabilityItemsValidator : TsiAbstractValidatorBase<UpdateUnsuitabilityItemsDto>
    {
        public UpdateUnsuitabilityItemsValidator()
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
