using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;

namespace TsiErp.Business.Entities.ShippingAdress.Validations
{
    public class CreateShippingAdressesValidator : TsiAbstractValidatorBase<CreateShippingAdressesDto>
    {
        public CreateShippingAdressesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen kodu yazın.")
                .MaximumLength(17)
                .WithMessage("Makina kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen açıklamayı yazın.")
                .MaximumLength(200)
                .WithMessage("Açıklama 200 karakterden fazla olamaz.");

            RuleFor(x => x.CustomerCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen müşteri kartını seçin.");



        }
    }
}
