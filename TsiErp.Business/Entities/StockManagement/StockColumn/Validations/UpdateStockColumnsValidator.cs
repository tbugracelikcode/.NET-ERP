using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.StockColumn.Dtos;

namespace TsiErp.Business.Entities.StockManagement.StockColumn.Validations
{
    public class UpdateStockColumnsValidator : TsiAbstractValidatorBase<UpdateStockColumnsDto>
    {
        public UpdateStockColumnsValidator()
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
