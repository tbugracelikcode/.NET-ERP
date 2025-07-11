﻿using FluentValidation;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.ContractTrackingFiche.Validations
{
    public class UpdateContractTrackingFichesValidator : TsiAbstractValidatorBase<UpdateContractTrackingFichesDto>
    {
        public UpdateContractTrackingFichesValidator()
        {
            RuleFor(x => x.FicheNr)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");

            RuleFor(x => x.ProductionOrderID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductionOrderID");

            RuleFor(x => x.CurrentAccountCardID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCurrentAccountCardID");

            RuleFor(x => x.ContractQualityPlanID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorContractQualityPlanID");

            RuleFor(x => x.Amount_)
                .GreaterThan(0).WithMessage("ValidatorAmount");
        }
    }
}
