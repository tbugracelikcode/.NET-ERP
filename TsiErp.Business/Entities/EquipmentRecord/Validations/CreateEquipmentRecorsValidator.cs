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
                .WithMessage("Lütfen ekipman kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Ekipman kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen ekipman adını yazın.")
                .MaximumLength(200)
                .WithMessage("Ekipman adı 200 karakterden fazla olamaz.");

            RuleFor(x => x.Department)
                .Must(x => x.HasValue && x.Value != Guid.Empty)
               .WithMessage("Lütfen departman seçin.");
        }
    }
}
