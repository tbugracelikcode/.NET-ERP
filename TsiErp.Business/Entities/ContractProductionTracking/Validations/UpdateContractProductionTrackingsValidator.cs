using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ContractProductionTracking.Validations
{
    public class UpdateContractProductionTrackingsValidator : TsiAbstractValidatorBase<UpdateContractProductionTrackingsDto>
    {
        public UpdateContractProductionTrackingsValidator()
        {
            RuleFor(x => x.StationID)
              .Must(x => x.HasValue && x.Value != Guid.Empty)
             .WithMessage("Lütfeniş istasyonu seçin.");

            RuleFor(x => x.EmployeeID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen çalışan seçin.");

            RuleFor(x => x.ShiftID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen vardiya seçin.");

            RuleFor(x => x.CurrentAccountID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen cari hesap kartı seçin.");
        }
    }
}
