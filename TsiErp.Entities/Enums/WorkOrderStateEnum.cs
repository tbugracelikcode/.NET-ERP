using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum WorkOrderStateEnum
    {
        [Display(Name = "Başlamadı")]
        Baslamadi = 1,
        [Display(Name = "Durduruldu")]
        Durduruldu = 2,
        [Display(Name = "İptal")]
        Iptal = 3,
        [Display(Name = "Devam Ediyor")]
        DevamEdiyor = 4,
        [Display(Name = "Tamamlandı")]
        Tamamlandi = 5
    }
}
