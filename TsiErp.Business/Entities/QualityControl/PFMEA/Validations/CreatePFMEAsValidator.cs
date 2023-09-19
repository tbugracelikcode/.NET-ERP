using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos;

namespace TsiErp.Business.Entities.QualityControl.PFMEA.Validations
{
    public class CreatePFMEAsValidator : TsiAbstractValidatorBase<CreatePFMEAsDto>
    {
        public CreatePFMEAsValidator()
        {

            RuleFor(x => x.FirstOperationalSPCID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorFirstOperationalSPCID");
            RuleFor(x => x.OperationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorOperationID");
            RuleFor(x => x.WorkCenterID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorWorkCenterID");
            RuleFor(x => x.UnsuitabilityItemID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUnsuitabilityItemID");
        }
    }
}
