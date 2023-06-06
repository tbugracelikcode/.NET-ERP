using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.ShippingAdress;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.CurrentAccountCard
{
    /// <summary>
    /// Cari Hesap Kartları
    /// </summary>
    public class CurrentAccountCards : FullAuditedEntity
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
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
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

    }
}
