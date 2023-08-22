using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;

namespace TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Validations
{
    public class CreatePurchaseQualityPlansValidatorDto : TsiAbstractValidatorBase<CreatePurchaseQualityPlansDto>
    {
        public CreatePurchaseQualityPlansValidatorDto()
        {
            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");
            RuleFor(x => x.CurrrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrrentAccountCardID");

        }
    }
}
