using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.WorkOrder.Dtos;

namespace TsiErp.Business.Entities.WorkOrder.Validations
{
    public class CreateWorkOrdersValidator : TsiAbstractValidatorBase<CreateWorkOrdersDto>
    {
        public CreateWorkOrdersValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.WorkOrderNo)
                .NotEmpty()
                .WithMessage("ValidatorNameEmpty")
                .MaximumLength(200)
                .WithMessage("ValidatorNameMaxLenght");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductionOrderID");
            RuleFor(x => x.PropositionID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorPropositionID");
            RuleFor(x => x.ProductsOperationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductsOperationID");
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");



        }
    }
}
