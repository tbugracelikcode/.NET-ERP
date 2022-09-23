using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionOrderChangeItem.Dtos;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.Validations
{
    public class UpdateProductionOrderChangeItemsValidator : TsiAbstractValidatorBase<UpdateProductionOrderChangeItemsDto>
    {
        public UpdateProductionOrderChangeItemsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen üretim emri değişiklik kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Üretim emri değişiklik kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen üretim emri değişiklik adını yazın.")
                .MaximumLength(200)
                .WithMessage("Üretim emri değişiklik adı 200 karakterden fazla olamaz."); ;

        }
    }
}
