using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.TaskScoring.Validations
{
    public class UpdateTaskScoringsValidator : TsiAbstractValidatorBase<UpdateTaskScoringsDto>
    {
        public UpdateTaskScoringsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.SeniorityID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorSeniorityID");

        }
    }
}
