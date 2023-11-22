using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Validations
{
    public class CreateEmployeeAnnualSeniorityDifferencesValidator : TsiAbstractValidatorBase<CreateEmployeeAnnualSeniorityDifferencesDto>
    {
        public CreateEmployeeAnnualSeniorityDifferencesValidator()
        {

            RuleFor(x => x.SeniorityID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorSeniorityID");

        }
    }
}
