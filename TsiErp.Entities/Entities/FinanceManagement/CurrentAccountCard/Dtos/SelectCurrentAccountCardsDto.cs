using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos
{
    public class SelectCurrentAccountCardsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Cari Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Büyük Koli KG
        /// </summary>
        public decimal BigPackageKG { get; set; }
        /// <summary>
        /// EORİ No
        /// </summary>
        public string EORINr { get; set; }
        /// <summary>
        /// Küçük Koli KG
        /// </summary>
        public decimal SmallPackageKG { get; set; }
        /// <summary>
        /// Yazılım Firma Bilgisi
        /// </summary>
        public bool IsSoftwareCompanyInformation { get; set; }
        /// <summary>
        /// Tedarikçi No
        /// </summary>
        public string SupplierNo { get; set; }
        /// <summary>
        /// Sevkiyat Adresi
        /// </summary>
        public string ShippingAddress { get; set; }
        /// <summary>
        /// Tür
        /// </summary>
        public int Type_ { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
        /// <summary>
        /// Adres1
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// Adres2
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// İlçe
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// Şehir
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Ülke
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Posta Kodu
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Tel1
        /// </summary>
        public string Tel1 { get; set; }
        /// <summary>
        /// Tel2
        /// </summary>
        public string Tel2 { get; set; }
        /// <summary>
        /// Faks
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// İlgili
        /// </summary>
        public string Responsible { get; set; }
        /// <summary>
        /// EPosta
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Web
        /// </summary>
        public string Web { get; set; }
        /// <summary>
        /// Özel Kod1
        /// </summary>
        public string PrivateCode1 { get; set; }
        /// <summary>
        /// Özel Kod2
        /// </summary>
        public string PrivateCode2 { get; set; }
        /// <summary>
        /// Özel Kod3
        /// </summary>
        public string PrivateCode3 { get; set; }
        /// <summary>
        /// Özel Kod4 
        /// </summary>
        public string PrivateCode4 { get; set; }
        /// <summary>
        /// Özel Kod5
        /// </summary>
        public string PrivateCode5 { get; set; }
        /// <summary>
        /// Şahıs Şirketi
        /// </summary>
        public bool SoleProprietorship { get; set; }
        /// <summary>
        /// TC Kimlik No
        /// </summary>
        public string IDnumber { get; set; }
        /// <summary>
        /// Vergi Dairesi
        /// </summary>
        public string TaxAdministration { get; set; }
        /// <summary>
        /// Vergi Numarası
        /// </summary>
        public string TaxNumber { get; set; }
        /// <summary>
        /// Kaplama Müşterisi
        /// </summary>
        public bool CoatingCustomer { get; set; }
        /// <summary>
        /// Satış Sözleşmesi
        /// </summary>
        public string SaleContract { get; set; }
        /// <summary>
        /// Artı Yüzde Oran
        /// </summary>
        public int PlusPercentage { get; set; }
        /// <summary>
        /// Para Birimi 
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Para Birimi ID 
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Tedarikçi
        /// </summary>
        public bool Supplier { get; set; }
        /// <summary>
        /// Fason Tedarikçi
        /// </summary>
        public bool ContractSupplier { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Fason Günlük Çalışma Kapasitesi (Saniye)
        /// </summary>
        public int ContractDailyWorkingCapacity { get; set; }
        /// <summary>
        /// İş İstasyonu Sayısı
        /// </summary>
        public int NumberOfStations { get; set; }
    }
}
