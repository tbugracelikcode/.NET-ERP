using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EducationLevelScore.Validations
{
    public class CreateEducationLevelScoresValidator : TsiAbstractValidatorBase<CreateEducationLevelScoresDto>
    {
        public CreateEducationLevelScoresValidator()
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
                .WithMessage("ValidatorNameMaxLenght"); ;

        }
    }
}
