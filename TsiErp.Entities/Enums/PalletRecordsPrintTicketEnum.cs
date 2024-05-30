using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum PalletRecordsPrintTicketEnum
    {
        [Display(Name = "EnumSmallTicket")]
        KucukEtiket = 1,
        [Display(Name = "EnumBigTicket")]
        BuyukEtiket = 2,
        [Display(Name = "EnumPalletTicket")]
        PaletEtiketi = 3,
    }
}
