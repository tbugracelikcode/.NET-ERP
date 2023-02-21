using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Validations
{
    public class UpdatePurchasingUnsuitabilityItemsValidator : TsiAbstractValidatorBase<UpdatePurchasingUnsuitabilityItemsDto>
    {
        public UpdatePurchasingUnsuitabilityItemsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen satın alma uygunsuzluk kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Satın alma uygunsuzluk kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen satın alma uygunsuzluk açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Satın alma uygunsuzluk açıklaması 200 karakterden fazla olamaz."); ;

        }
    }
}
