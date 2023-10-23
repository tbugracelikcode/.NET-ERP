using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintReport.Validations
{
    public class CreateCustomerComplaintReportsValidator : TsiAbstractValidatorBase<CreateCustomerComplaintReportsDto>
    {
        public CreateCustomerComplaintReportsValidator()
        {
            RuleFor(x => x.ReportNo)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.SalesOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorSalesOrderID");

            RuleFor(x => x.UnsuitqabilityItemsID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUnsuitqabilityItemsID");


        }
    }
}
