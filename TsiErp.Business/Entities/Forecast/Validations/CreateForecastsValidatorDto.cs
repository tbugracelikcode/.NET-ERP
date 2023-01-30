using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Forecast.Dtos;

namespace TsiErp.Business.Entities.Forecast.Validations
{
    public class CreateForecastsValidatorDto : AbstractValidator<CreateForecastsDto>
    {
        public CreateForecastsValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen kodu yazın.")
               .MaximumLength(17)
               .WithMessage("Kod, 17 karakterden fazla olamaz.");


            RuleFor(x => x.CurrentAccountCardID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen cari hesap seçin.");

            RuleFor(x => x.BranchID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen şube seçin.");

            RuleFor(x => x.PeriodID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen dönem seçin.");


         
        }
    }
}
