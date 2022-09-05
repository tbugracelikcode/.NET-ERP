using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Employee.Dtos;

namespace TsiErp.Business.Entities.Employee.Validations
{
    public class UpdateEmployeesValidator : TsiAbstractValidatorBase<UpdateEmployeesDto>
    {
        public UpdateEmployeesValidator()
        {

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen çalışan kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Çalışan kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen çalışan adını yazın.")
                .MaximumLength(200)
                .WithMessage("Çalışan adı 200 karakterden fazla olamaz.");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Lütfen çalışan soyadını yazın.")
                .MaximumLength(100)
                .WithMessage("Çalışan soyadı 100 karakterden fazla olamaz.");

            RuleFor(x => x.IDnumber)
                .NotEmpty()
                .WithMessage("T.C. Kimlik numarasını yazın.")
                .MaximumLength(11)
                .WithMessage("T.C. Kimlik numarası 11 karakter olmalıdır.");

            RuleFor(x => x.DepartmentID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
                .WithMessage("Çalışanın bağlı olduğu departmanı seçmelisiniz.");

        }
    }
}
