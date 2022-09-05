using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.EntityContracts.StationGroup
{
    public class CreateStationGroupsValidator : TsiAbstractValidatorBase<CreateStationGroupsDto>
    {
        public CreateStationGroupsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen istasyon grubu kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("İstasyon grubu kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen istasyon grubu adını yazın.")
                .MaximumLength(200)
                .WithMessage("İstasyon grubu adı 200 karakterden fazla olamaz."); ;

        }
    }
}
