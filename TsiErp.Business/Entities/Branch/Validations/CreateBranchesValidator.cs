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
        public CreateBranchesValidator(IStringLocalizer<BranchesResource> L)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(L["ValidatorCodeEmpty"])
                .MaximumLength(17)
                .WithMessage(L["ValidatorCodeMaxLenght"]);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(L["ValidatorNameEmpty"])
                .MaximumLength(200)
                .WithMessage(L["ValidatorNameMaxLenght"]); ;
        }
    }
}
