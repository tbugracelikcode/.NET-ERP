using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum StationWorkStateEnum
    {
        [Display(Name = "Duruş")]
        Duruş = 0,
        [Display(Name = "Bakım/Arıza")]
        BakımArıza = 1,
        [Display(Name = "Operasyonda")]
        Operasyonda = 2
    }
}
