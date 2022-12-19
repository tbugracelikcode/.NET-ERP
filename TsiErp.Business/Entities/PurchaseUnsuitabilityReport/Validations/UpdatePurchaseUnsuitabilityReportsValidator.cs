using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Validations
{
    public class UpdatePurchaseUnsuitabilityReportsValidator : TsiAbstractValidatorBase<UpdatePurchaseUnsuitabilityReportsDto>
    {
        public UpdatePurchaseUnsuitabilityReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("Lütfen fiş numarasını yazın.")
                .MaximumLength(17)
                .WithMessage("Fiş numarası, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Date_)
               .NotEmpty()
               .WithMessage("Lütfen tarihi seçin.");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün seçin.");

            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen cari hesap kartı seçin.");

            RuleFor(x => x.OrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen sipariş seçin.");

        }
    }
}
