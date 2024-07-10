using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos
{
    public class PackingListReportDto
    {
        public string FaturaNo { get; set; }

        public string CekiListesiNo { get; set; }

        public string FaturaTarihi { get; set; }

        public string SiparisNo { get; set; }

        public string EoriNr { get; set; }

        public string AliciUnvan { get; set; }

        public string AliciAdres1 { get; set; }

        public string AliciAdres2 { get; set; }

        public string AliciTel { get; set; }

        public string AliciFax { get; set; }

        public string PaketNo { get; set; }

        public string MalzemeTanimi { get; set; }

        public string PaketCinsi { get; set; }

        public int PaketIcerigi { get; set; }

        public int PaketSayisi { get; set; }

        public int ToplamAdet { get; set; }

        public decimal KoliNetKg { get; set; }

        public decimal KoliBrutKg { get; set; }

        public decimal KoliToplamNetKg { get; set; }

        public decimal KoliToplamBrutKg { get; set; }

        public int ToplamPaletAdedi { get; set; }

        public decimal ToplamHacim { get; set; }

        public string SevkiyatAdresi { get; set; }

    }
}
