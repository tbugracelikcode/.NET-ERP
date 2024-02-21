using FluentValidation;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.MRPII.Validations
{
    public class CreateMRPIIsValidator : AbstractValidator<CreateMRPIIsDto>
    {
        public CreateMRPIIsValidator()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
