﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification.Dtos;

namespace TsiErp.Business.Entities.CalibrationVerification.Validations
{
    public class UpdateCalibrationVerificationsValidator : TsiAbstractValidatorBase<UpdateCalibrationVerificationsDto>
    {
        public UpdateCalibrationVerificationsValidator()
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

            RuleFor(x => x.EquipmentID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorEquipmentID");

        }
    }
}
