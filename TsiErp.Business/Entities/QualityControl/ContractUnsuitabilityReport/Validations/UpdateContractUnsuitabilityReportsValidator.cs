using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.ContractUnsuitabilityReport.Validations
{
    public class UpdateContractUnsuitabilityReportsValidator : TsiAbstractValidatorBase<UpdateContractUnsuitabilityReportsDto>
    {
        public UpdateContractUnsuitabilityReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Date_)
              .NotEmpty()
              .WithMessage("ValidatorDate");

            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountCardID");

            RuleFor(x => x.UnsuitabilityItemsID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUnsuitabilityItemsID");

            //RuleFor(x => x.WorkOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorWorkOrderID");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductionOrderID");

        }
    }
}
