using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.StartingSalary.Validations
{
    public class UpdateStartingSalariesValidator : TsiAbstractValidatorBase<UpdateStartingSalariesDto>
    {
        public UpdateStartingSalariesValidator()
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
