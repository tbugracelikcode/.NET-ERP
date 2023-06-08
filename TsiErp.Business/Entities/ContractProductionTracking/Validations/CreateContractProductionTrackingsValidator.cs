using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ContractProductionTracking.Validations
{
    public class CreateContractProductionTrackingsValidator : TsiAbstractValidatorBase<CreateContractProductionTrackingsDto>
    {
        public CreateContractProductionTrackingsValidator()
        {
            RuleFor(x => x.StationID)
               .Must(x => x.HasValue && x.Value != Guid.Empty)
              .WithMessage("ValidatorStationID");

            RuleFor(x => x.EmployeeID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorEmployeeID");

            RuleFor(x => x.ShiftID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorShiftID");

            RuleFor(x => x.CurrentAccountID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorCurrentAccountID");
        }
    }
}
