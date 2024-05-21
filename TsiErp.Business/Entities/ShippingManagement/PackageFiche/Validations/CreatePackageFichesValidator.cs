using FluentValidation;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;

namespace TsiErp.Business.Entities.ShippingManagement.PackageFiche.Validations
{
    public class CreatePackageFichesValidator : AbstractValidator<CreatePackageFichesDto>
    {
        public CreatePackageFichesValidator()
        {
            //RuleFor(x => x.Code)
            //   .NotEmpty()
            //   .WithMessage("ValidatorCodeEmpty")
            //   .MaximumLength(17)
            //   .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.SalesOrderID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorSalesOrderID");


            RuleFor(x => x.ProductID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorProductID");

        }
    }
}
