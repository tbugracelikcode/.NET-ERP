using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum StockFicheTypeEnum
    {
        [Display(Name = "Fire Fişi")]
        FireFisi = 11,
        [Display(Name = "Sarf Fişi")]
        SarfFisi = 12,
        [Display(Name = "Üretimden Giriş Fişi")]
        UretimdenGirisFisi = 13,
        [Display(Name = "Depo Sevk Fişi")]
        DepoSevkFisi = 25,
        [Display(Name = "Stok Giriş Fişi")]
        StokGirisFisi = 50,
        [Display(Name = "Stok Çıkış Fişi")]
        StokCikisFisi = 51
    }
}
