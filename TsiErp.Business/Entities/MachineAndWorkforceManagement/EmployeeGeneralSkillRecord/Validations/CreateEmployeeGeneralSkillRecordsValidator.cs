using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Validations
{
    public class CreateEmployeeGeneralSkillRecordsValidator : TsiAbstractValidatorBase<CreateEmployeeGeneralSkillRecordsDto>
    {
        public CreateEmployeeGeneralSkillRecordsValidator()
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
