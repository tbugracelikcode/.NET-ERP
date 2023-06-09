using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos;

namespace TsiErp.Business.Entities.ByDateStockMovement.Validations
{
    public class CreateByDateStockMovementsValidator : TsiAbstractValidatorBase<CreateByDateStockMovementsDto>
    {
        public CreateByDateStockMovementsValidator()
        {
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.WarehouseID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorWarehouseID");

            RuleFor(x => x.BranchID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorBranchID");

        }
    }
}
