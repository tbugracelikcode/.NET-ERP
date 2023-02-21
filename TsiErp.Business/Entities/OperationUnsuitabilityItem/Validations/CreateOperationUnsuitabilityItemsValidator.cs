using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Validations
{
    public class CreateOperationUnsuitabilityItemsValidator : TsiAbstractValidatorBase<CreateOperationUnsuitabilityItemsDto>
    {
        public CreateOperationUnsuitabilityItemsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen operasyon uygunsuzluk kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Operasyon uygunsuzluk kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen operasyon uygunsuzluk açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Operasyon uygunsuzluk açıklaması 200 karakterden fazla olamaz."); ;

        }
    }
}
