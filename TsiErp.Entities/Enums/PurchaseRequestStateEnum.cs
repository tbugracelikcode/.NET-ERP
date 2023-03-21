using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PurchaseRequestStateEnum
    {
        [Display(Name = "EnumWaiting")]
        Beklemede = 1,
        [Display(Name = "EnumApproved")]
        Onaylandı = 2,
        [Display(Name = "EnumPurchase")]
        SatinAlma = 3,
        [Display(Name = "EnumPartialPurchase")]
        KismiSatinAlma = 4,
        [Display(Name = "EnumPartiallyApproved")]
        KismiOnaylandi = 5
    }
}
