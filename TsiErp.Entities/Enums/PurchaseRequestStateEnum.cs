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
        [Display(Name = "Beklemede")]
        Beklemede = 1,
        [Display(Name = "Onaylandı")]
        Onaylandı = 2,
        [Display(Name = "Satın Alma")]
        SatinAlma = 3,
        [Display(Name = "Kısmi Satın Alma")]
        KismiSatinAlma = 4,
        [Display(Name = "Kısmi Onaylandı")]
        KismiOnaylandi = 5
    }
}
