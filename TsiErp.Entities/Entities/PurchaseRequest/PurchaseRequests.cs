using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseRequest
{
    /// <summary>
    /// Satın Alma Talepleri
    /// </summary>
    public class PurchaseRequests : FullAuditedEntity
    {
        /// <summary>
        /// Talep Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Saat
        /// </summary>
        public TimeSpan? Time_ { get; set; }

        /// <summary>
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Özel Kod
        /// </summary>
        public string SpecialCode { get; set; }
        /// <summary>
        /// Teklif Revizyon No
        /// </summary>
        public string PropositionRevisionNo { get; set; }
        /// <summary>
        /// Revizyon Tarihi
        /// </summary>
        public DateTime? RevisionDate { get; set; }
        ///<summary>
        /// Revizyon Saati
        /// </summary>
        public string RevisionTime { get; set; }
        /// <summary>
        /// Satın Alma Talep Durumu
        /// </summary>
        public PurchaseRequestStateEnum PurchaseRequestState { get; set; }
        /// <summary>
        /// Bağlı Teklif ID
        /// </summary>
        public Guid LinkedPurchaseRequestID { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }
        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }


        /// <summary>
        /// Brüt Tutar
        /// </summary>
        public decimal GrossAmount { get; set; }

        /// <summary>
        /// KDV hariç Tutar
        /// </summary>
        public decimal TotalVatExcludedAmount { get; set; }

        /// <summary>
        /// KDV Tutar
        /// </summary>
        public decimal TotalVatAmount { get; set; }

        /// <summary>
        /// Toplam İndirimli Tutar
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>
        /// Net Tutar
        /// </summary>
        public decimal NetAmount { get; set; }
        /// <summary>
        /// Geçerlilik Tarihi
        /// </summary>
        public DateTime ValidityDate_ { get; set; }
        /// <summary>
        /// Sevkiyat Adresi ID
        /// </summary>
        public Guid? ShippingAdressID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }


        /// <summary>
        /// Ödeme Planı
        /// </summary>
        public PaymentPlans PaymentPlan { get; set; }
        /// <summary>
        /// Şube
        /// </summary>
        public Branches Branches { get; set; }
        /// <summary>
        /// Depo
        /// </summary>        
        public Warehouses Warehouses { get; set; }
        /// <summary>
        /// Para Birimi
        /// </summary>
        public Currencies Currencies { get; set; }
        /// <summary>
        /// Cari Hesap Kartları
        /// </summary>
        public CurrentAccountCards CurrentAccountCards { get; set; }
        /// <summary>
        /// Sevkiyat Adresleri
        /// </summary>
        public ShippingAdresses ShippingAdresses { get; set; }

        public ICollection<PurchaseRequestLines> PurchaseRequestLines { get; set; }
    }
}
