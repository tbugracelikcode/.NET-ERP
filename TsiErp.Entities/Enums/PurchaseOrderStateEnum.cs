using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PurchaseOrderStateEnum
    {
        [Display(Name = "EnumWaiting")]
        Beklemede = 1,
        [Display(Name = "EnumApproved")]
        Onaylandı = 2,
        [Display(Name = "EnumInProduction")]
        UretimeVerildi = 3,
        [Display(Name = "EnumCancel")]
        Iptal = 4,
        [Display(Name = "EnumInPartialProduction")]
        KismiUretimeVerildi = 5
    }
}
