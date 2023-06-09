using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionOrder.Validations
{
    public class UpdateProductionOrdersValidator : TsiAbstractValidatorBase<UpdateProductionOrdersDto>
    {
        public UpdateProductionOrdersValidator()
        {
            RuleFor(x => x.FicheNo)
                  .NotEmpty()
                  .WithMessage("ValidatorCodeEmpty")
                  .MaximumLength(17)
                  .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.OrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorOrderID");

            RuleFor(x => x.FinishedProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorFinishedProductID");

            RuleFor(x => x.UnitSetID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUnitSetID");

            RuleFor(x => x.BOMID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorBoMID");

            RuleFor(x => x.RouteID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorRouteID");

            RuleFor(x => x.PropositionID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorPropositionID");

            RuleFor(x => x.CurrentAccountID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountID");
        }
    }
}
