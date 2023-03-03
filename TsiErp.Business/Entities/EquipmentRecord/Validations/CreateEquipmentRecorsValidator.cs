using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.Business.Entities.EquipmentRecord.Validations
{
    public class CreateEquipmentRecorsValidator : TsiAbstractValidatorBase<CreateEquipmentRecordsDto>
    {
        public CreateEquipmentRecorsValidator()
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

            RuleFor(x => x.Department)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("ValidatorDepartmentID");
        }
    }
}
