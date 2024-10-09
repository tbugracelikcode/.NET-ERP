using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum CPRsIncotermEnum
    {
        [Display(Name = "FCAEnum")]
        FCA = 0,
        [Display(Name = "DAPEnum")]
        DAP = 1,
    }
}
