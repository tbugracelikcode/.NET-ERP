using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeScoring.Validations
{
    public class CreateEmployeeScoringsValidator : TsiAbstractValidatorBase<CreateEmployeeScoringsDto>
    {
        public CreateEmployeeScoringsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");


        }
    }
}
