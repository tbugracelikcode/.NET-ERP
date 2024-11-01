using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;

namespace TsiErp.Business.Entities.TechnicalDrawing.Validations
{
    public class UpdateTechnicalDrawingsValidator : TsiAbstractValidatorBase<UpdateTechnicalDrawingsDto>
    {
        public UpdateTechnicalDrawingsValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("ValidatorProductID");
            //RuleFor(x => x.CustomerCurrentAccountCardID).NotEmpty().WithMessage("ValidatorCurrentCardID");

            RuleFor(x => x.RevisionNo)
              .NotEmpty()
              .WithMessage("ValidatorCodeEmpty")
              .MaximumLength(50)
              .WithMessage("ValidatorCodeMaxLength"); ;
        }
    }
}
//Must(x => x.HasValue && x.Value != Guid.Empty) ===> ProductID için
//.Must(x => x.HasValue && x.Value != Guid.Empty) ===> CustomerCurrentAccountCardID için