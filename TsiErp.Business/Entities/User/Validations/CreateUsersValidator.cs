using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;

namespace TsiErp.EntityContracts.User
{
    public class CreateUsersValidator : TsiAbstractValidatorBase<CreateUsersDto>
    {
        public CreateUsersValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("ValidatorUserNameEmpty")
                .MaximumLength(300)
                .WithMessage("ValidatorUserNameMaxLenght");


            RuleFor(x => x.NameSurname)
                .NotEmpty()
                .WithMessage("ValidatorNameSurnameEmpty")
                .MaximumLength(300)
                .WithMessage("ValidatorNameSurnameMaxLength");

            RuleFor(x => x.GroupID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorUserGroupID");



        }
    }
}
