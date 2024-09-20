using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord.Dtos;

namespace TsiErp.Business.Entities.CostManagement.StandartStationCostRecord.Validations
{
    public class CreateStandartStationCostRecordsValidator : TsiAbstractValidatorBase<CreateStandartStationCostRecordsDto>
    {
        public CreateStandartStationCostRecordsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.StationID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorStationID");
            RuleFor(x => x.CurrencyID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrencyID");


            RuleFor(x => x.StartDate)
               .NotEmpty()
               .WithMessage("ValidatorStartDateEmpty");

            RuleFor(x => x.EndDate)
               .NotEmpty()
               .WithMessage("ValidatorEndDateEmpty")
               .Must((model, endDate) => endDate > model.StartDate)
               .WithMessage("ValidatorEndDateBeforeStartDate");


            RuleFor(x => x.StationCost)
                .NotNull()
                .WithMessage("ValidatorStationCostEmpty")
                .GreaterThanOrEqualTo(1)
                .WithMessage("ValidatorStationCostMin");


        }

    
}
}
