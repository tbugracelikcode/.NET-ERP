using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;

namespace TsiErp.Business.Entities.UnsuitabilityItemSPC.Validations
{
    public class UpdateUnsuitabilityItemSPCsValidator : TsiAbstractValidatorBase<UpdateUnsuitabilityItemSPCsDto>
    {
        public UpdateUnsuitabilityItemSPCsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
