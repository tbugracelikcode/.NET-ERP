using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum MRPsStateEnum
    {
        [Display(Name = "EnumTemplate")]
        Taslak = 1,
        [Display(Name = "EnumContinuing")]
        DevamEdiyor = 2,
        [Display(Name = "EnumCompleted")]
        Tamamlandi = 3,
        [Display(Name = "EnumPurchase")]
        SatinAlma = 4,
    }
}
