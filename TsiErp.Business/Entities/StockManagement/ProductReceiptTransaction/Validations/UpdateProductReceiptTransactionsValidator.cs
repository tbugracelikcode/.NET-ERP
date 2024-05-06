using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;

namespace TsiErp.Business.Entities.StockManagement.ProductReceiptTransaction.Validations
{
    public class UpdateProductReceiptTransactionsValidator : TsiAbstractValidatorBase<UpdateProductReceiptTransactionsDto>
    {
        public UpdateProductReceiptTransactionsValidator()
        {

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");
            RuleFor(x => x.PurchaseOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorPurchaseOrderID");
            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountCardID");

        }
    }
}
