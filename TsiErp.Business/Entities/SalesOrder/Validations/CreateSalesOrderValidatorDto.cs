using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Validations
{
    public class CreateSalesOrderValidatorDto : AbstractValidator<CreateSalesOrderDto>
    {
        public CreateSalesOrderValidatorDto()
        {
            RuleFor(x => x.FicheNo)
               .NotEmpty()
               .WithMessage("Lütfen teklif numarasını yazın.")
               .MaximumLength(17)
               .WithMessage("Teklif numarası 17 karakterden fazla olamaz.");


            RuleFor(x => x.CurrentAccountCardID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen cari hesap seçin.");

            RuleFor(x => x.CurrencyID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen para birimi seçin.");

            RuleFor(x => x.WarehouseID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen depo seçin.");

            //RuleFor(x => x.PaymentPlanID)
            //    .Must(x => x.HasValue && x.Value != Guid.Empty)
            //   .WithMessage("Lütfen ödeme planı seçin.");

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
