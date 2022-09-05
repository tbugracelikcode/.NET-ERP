using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Validations
{
    public class UpdateUnitSetsValidator : TsiAbstractValidatorBase<UpdateUnitSetsDto>
    {
        public UpdateUnitSetsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen birim seti kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Birim seti kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen birim seti adını yazın.")
                .MaximumLength(200)
                .WithMessage("Birim seti adı 200 karakterden fazla olamaz."); ;

        }
    }
}
