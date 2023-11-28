using FluentValidation;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;

namespace TsiErp.Business.Entities.ShippingManagement.PackingList.Validations
{
    public class CreatePackingListsValidator : AbstractValidator<CreatePackingListsDto>
    {
        public CreatePackingListsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Code2)
               .NotEmpty()
               .WithMessage("ValidatorCode2Empty")
               .MaximumLength(250)
               .WithMessage("ValidatorCode2MaxLenght");


            RuleFor(x => x.RecieverID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorRecieverID");

            RuleFor(x => x.TransmitterID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorTransmitterID");

            RuleFor(x => x.TIRType)
                .NotNull()
                .WithMessage("ValidatorTIRTypeEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorTIRTypeMin")
                .LessThanOrEqualTo(2)
                .WithMessage("ValidatorTIRTypeMax") ;

            RuleFor(x => x.PackingListState)
                .NotNull()
                .WithMessage("ValidatorPackingListStateEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorPackingListStateMin")
                .LessThanOrEqualTo(3)
                .WithMessage("ValidatorPackingListStateMax");

            RuleFor(x => x.SalesType)
                .NotNull()
                .WithMessage("ValidatorSalesTypeEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorSalesTypeMin")
                .LessThanOrEqualTo(2)
                .WithMessage("ValidatorSalesTypeMax");

            RuleFor(x => x.DeliveryDate)
              .NotEmpty()
              .WithMessage("ValidatorDeliveryDate");


            RuleFor(x => x.LoadingDate)
              .NotEmpty()
              .WithMessage("ValidatorLoadingDate");


            RuleFor(x => x.PaymentDate)
              .NotEmpty()
              .WithMessage("ValidatorPaymentDate");


            RuleFor(x => x.BillDate)
              .NotEmpty()
              .WithMessage("ValidatorBillDate");

        }
    }
}
