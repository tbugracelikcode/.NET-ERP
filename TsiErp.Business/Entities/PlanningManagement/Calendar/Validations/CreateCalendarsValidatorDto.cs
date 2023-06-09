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
               .WithMessage("Lütfen çalışma takvimi kodunu giriniz.")
               .MaximumLength(17)
               .WithMessage("Çalışma takvimi kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Year)
               .NotEmpty()
               .WithMessage("Lütfen takvim yılını seçiniz.");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Lütfen çalışma takvimi adını giriniz.")
               .MaximumLength(200)
               .WithMessage("Çalışma takvimi adı 200 karakterden fazla olamaz.");

        }
    }
}
