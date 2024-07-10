using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos
{
    public class PackingListPalletQuantityReportDto
    {
        public int PaletAdet { get; set; }

        public decimal En { get; set; }

        public decimal Boy { get; set; }

        public decimal Yukseklik { get; set; }

        public decimal Kubaj { get; set; }
    }
}
