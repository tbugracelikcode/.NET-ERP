using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum OrderAcceptanceRecordStateEnum
    {
        [Display(Name = "EnumPending")]
        Beklemede = 0,
        [Display(Name = "EnumTechnicalApproval")]
        TeknikOnayVerildi = 1,
        [Display(Name = "EnumOrderApproval")]
        SiparisFiyatOnayiVerildi = 2,
    }
}
