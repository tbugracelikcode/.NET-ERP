using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Validations
{
    public class UpdateCustomerComplaintItemsValidator : TsiAbstractValidatorBase<UpdateCustomerComplaintItemsDto>
    {
        public UpdateCustomerComplaintItemsValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Lütfen müşteri şikayet kodunu yazın.")
                .MaximumLength(17)
                .WithMessage("Müşteri şikayet kodu 17 karakterden fazla olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Lütfen müşteri şikayet açıklamasını yazın.")
                .MaximumLength(200)
                .WithMessage("Müşteri şikayet açıklaması 200 karakterden fazla olamaz."); ;

        }
    }
}
