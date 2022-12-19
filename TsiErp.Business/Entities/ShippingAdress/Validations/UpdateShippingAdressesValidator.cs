using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;

namespace TsiErp.Business.Entities.ShippingAdress.Validations
{
    public class UpdateShippingAdressesValidator : TsiAbstractValidatorBase<UpdateShippingAdressesDto>
    {
        public UpdateShippingAdressesValidator()
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

            RuleFor(x => x.Adress1)
              .NotEmpty()
              .WithMessage("Lütfen adresi yazın.");

            RuleFor(x => x.District)
                .NotEmpty()
                .WithMessage("Lütfen ilçeyi yazın.")
                .MaximumLength(50)
                .WithMessage("İlçe, 50 karakterden fazla olamaz.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Lütfen şehri yazın.")
                .MaximumLength(50)
                .WithMessage("Şehir, 50 karakterden fazla olamaz.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Lütfen ülkeyi yazın.")
                .MaximumLength(50)
                .WithMessage("Ülke, 50 karakterden fazla olamaz.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Lütfen telefon numarasını yazın.")
                .MaximumLength(100)
                .WithMessage("Telefon numarası, 100 karakterden fazla olamaz.");

            RuleFor(x => x.PostCode)
               .NotEmpty()
               .WithMessage("Lütfen posta kodunu yazın.")
               .MaximumLength(50)
               .WithMessage("Posta kodu, 50 karakterden fazla olamaz.");

        }
    }
}
