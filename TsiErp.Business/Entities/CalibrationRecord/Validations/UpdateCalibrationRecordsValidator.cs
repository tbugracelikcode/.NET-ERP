using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;

namespace TsiErp.Business.Entities.CalibrationRecord.Validations
{
    public class UpdateCalibrationRecordsValidator : TsiAbstractValidatorBase<UpdateCalibrationRecordsDto>
    {
        public UpdateCalibrationRecordsValidator()
        {
            RuleFor(x => x.Code)
               .NotEmpty()
               .WithMessage("Lütfen kalibrasyon doğrulama kodunu yazın.")
               .MaximumLength(17)
               .WithMessage("Kalibrasyon doğrulama kodu, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen kalibrasyon doğrulama açıklaması yazın.")
                .MaximumLength(200)
                .WithMessage("Kalibrasyon doğrulama açıklaması, 200 karakterden fazla olamaz.");

            RuleFor(x => x.EquipmentID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ekipman seçin.");

        }
    }
}
