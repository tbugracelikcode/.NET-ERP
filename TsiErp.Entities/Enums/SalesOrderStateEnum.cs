using System.ComponentModel.DataAnnotations;

namespace TsiErp.Entities.Enums
{
    public enum SalesOrderStateEnum
    {
        [Display(Name = "EnumWaiting")]
        Beklemede = 1,
        [Display(Name = "EnumApproved")]
        Onaylandı = 2,
        [Display(Name = "EnumInProduction")]
        UretimeVerildi = 3,
        [Display(Name = "EnumCancel")]
        Iptal = 4,
        [Display(Name = "EnumInPartialProduction")]
        KismiUretimeVerildi = 5,
        [Display(Name = "EnumAvailableProduction")]
        UretimeVerilebilir = 6

    }
}
