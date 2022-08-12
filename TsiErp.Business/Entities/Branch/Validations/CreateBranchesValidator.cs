using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Validation.Validations.FluentValidation;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Validations
{
    public class CreateBranchesValidator : TsiAbstractValidatorBase<CreateBranchesDto>
    {
        public CreateBranchesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen şube kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Şube kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen şube adını yazın.")
                .MaximumLength(200)
                .WithMessage("Şube adı 200 karakterden fazla olamaz."); ;
        }
    }
}
