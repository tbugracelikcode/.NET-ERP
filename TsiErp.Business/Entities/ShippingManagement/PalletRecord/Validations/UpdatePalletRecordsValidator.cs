using FluentValidation;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;

namespace TsiErp.Business.Entities.ShippingManagement.PalletRecord.Validations
{
    public class UpdatePalletRecordsValidator : AbstractValidator<UpdatePalletRecordsDto>
    {
        public UpdatePalletRecordsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("ValidatorNameEmpty")
              .MaximumLength(17)
              .WithMessage("ValidatorNameMaxLenght");

        }
    }
}
