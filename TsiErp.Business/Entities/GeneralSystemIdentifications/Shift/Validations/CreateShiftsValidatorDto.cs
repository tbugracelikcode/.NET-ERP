using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;

namespace TsiErp.Business.Entities.Shift.Validations
{
    public class CreateShiftsValidatorDto : AbstractValidator<CreateShiftsDto>
    {
        public CreateShiftsValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("ValidatorNameEmpty")
               .MaximumLength(200)
               .WithMessage("ValidatorNameMaxLenght");

            RuleFor(x => x.TotalWorkTime).GreaterThanOrEqualTo(1).WithMessage("ValidatorTotalWorkTimeMin");

            RuleFor(x => x.TotalBreakTime).GreaterThanOrEqualTo(1).WithMessage("ValidatorTotalBreakTimeMin");


        }

    }
}
