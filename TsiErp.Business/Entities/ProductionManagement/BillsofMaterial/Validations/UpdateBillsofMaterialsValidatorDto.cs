using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Validations
{
    public class UpdateBillsofMaterialsValidatorDto : AbstractValidator<UpdateBillsofMaterialsDto>
    {
        public UpdateBillsofMaterialsValidatorDto()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("ValidatorNameEmpty")
               .MaximumLength(200)
               .WithMessage("ValidatorNameMaxLenght");


            RuleFor(x => x.FinishedProductID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorFinishedProductID");

            RuleFor(x => x.CurrentAccountCardID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorCurrentCardID");



        }
    }
}
