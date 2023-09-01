using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;

namespace TsiErp.Business.Entities.OperationalSPC.Validations
{
    public class UpdateOperationalSPCsValidator : TsiAbstractValidatorBase<UpdateOperationalSPCsDto>
    {
        public UpdateOperationalSPCsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
