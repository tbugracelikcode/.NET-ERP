﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;

namespace TsiErp.Business.Entities.TechnicalDrawing.Validations
{
    public class CreateTechnicalDrawingsValidator : TsiAbstractValidatorBase<CreateTechnicalDrawingsDto>
    {
        public CreateTechnicalDrawingsValidator()
        {
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");
            //RuleFor(x => x.CustomerCurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentCardID");

            RuleFor(x => x.RevisionNo)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(50)
              .WithMessage("ValidatorCodeMaxLength"); ;
        }
    }
}
