using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Validations
{
    public class CreateOperationUnsuitabilityReportsValidator :  TsiAbstractValidatorBase<CreateOperationUnsuitabilityReportsDto>
    {
        public CreateOperationUnsuitabilityReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("Lütfen fiş numarasını yazın.")
                .MaximumLength(17)
                .WithMessage("Fiş numarası, 17 karakterden fazla olamaz.");

            RuleFor(x => x.Date_)
              .NotEmpty()
              .WithMessage("Lütfen tarihi seçin.");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen ürün seçin.");

            RuleFor(x => x.OperationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen operasyon seçin.");

            RuleFor(x => x.EmployeeID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen çalışan seçin.");

            RuleFor(x => x.StationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen istasyon seçin.");

            RuleFor(x => x.StationGroupID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen istasyon grubu seçin.");

            RuleFor(x => x.WorkOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen iş emri seçin.");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen üretim emri seçin.");

        }
    }
}
