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
        [Display(Name = "EnumWastage")]
        FireFisi = 11,
        [Display(Name = "EnumConsume")]
        SarfFisi = 12,
        [Display(Name = "EnumProductionIncome")]
        UretimdenGirisFisi = 13,
        [Display(Name = "EnumWarehouse")]
        DepoSevkFisi = 25,
        [Display(Name = "EnumStockIncome")]
        StokGirisFisi = 50,
        [Display(Name = "EnumStockOutput")]
        StokCikisFisi = 51
    }
}
