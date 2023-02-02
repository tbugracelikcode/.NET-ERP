using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Validations
{
    public class UpdatePlannedMaintenanceValidatorDto : AbstractValidator<UpdatePlannedMaintenancesDto>
    {
        public UpdatePlannedMaintenanceValidatorDto()
        {
            RuleFor(x => x.RegistrationNo)
              .NotEmpty()
              .WithMessage("Lütfen bakım kayıt numarasını yazın.")
              .MaximumLength(17)
              .WithMessage("Bakım kayıt numarası, 17 karakterden fazla olamaz.");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen istasyon seçin.");

            RuleFor(x => x.PeriodID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfenbakım türü seçin.");

        }
    }
}
