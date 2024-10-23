using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;

namespace TsiErp.Business.Entities.PurchaseManagement.PurchaseInvoice.Validations
{
    public class CreatePurchaseInvoicesValidator : AbstractValidator<CreatePurchaseInvoicesDto>
    {
        public CreatePurchaseInvoicesValidator()
        {
            RuleFor(x => x.FicheNo)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Date_)
               .NotEmpty()
               .WithMessage("ValidatorDate");



            RuleFor(x => x.WarehouseID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorWarehouseID");


            RuleFor(x => x.BranchID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorBranchID");

        }
    }
}