using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;

namespace TsiErp.Business.Entities.StockManagement.StockAddress.Validations
{
    public class CreateStockAddressesValidator : TsiAbstractValidatorBase<CreateStockAddressesDto>
    {
        public CreateStockAddressesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

        }
    }
}
