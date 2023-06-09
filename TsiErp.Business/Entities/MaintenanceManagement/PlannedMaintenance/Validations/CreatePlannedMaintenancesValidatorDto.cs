using FluentValidation;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Validations
{
    public class CreatePlannedMaintenanceValidatorDto : AbstractValidator<CreatePlannedMaintenancesDto>
    {
        public CreatePlannedMaintenanceValidatorDto()
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
