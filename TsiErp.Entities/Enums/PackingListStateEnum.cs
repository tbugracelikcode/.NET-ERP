using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PackingListStateEnum
    {
        [Display(Name = "EnumPreparing")]
        Hazırlanıyor = 1,
        [Display(Name = "EnumCompleted")]
        Tamamlandı = 2,
        [Display(Name = "EnumTransferred")]
        SevkEdildi = 3,
    }
}
