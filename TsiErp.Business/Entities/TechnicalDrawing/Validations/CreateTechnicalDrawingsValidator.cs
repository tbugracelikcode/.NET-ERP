using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;

namespace TsiErp.Business.Entities.TechnicalDrawing.Validations
{
    public class CreateTechnicalDrawingsValidator : TsiAbstractValidatorBase<CreateTechnicalDrawingsDto>
    {
        public CreateTechnicalDrawingsValidator()
        {
            RuleFor(x => x.ProductID).Must(x => x.HasValue && x.Value != Guid.Empty).WithMessage("Lütfen stok seçin.");

            RuleFor(x => x.RevisionNo)
              .NotEmpty()
              .WithMessage("Lütfen revizyon numarasını yazın.")
              .MaximumLength(50)
              .WithMessage("Revizyon numarası, 50 karakterden fazla olamaz."); ;
        }
    }
}
