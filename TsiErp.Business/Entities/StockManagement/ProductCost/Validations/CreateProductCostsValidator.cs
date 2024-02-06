using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos;

namespace TsiErp.Business.Entities.StockManagement.ProductCostCost.Validations
{
    public class CreateProductCostsValidator : TsiAbstractValidatorBase<CreateProductCostsDto>
    {
        public CreateProductCostsValidator()
        {
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");
        }
    }
}
