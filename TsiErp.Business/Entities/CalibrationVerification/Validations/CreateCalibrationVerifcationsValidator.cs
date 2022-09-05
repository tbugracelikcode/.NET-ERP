using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;

namespace TsiErp.Business.Entities.CalibrationVerification.Validations
{
    public class CreateCalibrationVerifcationsValidator : TsiAbstractValidatorBase<CreateCalibrationVerificationsDto>
    {
        public CreateCalibrationVerifcationsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen kalibrasyon doğrulama kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Kalibrasyon doğrulama kodu, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen kalibrasyon doğrulama açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Kalibrasyon doğrulama açıklaması, 200 karakterden fazla olamaz.");

            RuleFor(x => x.EquipmentID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ekipman seçin.");

        }
    }
}
