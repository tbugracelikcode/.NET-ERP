using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionOrder.Validations
{
    public class CreateProductionOrdersValidator : TsiAbstractValidatorBase<CreateProductionOrdersDto>
    {
        public CreateProductionOrdersValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("Lütfen fiş numarasını yazın.")
                .MaximumLength(17)
                .WithMessage("Fiş numarası, 17 karakterden fazla olamaz.");

            RuleFor(x => x.OrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen siparişi seçin.");

            RuleFor(x => x.FinishedProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen mamül seçin.");

            RuleFor(x => x.UnitSetID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen birim seti seçin.");

            RuleFor(x => x.BOMID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen reçete seçin.");

            RuleFor(x => x.RouteID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen rota seçin.");

            RuleFor(x => x.PropositionID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen teklif seçin.");

            RuleFor(x => x.CurrentAccountID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen cari hesap seçin.");
        }
    }
}
