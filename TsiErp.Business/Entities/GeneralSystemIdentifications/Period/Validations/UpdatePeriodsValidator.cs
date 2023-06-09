using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Validations
{
    public class UpdatePeriodsValidator : TsiAbstractValidatorBase<UpdatePeriodsDto>
    {
        public UpdatePeriodsValidator()
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

            RuleFor(x => x.BranchID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorBranchID");
        }
    }
}
