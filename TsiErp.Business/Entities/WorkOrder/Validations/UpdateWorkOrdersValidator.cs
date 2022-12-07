using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.WorkOrder.Dtos;

namespace TsiErp.Business.Entities.WorkOrder.Validations
{
    public class UpdateWorkOrdersValidator : TsiAbstractValidatorBase<UpdateWorkOrdersDto>
    {
        public UpdateWorkOrdersValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen iş emri kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("İş emri kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.WorkOrderNo)
                .NotEmpty()
                .WithMessage("Lütfen iş emri numarasını yazın.")
                .MaximumLength(200)
                .WithMessage("İş emri numarası, 200 karakterden fazla olamaz.");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen iş emri seçin.");
            RuleFor(x => x.PropositionID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen sipariş seçin.");
            RuleFor(x => x.ProductsOperationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen operasyon seçin.");
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün seçin.");



        }
    }
}
