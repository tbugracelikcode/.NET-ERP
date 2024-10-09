using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CostManagement.CPR.Dtos;

namespace TsiErp.Business.Entities.CostManagement.CPR.Validations
{
    public class UpdateCPRsValidator : TsiAbstractValidatorBase<UpdateCPRsDto>
    {
        public UpdateCPRsValidator()
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
