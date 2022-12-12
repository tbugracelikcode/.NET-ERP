using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PurchaseRequest.Dtos;

namespace TsiErp.Business.Entities.PurchaseRequest.Validations
{
    public class UpdatePurchaseRequestsValidator : AbstractValidator<UpdatePurchaseRequestsDto>
    {
        public UpdatePurchaseRequestsValidator()
        {
            RuleFor(x => x.FicheNo)
               .NotEmpty()
               .WithMessage("Lütfen talep numarasını yazın.")
               .MaximumLength(17)
               .WithMessage("Talep numarası 17 karakterden fazla olamaz.");

            RuleFor(x => x.ValidityDate_)
               .NotEmpty()
               .WithMessage("Lütfen talep geçerlilik tarihi seçin.");


            RuleFor(x => x.CurrentAccountCardID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen cari hesap seçin.");

            RuleFor(x => x.CurrencyID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen para birimi seçin.");

            RuleFor(x => x.WarehouseID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen depo seçin.");

            RuleFor(x => x.BranchID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen şube seçin.");


            RuleFor(x => x.ExchangeRate)
                .NotNull()
                .WithMessage("Lütfen kur tutarını yazın.")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Kur tutarı 0'dan büyük olmalıdır.");

        }
    }
}
