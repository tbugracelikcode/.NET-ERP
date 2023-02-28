using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Branch.BusinessRules;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Localizations.Resources.Branches.Page;

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
