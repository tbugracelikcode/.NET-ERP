using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;

namespace TsiErp.Business.Entities.MaintenancePeriod.Validations
{
    public class CreateMaintenancePeriodsValidator : TsiAbstractValidatorBase<CreateMaintenancePeriodsDto>
    {
        public CreateMaintenancePeriodsValidator()
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
