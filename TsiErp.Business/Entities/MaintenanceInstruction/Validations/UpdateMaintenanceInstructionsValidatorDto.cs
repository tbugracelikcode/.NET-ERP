using FluentValidation;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Validations
{
    public class UpdateMaintenanceInstructionValidatorDto : AbstractValidator<UpdateMaintenanceInstructionsDto>
    {
        public UpdateMaintenanceInstructionValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorStationID");

            RuleFor(x => x.PeriodID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorPeriodID");

        }
    }
}
