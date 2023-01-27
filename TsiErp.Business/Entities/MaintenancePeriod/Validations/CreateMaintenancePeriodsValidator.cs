using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;

namespace TsiErp.Business.Entities.MaintenancePeriod.Validations
{
    public class CreateMaintenancePeriodsValidator : TsiAbstractValidatorBase<CreateMaintenancePeriodsDto>
    {
        public CreateMaintenancePeriodsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen bakım periyodu kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Bakım periyodu kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen bakım periyodu adını yazın.")
                .MaximumLength(200)
                .WithMessage("Bakım periyodu adı 200 karakterden fazla olamaz."); ;
        }
    }
}
