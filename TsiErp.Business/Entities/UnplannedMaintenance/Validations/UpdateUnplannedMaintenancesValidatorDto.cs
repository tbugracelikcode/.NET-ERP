using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Validations
{
    public class UpdateUnplannedMaintenanceValidatorDto : AbstractValidator<UpdateUnplannedMaintenancesDto>
    {
        public UpdateUnplannedMaintenanceValidatorDto()
        {
            RuleFor(x => x.RegistrationNo)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLength");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorStationID");

            RuleFor(x => x.PeriodID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorPeriodID");

        }
    }
}
