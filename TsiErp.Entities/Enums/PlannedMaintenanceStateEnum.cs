using System.ComponentModel.DataAnnotations;


namespace TsiErp.Entities.Enums
{
    public enum PlannedMaintenanceStateEnum
    {
        [Display(Name = "Done")]
        Yapıldı = 1,
        [Display(Name = "NotDone")]
        Yapılmadı = 2,
    }
}
