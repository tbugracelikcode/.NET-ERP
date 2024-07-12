using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum RowMaterialTypeEnum
    {

        [Display(Name = "EnumMil")]
        MilHammadde = 1,
        [Display(Name = "EnumSac")]
        SacHammadde = 2,
        [Display(Name = "EnumBoru")]
        BoruHammadde = 3,

    }
}
