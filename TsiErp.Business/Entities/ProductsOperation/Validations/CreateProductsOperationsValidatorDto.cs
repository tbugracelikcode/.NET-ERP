using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;

namespace TsiErp.Business.Entities.ProductsOperation.Validations
{
    public class CreateProductsOperationsValidatorDto : AbstractValidator<CreateProductsOperationsDto>
    {
        public CreateProductsOperationsValidatorDto()
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

            RuleFor(x => x.ProductID)
               .Must(x => x.HasValue && x.Value != Guid.Empty)
              .WithMessage("ValidatorProductID");

            //RuleFor(x => x.WorkCenterID)
            //   .Must(x => x.HasValue && x.Value != Guid.Empty)
            //  .WithMessage("Lütfen ürün seçin.");

        }
    }
}
