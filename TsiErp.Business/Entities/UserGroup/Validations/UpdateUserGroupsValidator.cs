using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.UserGroup.Dtos;

namespace TsiErp.EntityContracts.UserGroup
{
    public class UpdateUserGroupsValidator : TsiAbstractValidatorBase<UpdateUserGroupsDto>
    {
        public UpdateUserGroupsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("ValidatorCodeEmpty")
               .MaximumLength(17)
               .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("ValidatorNameEmpty")
                .MaximumLength(300)
                .WithMessage("ValidatorNameMaxLenght"); ;

        }
    }
}
