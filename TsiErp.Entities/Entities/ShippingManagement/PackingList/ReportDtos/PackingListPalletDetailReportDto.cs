using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos
{
    public class PackingListPalletDetailReportDto
    {
        public string PaletAdi { get; set; }

        public string IlkKoliNo { get; set; }

        public string SonKoliNo { get; set; }

        public int KoliSayisi { get; set; }
    }
}
