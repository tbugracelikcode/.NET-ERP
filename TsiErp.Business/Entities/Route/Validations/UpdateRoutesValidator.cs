using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Route.Dtos;

namespace TsiErp.Business.Entities.Route.Validations
{
    public class UpdateRoutesValidator : TsiAbstractValidatorBase<UpdateRoutesDto>
    {
        public UpdateRoutesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen rota kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Rota kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen rota açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Rota açıklaması 200 karakterden fazla olamaz."); ;
        }
    }
}
