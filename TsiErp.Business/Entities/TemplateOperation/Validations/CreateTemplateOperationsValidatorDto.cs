using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;

namespace TsiErp.Business.Entities.TemplateOperation.Validations
{
    public class CreateTemplateOperationsValidatorDto : AbstractValidator<CreateTemplateOperationsDto>
    {
        public CreateTemplateOperationsValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen operasyon kodunu yazın.")
               .MaximumLength(17)
               .WithMessage("Operasyon kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Lütfen operasyon açıklamasını yazın.")
               .MaximumLength(200)
               .WithMessage("Operasyon açıklaması 200 karakterden fazla olamaz.");

            RuleFor(x => x.WorkCenterID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün grubunu seçin.");

        }

    }
}
