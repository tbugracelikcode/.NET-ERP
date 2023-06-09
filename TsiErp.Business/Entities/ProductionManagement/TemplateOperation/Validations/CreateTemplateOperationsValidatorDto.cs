using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;

namespace TsiErp.Business.Entities.TemplateOperation.Validations
{
    public class CreateTemplateOperationsValidatorDto : AbstractValidator<CreateTemplateOperationsDto>
    {
        public CreateTemplateOperationsValidatorDto()
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
               .WithMessage("OValidatorNameMaxLenght");

            //RuleFor(x => x.WorkCenterID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün grubunu seçin.");

        }

    }
}
