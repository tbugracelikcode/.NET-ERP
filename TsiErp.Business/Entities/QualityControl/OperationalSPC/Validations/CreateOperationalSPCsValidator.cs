using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;

namespace TsiErp.Business.Entities.OperationalSPC.Validations
{
    public class CreateOperationalSPCsValidator : TsiAbstractValidatorBase<CreateOperationalSPCsDto>
    {
        public CreateOperationalSPCsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
