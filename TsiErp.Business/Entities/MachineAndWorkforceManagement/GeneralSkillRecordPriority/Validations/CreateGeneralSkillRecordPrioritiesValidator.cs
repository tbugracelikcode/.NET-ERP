using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Validations
{
    public class CreateGeneralSkillRecordPrioritiesValidator : TsiAbstractValidatorBase<CreateGeneralSkillRecordPrioritiesDto>
    {
        public CreateGeneralSkillRecordPrioritiesValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.GeneralSkillID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorGeneralSkillID");

        }
    }
}
