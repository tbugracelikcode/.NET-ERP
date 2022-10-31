using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum ShiftLinesTypeEnum
    {
        [Display(Name = "Çalışma")]
        Calisma = 1,
        [Display(Name = "Mola")]
        Mola = 2,
        [Display(Name = "Fazla Mesai")]
        FazlaMesai = 3,
        [Display(Name = "Temizlik")]
        Temizlik = 4
    }
}
