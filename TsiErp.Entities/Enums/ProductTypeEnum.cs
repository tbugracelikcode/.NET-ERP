using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ProductTypeEnum
    {
        [Display(Name = "EnumCommercialProduct")]
        TM = 1,
        [Display(Name = "EnumMaterial")]
        HM = 10,
        [Display(Name = "EnumSemiProduct")]
        YM = 11,
        [Display(Name = "EnumProduct")]
        MM = 12,
        [Display(Name = "EnumSparePart")]
        BP = 30,
        [Display(Name = "EnumKit")]
        TK = 40,
        [Display(Name = "EnumMold")]
        KLP = 50,
        [Display(Name = "EnumAparatus")]
        APRT = 60
    }
}
