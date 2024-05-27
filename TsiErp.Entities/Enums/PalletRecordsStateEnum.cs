using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PalletRecordsStateEnum
    {

        [Display(Name = "EnumPreparing")]
        Hazirlaniyor = 1,
        [Display(Name = "EnumCompleted")]
        Tamamlandi = 2,
        [Display(Name = "EnumApproved")]
        Onaylandi = 3
    }
}
