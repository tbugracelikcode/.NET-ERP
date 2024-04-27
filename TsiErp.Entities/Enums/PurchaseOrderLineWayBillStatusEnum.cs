using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PurchaseOrderLineWayBillStatusEnum
    {
        [Display(Name = "EnumWayBillPending")]
        Beklemede = 1,
        [Display(Name = "EnumWayBillPartialApproved")]
        KismiOnaylandi = 2,
        [Display(Name = "EnumWayBillApproved")]
        Onaylandi = 3
    }
}
