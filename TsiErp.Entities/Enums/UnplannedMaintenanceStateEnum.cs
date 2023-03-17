using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum UnplannedMaintenanceStateEnum
    {
        [Display(Name = "EnumDone")]
        Yapıldı = 1,
        [Display(Name = "EnumNotDone")]
        Yapılmadı = 2,
    }
}
