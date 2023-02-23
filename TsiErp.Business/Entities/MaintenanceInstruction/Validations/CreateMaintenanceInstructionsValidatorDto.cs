using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Validations
{
    public class CreateMaintenanceInstructionValidatorDto : AbstractValidator<CreateMaintenanceInstructionsDto>
    {
        public CreateMaintenanceInstructionValidatorDto()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("Lütfen bakım talimatı kodunu yazın.")
              .MaximumLength(17)
              .WithMessage("Bakım talimatı kodu, 17 karakterden fazla olamaz.");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen istasyon seçin.");

            RuleFor(x => x.PeriodID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen bakım türü seçin.");

        }
    }
}
