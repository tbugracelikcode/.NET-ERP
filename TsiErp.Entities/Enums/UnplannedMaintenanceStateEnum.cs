using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum UnplannedMaintenanceStateEnum
    {
        [Display(Name = "Yapıldı")]
        Yapıldı = 1,
        [Display(Name = "Yapılmadı")]
        Yapılmadı = 2,
    }
}
