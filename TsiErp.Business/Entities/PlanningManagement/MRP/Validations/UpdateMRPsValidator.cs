using FluentValidation;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.MRP.Validations
{
    public class UpdateMRPsValidator : AbstractValidator<UpdateMRPsDto>
    {
        public UpdateMRPsValidator()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

        }
    }
}
