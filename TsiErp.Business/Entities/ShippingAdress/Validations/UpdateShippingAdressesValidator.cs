using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;

namespace TsiErp.Business.Entities.ShippingAdress.Validations
{
    public class UpdateShippingAdressesValidator : TsiAbstractValidatorBase<UpdateShippingAdressesDto>
    {
        public UpdateShippingAdressesValidator()
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

            RuleFor(x => x.CustomerCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountID");

            RuleFor(x => x.Adress1)
               .NotEmpty()
               .WithMessage("ValidatorAdress1Empty");

            RuleFor(x => x.District)
                .NotEmpty()
                .WithMessage("ValidatorDistrictEmpty")
                .MaximumLength(50)
                .WithMessage("ValidatorDistrictMaxLength");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("ValidatorCityEmpty")
                .MaximumLength(50)
                .WithMessage("ValidatorCityMaxLength");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("ValidatorCountryEmpty")
                .MaximumLength(50)
                .WithMessage("ValidatorCountryMaxLength");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("ValidatorPhoneEmpty")
                .MaximumLength(100)
                .WithMessage("ValidatorPhoneMaxLength");

            RuleFor(x => x.PostCode)
               .NotEmpty()
               .WithMessage("ValidatorPostEmpty")
               .MaximumLength(50)
               .WithMessage("ValidatorPostMaxLength");

        }
    }
}
