using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ByDateStockMovement.Dtos;

namespace TsiErp.Business.Entities.ByDateStockMovement.Validations
{
    public class CreateByDateStockMovementsValidator : TsiAbstractValidatorBase<CreateByDateStockMovementsDto>
    {
        public CreateByDateStockMovementsValidator()
        {
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün seçin.");

            RuleFor(x => x.WarehouseID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen depo seçin.");

            RuleFor(x => x.BranchID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen şube seçin.");

        }
    }
}
