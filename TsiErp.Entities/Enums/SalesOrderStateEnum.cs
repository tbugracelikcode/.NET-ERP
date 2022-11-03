using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum SalesOrderStateEnum
    {
        [Display(Name = "Beklemede")]
        Beklemede = 1,
        [Display(Name = "Onaylandı")]
        Onaylandı = 2,
        [Display(Name = "Üretime Verildi")]
        UretimeVerildi = 3,
        [Display(Name = "İptal")]
        Iptal = 4,
        [Display(Name = "Kısmi Üretime Verildi")]
        KismiUretimeVerildi = 5
        
    }
}
