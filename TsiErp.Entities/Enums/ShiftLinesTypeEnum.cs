using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ShiftLinesTypeEnum
    {
        [Display(Name = "EnumWork")]
        Calisma = 1,
        [Display(Name = "EnumBreak")]
        Mola = 2,
        [Display(Name = "EnumOvertime")]
        FazlaMesai = 3,
        [Display(Name = "EnumCleaning")]
        Temizlik = 4
    }
}
