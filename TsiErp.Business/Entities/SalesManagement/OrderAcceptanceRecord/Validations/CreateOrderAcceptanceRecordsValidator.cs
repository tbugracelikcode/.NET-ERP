using FluentValidation;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;

namespace TsiErp.Business.Entities.SalesManagement.OrderAcceptanceRecord.Validations
{
    public class CreateOrderAcceptanceRecordsValidator : AbstractValidator<CreateOrderAcceptanceRecordsDto>
    {
        public CreateOrderAcceptanceRecordsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.CurrentAccountCardID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorCurrentAccountCardID");

        }
    }
}
