using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PricingCurrencyEnum
    {
        [Display(Name = "EnumLocalCurrency")]
        LocalCurrency = 1,
        [Display(Name = "EnumTransactionCurrency")]
        TransactionCurrency = 2
    }
}
