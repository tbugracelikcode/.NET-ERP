using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Shift.Dtos;

namespace TsiErp.Business.Entities.Shift.Validations
{
    public class CreateShiftsValidatorDto : AbstractValidator<CreateShiftsDto>
    {
        public CreateShiftsValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen vardiya kodunu yazın.")
               .MaximumLength(17)
               .WithMessage("Vardiya kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Lütfen vardiya açıklamasını yazın.")
               .MaximumLength(200)
               .WithMessage("Vardiya açıklaması 200 karakterden fazla olamaz.");

            RuleFor(x => x.TotalWorkTime).GreaterThanOrEqualTo(1).WithMessage("Toplam çalışma süresi, 0'dan büyük olmalıdır.");

            RuleFor(x => x.TotalBreakTime).GreaterThanOrEqualTo(1).WithMessage("Toplam mola süresi, 0'dan büyük olmalıdır.");


        }

    }
}
