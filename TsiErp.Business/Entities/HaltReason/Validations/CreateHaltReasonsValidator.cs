using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.HaltReason.Dtos;

namespace TsiErp.Business.Entities.HaltReason.Validations
{
    public class CreateHaltReasonsValidator : TsiAbstractValidatorBase<CreateHaltReasonsDto>
    {
        public CreateHaltReasonsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen duruş sebebi kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Duruş sebebi kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen duruş sebebi adını yazın.")
                .MaximumLength(200)
                .WithMessage("Duruş sebebi adı 200 karakterden fazla olamaz.");

        }
    }
}
