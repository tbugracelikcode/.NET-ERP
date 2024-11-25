using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum CompanyChecksStateEnum
    {
        [Display(Name = "EnumOurOwn")]
        KendiCekimiz = 1,
        [Display(Name = "EnumCustomer")]
        MusteriCeki = 2
    }
}
