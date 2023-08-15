using FluentValidation;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;

namespace TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Validations
{
    public class CreateOperationalQualityPlansValidatorDto : AbstractValidator<CreateOperationalQualityPlansDto>
    {
        public CreateOperationalQualityPlansValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.ProductID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorProductID");

            RuleFor(x => x.ProductsOperationID)
              .Must(x => x.HasValue && x.Value != Guid.Empty)
             .WithMessage("ValidatorProductsOperationID");

            RuleFor(x => x.WorkCenterID)
              .Must(x => x.HasValue && x.Value != Guid.Empty)
             .WithMessage("ValidatorWorkCenterID");



        }
    }
}
