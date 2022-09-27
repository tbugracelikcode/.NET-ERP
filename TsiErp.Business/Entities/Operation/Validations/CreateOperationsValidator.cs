using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Operation.Dtos;

namespace TsiErp.Business.Entities.Operation.Validations
{
    public class CreateOperationsValidator : TsiAbstractValidatorBase<CreateOperationsDto>
    {
        public CreateOperationsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen operasyon kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("operasyon kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen operasyon açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("operasyon açıklaması 200 karakterden fazla olamaz.");

        }
    }
}
