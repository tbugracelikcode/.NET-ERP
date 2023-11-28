using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PackingListTIRTypeEnum
    {
        [Display(Name = "EnumCompleteTIR")]
        KompleTır = 1,
        [Display(Name = "EnumPartialTIR")]
        ParsiyelTır = 2,
    }
}
