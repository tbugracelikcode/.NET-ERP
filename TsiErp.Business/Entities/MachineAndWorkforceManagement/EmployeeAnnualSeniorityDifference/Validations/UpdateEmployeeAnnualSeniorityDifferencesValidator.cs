using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Validations
{
    public class UpdateEmployeeAnnualSeniorityDifferencesValidator : TsiAbstractValidatorBase<UpdateEmployeeAnnualSeniorityDifferencesDto>
    {
        public UpdateEmployeeAnnualSeniorityDifferencesValidator()
        {

            RuleFor(x => x.SeniorityID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorSeniorityID");

        }
    }
}
