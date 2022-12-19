using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Validations
{
    public class UpdateBillsofMaterialsValidatorDto : AbstractValidator<UpdateBillsofMaterialsDto>
    {
        public UpdateBillsofMaterialsValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen reçete kodunu yazın.")
               .MaximumLength(17)
               .WithMessage("Reçete kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Lütfen reçete adını yazın.")
               .MaximumLength(200)
               .WithMessage("Reçete adı 200 karakterden fazla olamaz.");


            RuleFor(x => x.FinishedProductID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen mamül seçin.");

            RuleFor(x => x.RouteID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen mamül seçin.");


        }
    }
}
