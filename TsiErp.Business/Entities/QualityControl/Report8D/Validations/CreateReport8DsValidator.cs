using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.QualityControl.Report8D.Dtos;

namespace TsiErp.Business.Entities.Report8D.Validations
{
    public class CreateReport8DsValidator : TsiAbstractValidatorBase<CreateReport8DsDto>
    {
        public CreateReport8DsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("ValidatorCodeEmpty")
                .MaximumLength(17)
                .WithMessage("ValidatorCodeMaxLenght");


            RuleFor(x => x.CustomerID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorCustomerID");
            RuleFor(x => x.SupplierID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorSupplierID");
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorProductID");
            //RuleFor(x => x.TechnicalDrawingID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("ValidatorTechnicalDrawingID");

        }
    }
}