using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Validations
{
    public class CreateProductionTrackingsValidator : TsiAbstractValidatorBase<CreateProductionTrackingsDto>
    {
        public CreateProductionTrackingsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen üretim takip kodunu yazın.")
               .MaximumLength(17)
               .WithMessage("Üretim takip kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfeniş istasyonu seçin.");

            RuleFor(x => x.EmployeeID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen çalışan seçin.");

            RuleFor(x => x.ShiftID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen vardiya seçin.");
        }
    }
}
