using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ProductSupplyFormEnum
    {
        [Display(Name = "Satın Alma")]
        Satınalma = 1,
        [Display(Name = "Üretim")]
        Üretim = 2,
    }
}
