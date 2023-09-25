using FluentValidation;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.MRP.Validations
{
    public class CreateMRPsValidator : AbstractValidator<CreateMRPsDto>
    {
        public CreateMRPsValidator()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
