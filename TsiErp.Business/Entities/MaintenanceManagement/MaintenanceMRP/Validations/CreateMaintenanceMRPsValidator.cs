using FluentValidation;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Validations
{
    public class CreateMaintenanceMRPsValidatorDto : AbstractValidator<CreateMaintenanceMRPsDto>
    {
        public CreateMaintenanceMRPsValidatorDto()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
