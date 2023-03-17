using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TsiErp.Entities.Enums
{
    public enum SalesPropositionStateEnum
    {
        [Display(Name = "EnumWaiting")]
        Beklemede = 1,
        [Display(Name = "EnumApproved")]
        Onaylandı = 2,
        [Display(Name = "EnumOrder")]
        Siparis = 3,
        [Display(Name = "EnumPartialOrder")]
        KismiSiparis = 4,
        [Display(Name = "EnumPartiallyApproved")]
        KismiOnaylandi = 5
    }
}
