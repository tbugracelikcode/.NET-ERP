using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Validations
{
    public class UpdateFinalControlUnsuitabilityItemsValidator : TsiAbstractValidatorBase<UpdateFinalControlUnsuitabilityItemsDto>
    {
        public UpdateFinalControlUnsuitabilityItemsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen final kontrol uygunsuzluk kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Final kontrol uygunsuzluk kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen final kontrol uygunsuzluk açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Final kontrol uygunsuzluk açıklaması 200 karakterden fazla olamaz."); ;

        }
    }
}
