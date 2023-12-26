using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum EmployeeScoringsStateEnum
    {
        [Display(Name = "EnumDraft")]
        Taslak = 1,
        [Display(Name = "EnumSaved")]
        Kaydedildi = 2,
        [Display(Name = "EnumApproved")]
        Onaylandi = 3,
    }
}
