﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.TestManagement.City.Dtos;

namespace TsiErp.Business.Entities.TestManagement.City.Validations
{
    public class UpdateCitiesValidator : TsiAbstractValidatorBase<SelectCitiesDto>
    {

        public UpdateCitiesValidator()
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
        }
    }
}
