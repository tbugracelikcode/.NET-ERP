using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Validations
{
    public class CreateProductionTrackingsValidator : TsiAbstractValidatorBase<CreateProductionTrackingsDto>
    {
        public CreateProductionTrackingsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLength");

            RuleFor(x => x.StationID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorStationID");

            RuleFor(x => x.EmployeeID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorEmployeeID");

            RuleFor(x => x.CurrentAccountCardID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorCurrentCardID");

            RuleFor(x => x.ShiftID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen vardiya seçin.");
        }
    }
}
