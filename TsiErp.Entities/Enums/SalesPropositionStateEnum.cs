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
        [Display(Name = "Beklemede")]
        Beklemede = 1,
        [Display(Name = "Onaylandı")]
        Onaylandı = 2,
        [Display(Name = "Sipariş")]
        Siparis = 3,
        [Display(Name = "Kısmi Sipariş")]
        KismiSiparis = 4,
        [Display(Name = "Kısmi Onaylandı")]
        KismiOnaylandi = 5
    }
}
