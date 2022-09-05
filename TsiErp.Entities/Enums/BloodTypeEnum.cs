using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TsiErp.Entities.Enums
{
    public enum BloodTypeEnum
    {
        [Display(Name = "0 Rh(+)")]
        SifirRhPozitif = 1,
        [Display(Name = "0 Rh(-)")]
        SifirRhNegatif = 2,
        [Display(Name = "AB Rh(+)")]
        AbRhPozitif = 3,
        [Display(Name = "AB Rh(-)")]
        AbRhNegatif = 4,
        [Display(Name = "A Rh(+)")]
        ARhPozitif = 5,
        [Display(Name = "A Rh(-)")]
        ARhNegatif = 6,
        [Display(Name = "B Rh(+)")]
        BRhPozitif = 7,
        [Display(Name = "B Rh(-)")]
        BRhNegatif = 8
    }
}
