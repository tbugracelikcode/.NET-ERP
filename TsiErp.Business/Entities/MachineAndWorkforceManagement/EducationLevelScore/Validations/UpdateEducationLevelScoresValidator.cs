using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EducationLevelScore.Validations
{
    public class UpdateEducationLevelScoresValidator : TsiAbstractValidatorBase<UpdateEducationLevelScoresDto>
    {
        public UpdateEducationLevelScoresValidator()
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
