using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;

namespace TsiErp.Business.Entities.ProductsOperation.Validations
{
    public class CreateProductsOperationsValidatorDto : AbstractValidator<CreateProductsOperationsDto>
    {
        public CreateProductsOperationsValidatorDto()
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

            RuleFor(x => x.ProductID)
               .Must(x => x.HasValue && x.Value != Guid.Empty)
              .WithMessage("Lütfen ürün seçin.");

            RuleFor(x => x.WorkCenterID)
               .Must(x => x.HasValue && x.Value != Guid.Empty)
              .WithMessage("Lütfen ürün seçin.");

        }
    }
}
