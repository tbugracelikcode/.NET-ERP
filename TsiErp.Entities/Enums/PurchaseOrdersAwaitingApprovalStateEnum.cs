using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum PurchaseOrdersAwaitingApprovalStateEnum
    {
        [Display(Name = "EnumAwaitingQualityControlApproval")]
        KaliteKontrolOnayBekliyor = 1,
        [Display(Name = "EnumPartialApproval")]
        SartliOnaylandi = 2,
        [Display(Name = "EnumQualityControlApproved")]
        KaliteKontrolOnayVerildi = 3,
        [Display(Name = "EnumReject")]
        Red = 4,
    }
}
