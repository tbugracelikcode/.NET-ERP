using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.TestManagement.Continent.Dtos;

namespace TsiErp.Business.Entities.TestManagement.Continent.Validations
{
    public class UpdateContinentsValidator : TsiAbstractValidatorBase<UpdateContinentsDto>
    {
        public UpdateContinentsValidator()
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
