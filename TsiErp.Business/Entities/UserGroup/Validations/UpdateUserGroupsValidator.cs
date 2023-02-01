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
                .WithMessage("Lütfen kullanıcı grubu kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Kullanıcı grubu kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen kullanıcı grubu adını yazın.")
                .MaximumLength(300)
                .WithMessage("Kullanıcı grubu adı 300 karakterden fazla olamaz."); ;

        }
    }
}
