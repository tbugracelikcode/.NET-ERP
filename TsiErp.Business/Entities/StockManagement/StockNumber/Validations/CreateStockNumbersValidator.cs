using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.StockNumber.Dtos;

namespace TsiErp.Business.Entities.StockManagement.StockNumber.Validations
{
    public class CreateStockNumbersValidator : TsiAbstractValidatorBase<CreateStockNumbersDto>
    {
        public CreateStockNumbersValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("ValidatorNameEmpty")
                .MaximumLength(200)
                .WithMessage("ValidatorNameMaxLenght"); ;

        }
    }
}
