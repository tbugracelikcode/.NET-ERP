using FluentValidation;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;

namespace TsiErp.Business.Entities.ShippingManagement.PalletRecord.Validations
{
    public class CreatePalletRecordsValidator : AbstractValidator<CreatePalletRecordsDto>
    {
        public CreatePalletRecordsValidator()
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

            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountCardID");

            RuleFor(x => x.PackingListID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorPackingListID");

            RuleFor(x => x.PackageType)
             .NotEmpty()
             .WithMessage("ValidatorPackageTypeEmpty");

            RuleFor(x => x.PlannedLoadingTime)
               .NotEmpty()
               .WithMessage("ValidatorPlannedLoadingTime");

        }
    }
}
