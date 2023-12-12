using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.OperationAdjustment.Validations
{
    public class UpdateOperationAdjustmentsValidator : TsiAbstractValidatorBase<UpdateOperationAdjustmentsDto>
    {
        public UpdateOperationAdjustmentsValidator()
        {
            RuleFor(x => x.WorkOrderId).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen iş emri seçin.");
            RuleFor(x => x.AdjustmentUserId).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ayarı yapan kullanıcıyı seçin.");
        }
    }
}
