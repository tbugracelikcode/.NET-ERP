using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum OperationQuantityInformationsTypeEnum
    {
        [Display(Name = "EnumAdjustment")]
        Ayar = 3,
        [Display(Name = "EnumOperation")]
        Operasyon = 2,
    }
}
