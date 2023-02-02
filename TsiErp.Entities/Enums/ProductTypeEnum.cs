using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ProductTypeEnum
    {
        [Display(Name = "(TM) Ticari Mal")]
        TM = 1,
        [Display(Name = "(HM) Hammadde")]
        HM = 10,
        [Display(Name = "(YM) Yarı Mamül")]
        YM = 11,
        [Display(Name = "(MM) Mamül")]
        MM = 12,
        [Display(Name = "(BP) Yedek Parça")]
        BP = 30,
        [Display(Name = "(TK) Takım")]
        TK = 40,
        [Display(Name = "(KLP) Kalıp")]
        KLP = 50,
        [Display(Name = "(APRT) Aparat")]
        APRT = 60
    }
}
