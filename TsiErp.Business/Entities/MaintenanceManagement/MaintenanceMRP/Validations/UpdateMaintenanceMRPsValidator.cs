using FluentValidation;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;

namespace TsiErp.Business.Entities.MaintenanceMRP.Validations
{
    public class UpdateMaintenanceMRPsValidatorDto : AbstractValidator<UpdateMaintenanceMRPsDto>
    {
        public UpdateMaintenanceMRPsValidatorDto()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
