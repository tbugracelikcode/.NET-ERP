using FluentValidation;
using Microsoft.Extensions.Localization;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Branch.Validations
{
    public class UpdateBranchesValidator : TsiAbstractValidatorBase<UpdateBranchesDto>
    {
        public UpdateBranchesValidator(IStringLocalizer<BranchesResource>L)
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
