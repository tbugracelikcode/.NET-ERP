using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum CompanyChecksPaymentStateEnum
    {
        [Display(Name = "EnumPaid")]
        Odendi = 1,
        [Display(Name = "EnumNotPaid")]
        Odenmedi = 2
    }
}
