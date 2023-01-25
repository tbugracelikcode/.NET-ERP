using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.User.Dtos;

namespace TsiErp.EntityContracts.User
{
    public class CreateUsersValidator : TsiAbstractValidatorBase<CreateUsersDto>
    {
        public CreateUsersValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen kullanıcı kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Kullanıcı kodu, 17 karakterden fazla olamaz.");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Lütfen kullanıcı adını yazın.")
                .MaximumLength(300)
                .WithMessage("Kullanıcı adı, 300 karakterden fazla olamaz.");


            RuleFor(x => x.NameSurname)
                .NotEmpty()
                .WithMessage("Lütfen kullanıcı adı ve soyadını yazın.")
                .MaximumLength(300)
                .WithMessage("Kullanıcı adı ve soyadı, 300 karakterden fazla olamaz.");

            RuleFor(x => x.GroupID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen kullanıcı grubunu seçin.");



        }
    }
}
