using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PurchaseOrderLineStateEnum
    {
        [Display(Name = "EnumWaiting")]
        Beklemede = 1,
        [Display(Name = "EnumApproved")]
        Onaylandı = 2,
        [Display(Name = "EnumCompleted")]
        Tamamlandi = 3,
        [Display(Name = "EnumCancel")]
        Iptal = 4,
        [Display(Name = "EnumInPartialCompleted")]
        KismiTamamlandi = 5,
        [Display(Name = "EnumQualityControlApproval")]
        KaliteKontrolOnayiVerildi = 6,
        [Display(Name = "EnumPartialApproved")]
        KismiOnayVerildi = 7
    }
}
