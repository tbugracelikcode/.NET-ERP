using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.ReportDtos
{
    public class RawMaterialRequestFormReportDto
    {
        public string Departman { get; set; }
        public string StokTuru { get; set; }
        public string StokKodu { get; set; }
        public string StokAciklamasi { get; set; }
        public string VaryantKodu { get; set; }
        public string Birim { get; set; }
        public decimal Adet { get; set; }
        public decimal Boy { get; set; }
        public string Aciklama { get; set; }
        public string UretimEmriNo { get; set; }
        public string AnaUrunAdi { get; set; }
        public string UygunsuzlukTuru { get; set; }
        public string StokAdres { get; set; }
        public string TeminSekli { get; set; }
    }
}
