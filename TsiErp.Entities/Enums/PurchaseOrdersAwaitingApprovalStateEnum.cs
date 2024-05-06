using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum PurchaseOrdersAwaitingApprovalStateEnum
    {
        [Display(Name = "EnumAwaitingQualityControlApproval")]
        KaliteKontrolOnayBekliyor = 1,
        [Display(Name = "EnumQualityControlApproved")]
        KaliteKontrolOnayVerildi = 2,
    }
}
