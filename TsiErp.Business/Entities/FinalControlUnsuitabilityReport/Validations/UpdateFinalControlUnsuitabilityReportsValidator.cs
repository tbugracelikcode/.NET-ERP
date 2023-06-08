using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Validations
{
    public class UpdateFinalControlUnsuitabilityReportsValidator : TsiAbstractValidatorBase<UpdateFinalControlUnsuitabilityReportsDto>
    {
        public UpdateFinalControlUnsuitabilityReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                 .NotEmpty()
                 .WithMessage("ValidatorFicheNoEmpty")
                 .MaximumLength(17)
                 .WithMessage("ValidatorFicheNoMaxLenght");

            RuleFor(x => x.Date_)
              .NotEmpty()
              .WithMessage("ValidatorDate");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.EmployeeID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorEmployeeID");

        }
    }
}
