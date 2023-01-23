using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.SalesPrice.Dtos;

namespace TsiErp.Business.Entities.SalesPrice.Validations
{
    public class UpdateSalesPricesValidatorDto : AbstractValidator<UpdateSalesPricesDto>
    {
        public UpdateSalesPricesValidatorDto()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen fiyat listesi kodunu yazın.")
               .MaximumLength(17)
               .WithMessage("Fiyat listesi kodu, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Lütfen fiyat listesi açıklaması yazın.")
               .MaximumLength(200)
               .WithMessage("Fiyat listesi açıklaması, 200 karakterden fazla olamaz.");

            RuleFor(x => x.CurrencyID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen para birimi seçin.");
        }
    }
}
