using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Validation.Validations.FluentValidation;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Validations
{
    public class UpdatePeriodsValidator : TsiAbstractValidatorBase<UpdatePeriodsDto>
    {
        public UpdatePeriodsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen dönem kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Dönem kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen dönem adını yazın.")
                .MaximumLength(200)
                .WithMessage("Dönem adı 200 karakterden fazla olamaz.");

            RuleFor(x => x.BranchID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen döneme ait şubeyi seçin.");
        }
    }
}
