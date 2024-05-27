using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PalletRecordsTicketStateEnum
    {
        [Display(Name = "EnumTicketPending")]
        Bekliyor = 1,
        [Display(Name = "EnumTicketCompleted")]
        Tamamlandi = 2
    }
}
