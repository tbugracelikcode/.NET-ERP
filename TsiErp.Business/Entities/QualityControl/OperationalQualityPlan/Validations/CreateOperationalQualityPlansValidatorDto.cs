using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;

namespace TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Validations
{
    public class CreateOperationalQualityPlansValidatorDto : TsiAbstractValidatorBase<CreateOperationalQualityPlansDto>
    {
        public CreateOperationalQualityPlansValidatorDto()
        {
            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");
            RuleFor(x => x.ProductsOperationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductsOperationID");

        }
    }
}
