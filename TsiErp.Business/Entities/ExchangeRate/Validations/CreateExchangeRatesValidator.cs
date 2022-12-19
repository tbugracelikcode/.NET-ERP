using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;

namespace TsiErp.Business.Entities.ExchangeRate.Validations
{
    public class CreateExchangeRatesValidator : TsiAbstractValidatorBase<CreateExchangeRatesDto>
    {
        public CreateExchangeRatesValidator()
        {

        }
    }
}
