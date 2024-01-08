using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Validations
{
    public class CreateShipmentPlanningsValidator : AbstractValidator<CreateShipmentPlanningsDto>
    {
        public CreateShipmentPlanningsValidator()
        {
            RuleFor(x => x.Code)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorCodeMaxLenght");
        }
    }
}
