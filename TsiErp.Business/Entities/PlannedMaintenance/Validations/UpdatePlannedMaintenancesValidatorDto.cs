using FluentValidation;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Validations
{
    public class UpdatePlannedMaintenanceValidatorDto : AbstractValidator<UpdatePlannedMaintenancesDto>
    {
        public UpdatePlannedMaintenanceValidatorDto()
        {
            RuleFor(x => x.RegistrationNo)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorStationID");

            RuleFor(x => x.PeriodID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorPediodID");

        }
    }
}
