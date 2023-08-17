using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Validations
{
    public class CreatePurchaseUnsuitabilityReportsValidator : TsiAbstractValidatorBase<CreatePurchaseUnsuitabilityReportsDto>
    {
        public CreatePurchaseUnsuitabilityReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Date_)
               .NotEmpty()
               .WithMessage("ValidatorDate");

            RuleFor(x => x.Action_).NotEmpty().WithMessage("ValidatorAction");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountID");

            RuleFor(x => x.OrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorOrderID");

        }
    }
}
