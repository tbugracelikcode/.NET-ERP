using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ProductionTrackingTypesEnum
    {
        [Display(Name = "EnumHalt")]
        Durus = 0, 
        [Display(Name = "EnumInOperation")]
        Operasyonda = 2,
        [Display(Name = "EnumAdjustment")]
        Ayar = 3,
    }
}
