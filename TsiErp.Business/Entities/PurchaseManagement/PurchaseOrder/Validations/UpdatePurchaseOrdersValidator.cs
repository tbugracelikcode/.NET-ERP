using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrder.Validations
{
    public class UpdatePurchaseOrdersValidator : AbstractValidator<UpdatePurchaseOrdersDto>
    {
        public UpdatePurchaseOrdersValidator()
        {
            RuleFor(x => x.FicheNo)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Date_)
               .NotEmpty()
               .WithMessage("ValidatorDate");


            //RuleFor(x => x.CurrentAccountCardID)
            //    .Must(x => x.HasValue && x.Value != Guid.Empty)
            //   .WithMessage("ValidatorCurrentAccountID");

            //RuleFor(x => x.CurrencyID)
            //    .Must(x => x.HasValue && x.Value != Guid.Empty)
            //   .WithMessage("ValidatorCurrencyID");

            RuleFor(x => x.WarehouseID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorWarehouseID");


            RuleFor(x => x.BranchID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorBranchID");

            //RuleFor(x => x.ExchangeRate)
            //    .NotNull()
            //    .WithMessage("ValidatorExchangeRateEmpty")
            //    .GreaterThanOrEqualTo(1)
            //    .WithMessage("ValidatorExchangeRateMin");
        }
    }
}
