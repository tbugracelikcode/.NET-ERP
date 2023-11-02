using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;

namespace TsiErp.Business.Entities.QualityControl.FirstProductApproval.Validations
{
    public class UpdateFirstProductApprovalsValidatorDto : TsiAbstractValidatorBase<UpdateFirstProductApprovalsDto>
    {
        public UpdateFirstProductApprovalsValidatorDto()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.WorkOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorWorkOrderID");

        }
    }
}
