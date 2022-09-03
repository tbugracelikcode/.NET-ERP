using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Department.Dtos;

namespace TsiErp.Business.Entities.Department.Validations
{
    public class CreateDepartmentsValidator : TsiAbstractValidatorBase<CreateDepartmentsDto>
    {
        public CreateDepartmentsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen departman kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Departman kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen departman adını yazın.")
                .MaximumLength(200)
                .WithMessage("Departman adı 200 karakterden fazla olamaz."); ;

        }
    }
}
