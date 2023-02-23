using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Validations
{
    public class UpdateContractUnsuitabilityItemsValidator : TsiAbstractValidatorBase<UpdateContractUnsuitabilityItemsDto>
    {
        public UpdateContractUnsuitabilityItemsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen fason uygunsuzluk kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Fason uygunsuzluk kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen fason uygunsuzluk açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Fason uygunsuzluk açıklaması 200 karakterden fazla olamaz."); ;

        }
    }
}
