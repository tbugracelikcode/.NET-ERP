using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;

namespace TsiErp.Business.Entities.StationInventory.Validations
{
    public class CreateStationInventoriesValidator : TsiAbstractValidatorBase<CreateStationInventoriesDto>
    {
        public CreateStationInventoriesValidator()
        {
            RuleFor(x => x.ProductID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorProductID");

        }
    }
}
