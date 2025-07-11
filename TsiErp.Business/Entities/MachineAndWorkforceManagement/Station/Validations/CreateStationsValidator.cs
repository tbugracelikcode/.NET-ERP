﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;

namespace TsiErp.EntityContracts.Station
{
    public class CreateStationsValidator : TsiAbstractValidatorBase<CreateStationsDto>
    {
        public CreateStationsValidator()
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

            RuleFor(x => x.GroupID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorStationGroupID");
                
            

        }
    }
}
