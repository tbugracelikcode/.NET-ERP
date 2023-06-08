using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Validations
{
    public class UpdateUnitSetsValidator : TsiAbstractValidatorBase<UpdateUnitSetsDto>
    {
        public UpdateUnitSetsValidator()
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
                .WithMessage("ValidatorNameMaxLenght"); ;

        }
    }
}
