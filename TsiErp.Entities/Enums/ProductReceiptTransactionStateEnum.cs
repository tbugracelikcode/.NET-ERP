using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ProductReceiptTransactionStateEnum
    {
        [Display(Name = "EnumAwaitingQualityControlApproval")]
        KaliteKontrolOnayBekliyor = 1, 
        [Display(Name = "EnumQualityControlApproved")]
        KaliteKontrolOnayVerildi = 2,
        [Display(Name = "EnumWarehouseApproved")]
        DepoOnayiVerildi = 3,
    }
}
