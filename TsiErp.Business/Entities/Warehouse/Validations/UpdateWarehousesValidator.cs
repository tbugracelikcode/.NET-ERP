using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.WareHouse.Dtos;

namespace TsiErp.Business.Entities.Warehouse.Validations
{
    public class UpdateWarehousesValidator : TsiAbstractValidatorBase<UpdateWarehousesDto>
    {
        public UpdateWarehousesValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen depo kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Depo kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen depo adını yazın.")
                .MaximumLength(200)
                .WithMessage("Depo adı 200 karakterden fazla olamaz."); ;

        }
    }
}
