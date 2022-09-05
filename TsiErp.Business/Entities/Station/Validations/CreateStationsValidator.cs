using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.Station.Dtos;

namespace TsiErp.EntityContracts.Station
{
    public class CreateStationsValidator : TsiAbstractValidatorBase<CreateStationsDto>
    {
        public CreateStationsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen makina kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Makina kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen makina açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Makina açıklaması 200 karakterden fazla olamaz.");

            RuleFor(x => x.GroupID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen makina grubunu seçin.");
                
            

        }
    }
}
