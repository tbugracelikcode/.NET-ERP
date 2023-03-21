using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum BloodTypeEnum
    {
        [Display(Name = "0+")]
        SifirRhPozitif = 1,
        [Display(Name = "0-")]
        SifirRhNegatif = 2,
        [Display(Name = "AB+")]
        AbRhPozitif = 3,
        [Display(Name = "AB-")]
        AbRhNegatif = 4,
        [Display(Name = "A+")]
        ARhPozitif = 5,
        [Display(Name = "A-")]
        ARhNegatif = 6,
        [Display(Name = "B+")]
        BRhPozitif = 7,
        [Display(Name = "B-")]
        BRhNegatif = 8
    }
}
