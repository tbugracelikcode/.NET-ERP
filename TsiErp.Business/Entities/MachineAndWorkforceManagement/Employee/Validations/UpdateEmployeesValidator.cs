using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;

namespace TsiErp.Business.Entities.Employee.Validations
{
    public class UpdateEmployeesValidator : TsiAbstractValidatorBase<UpdateEmployeesDto>
    {
        public UpdateEmployeesValidator()
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

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("ValidatorSurnameEmpty")
                .MaximumLength(100)
                .WithMessage("ValidatorSurnameMaxLength");

            RuleFor(x => x.IDnumber)
                .NotEmpty()
                .WithMessage("ValidatorIDEmpty")
                .MaximumLength(11)
                .WithMessage("ValidatorIDMaxLenght");

            RuleFor(x => x.DepartmentID)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
                .WithMessage("ValidatorDepartmentID");

        }
    }
}
