using FluentValidation;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance.Dtos;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Validations
{
    public class CreateUnplannedMaintenanceValidatorDto : AbstractValidator<CreateUnplannedMaintenancesDto>
    {
        public CreateUnplannedMaintenanceValidatorDto()
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
