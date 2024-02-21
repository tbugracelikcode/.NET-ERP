using FluentValidation;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.MRPII.Validations
{
    public class UpdateMRPIIsValidator : AbstractValidator<UpdateMRPIIsDto>
    {
        public UpdateMRPIIsValidator()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
