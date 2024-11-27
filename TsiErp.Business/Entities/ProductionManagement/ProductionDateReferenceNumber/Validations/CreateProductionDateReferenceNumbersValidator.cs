using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.ProductionDateReferenceNumber.Validations
{
    public class CreateProductionDateReferenceNumbersValidator : TsiAbstractValidatorBase<CreateProductionDateReferenceNumbersDto>
    {

        public CreateProductionDateReferenceNumbersValidator()
        {
            RuleFor(x => x.ProductionDateReferenceNo)
                .NotEmpty()
                .WithMessage("ValidatorProductionDateReferenceNoEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorProductionDateReferenceNoMaxLenght");

        }
    }
}
