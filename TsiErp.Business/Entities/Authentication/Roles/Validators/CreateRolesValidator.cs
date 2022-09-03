using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Authentication.Roles.Validators
{
    public class CreateRolesValidator : TsiAbstractValidatorBase<CreateRolesDto>
    {
        public CreateRolesValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage("Lütfen rol adını yazın.");
        }
    }
}
