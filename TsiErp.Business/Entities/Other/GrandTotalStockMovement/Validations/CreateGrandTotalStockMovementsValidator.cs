using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;

namespace TsiErp.Business.Entities.GrandTotalStockMovement.Validations
{
    public class CreateGrandTotalStockMovementsValidator : TsiAbstractValidatorBase<CreateGrandTotalStockMovementsDto>
    {
        public CreateGrandTotalStockMovementsValidator()
        {
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.WarehouseID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorWarehouseID");

            RuleFor(x => x.BranchID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorBranchID");

        }
    }
}
