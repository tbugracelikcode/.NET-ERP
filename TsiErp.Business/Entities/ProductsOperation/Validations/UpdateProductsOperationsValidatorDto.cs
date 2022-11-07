using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;

namespace TsiErp.Business.Entities.ProductsOperation.Validations
{
    public class UpdateProductsOperationsValidatorDto : AbstractValidator<UpdateProductsOperationsDto>
    {
        public UpdateProductsOperationsValidatorDto()
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

        }
    }
}
