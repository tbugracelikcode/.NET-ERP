using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;

namespace TsiErp.Business.Entities.ProductionOrderChangeReport.Validations
{
    public class CreateProductionOrderChangeReportsValidator : TsiAbstractValidatorBase<CreateProductionOrderChangeReportsDto>
    {
        public CreateProductionOrderChangeReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.SalesOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorSalesOrderID");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductionOrderID");

            RuleFor(x => x.UnsuitabilityItemsID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUnsuitabilityItemsID");

        }
    }
}
