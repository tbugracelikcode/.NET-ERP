﻿using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Validations
{
    public class CreateOperationUnsuitabilityReportsValidator :  TsiAbstractValidatorBase<CreateOperationUnsuitabilityReportsDto>
    {
        public CreateOperationUnsuitabilityReportsValidator()
        {
            RuleFor(x => x.FicheNo)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.Date_)
              .NotEmpty()
              .WithMessage("ValidatorDate");

            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");

            RuleFor(x => x.OperationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorOperationID");

            RuleFor(x => x.EmployeeID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorEmployeeID");

            RuleFor(x => x.StationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorStationID");

            RuleFor(x => x.StationGroupID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorStationGroupID");

            RuleFor(x => x.WorkOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorWorkOrderID");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductionOrderID");

        }
    }
}
