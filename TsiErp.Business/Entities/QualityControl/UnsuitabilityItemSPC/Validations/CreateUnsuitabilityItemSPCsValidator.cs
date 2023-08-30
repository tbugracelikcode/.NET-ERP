using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;

namespace TsiErp.Business.Entities.UnsuitabilityItemSPC.Validations
{
    public class CreateUnsuitabilityItemSPCsValidator : TsiAbstractValidatorBase<CreateUnsuitabilityItemSPCsDto>
    {
        public CreateUnsuitabilityItemSPCsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
