using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PackingListSalesTypeEnum
    {
        [Display(Name = "EnumFOB")]
        FOBİstanbul = 1,
        [Display(Name = "EnumExWorks")]
        ExWorksİstanbul = 2,
    }
}
