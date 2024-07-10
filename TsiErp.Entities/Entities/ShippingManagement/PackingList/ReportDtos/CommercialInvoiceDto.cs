using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos
{
    public class CommercialInvoiceDto
    {
        public string CekiListesiNo { get; set; }

        public string CariUnvan { get; set; }

        public string Adres1 { get; set; }

        public string Adres2 { get; set; }

        public string Tel1 { get; set; }

        public string Faks { get; set; }

        public string EoriNr { get; set; }

        public string FaturaNo { get; set; }

        public DateTime FaturaTarhi { get; set; }

        public decimal Adet { get; set; }

        public string StokKodu { get; set; }

        public string StokAciklamasi { get; set; }

        public string VaryantKodu { get; set; }

        public decimal BirimFiyat { get; set; }

        public decimal ToplamTutar { get; set; }

        public string BagliSiparisNo { get; set; }

        public DateTime OdemeTarihi { get; set; }

        public DateTime TeslimTarihi { get; set; }

        public string NakliyeFirmasi { get; set; }

        public string SatisSekli { get; set; }

        public string SevkiyatAdresi { get; set; }

        public Guid SiparisId { get; set; }

        public string BankName { get; set; }

        public string BankBranch { get; set; }

        public string SwiftKodu { get; set; }

        public string EuroAccIbanNo { get; set; }

        public string EuroAccNr { get; set; }

        public string UsdAccIbanNo { get; set; }

        public string UsdAccNr { get; set; }

        public string TlAccIbanNo { get; set; }

        public string TlAccNr { get; set; }

        public string GbpAccIbanNo { get; set; }

        public string GbpAccNr { get; set; }


    }
}
