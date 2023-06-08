using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Branch.Validations
{
    public class CreateBranchesValidator : TsiAbstractValidatorBase<CreateBranchesDto>
    {
        public CreateBranchesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("ValidatorNameEmpty")
                .MaximumLength(200)
                .WithMessage("ValidatorNameMaxLenght");
        }
    }
}
