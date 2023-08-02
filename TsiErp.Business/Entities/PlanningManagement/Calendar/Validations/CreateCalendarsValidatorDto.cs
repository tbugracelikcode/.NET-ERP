using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;

namespace TsiErp.Business.Entities.Calendar.Validations
{
    public class CreateCalendarsValidatorDto : AbstractValidator<CreateCalendarsDto>
    {
        public CreateCalendarsValidatorDto()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Year)
               .NotEmpty()
               .WithMessage("ValidatorYearEmpty");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("ValidatorNameEmpty")
               .MaximumLength(200)
               .WithMessage("ValidatorNameMaxLenght");

        }
    }
}
