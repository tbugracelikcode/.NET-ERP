using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos
{
    public class SelectPurchaseRequestsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İşlem Dövizi Brüt Tutar
        /// </summary>
        public decimal TransactionExchangeGrossAmount { get; set; }

        /// <summary>
        /// İşlem Dövizi Toplam İndirimli Tutar
        /// </summary>
        public decimal TransactionExchangeTotalDiscountAmount { get; set; }
        /// <summary>
        /// İşlem Dövizi Net Tutar
        /// </summary>
        public decimal TransactionExchangeNetAmount { get; set; }
        /// <summary>
        /// İşlem Dövizi KDV Tutar
        /// </summary>
        public decimal TransactionExchangeTotalVatAmount { get; set; }

        /// <summary>
        /// İşlem Dövizi KDV hariç Tutar
        /// </summary>
        public decimal TransactionExchangeTotalVatExcludedAmount { get; set; }
        /// <summary>
        /// Talep Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// İşlem Dövizi ID
        /// </summary>
        public Guid? TransactionExchangeCurrencyID { get; set; }
        /// <summary>
        /// İşlem Dövizi Kodu
        /// </summary>
        public string TransactionExchangeCurrencyCode { get; set; }
        /// <summary>
        /// MRP ID
        /// </summary>
        public Guid? MRPID { get; set; }
        /// <summary>
        /// MRP Kodu
        /// </summary>
        public string MRPCode { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Saat
        /// </summary>
        public string Time_ { get; set; }
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
        public Guid? LinkedPurchaseRequestID { get; set; }
        /// <summary>
        /// Bağlı Satın Alma Talep Fiş No
        /// </summary>
        public string LinkedPurchaseRequestFicheNo { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Para Birimi Sembol
        /// </summary>
        public string CurrencySymbol { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid? PaymentPlanID { get; set; }
        /// <summary>
        /// Ödeme Planı Adı
        /// </summary>
        public string PaymentPlanName { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Şube Adı
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Depo Adı
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Cari Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string CurrentAccountCardName { get; set; }
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
        /// Sevkiyat Adresi Kodu
        /// </summary>
        public string ShippingAdressCode { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }

        /// <summary>
        ///Fiyatlandırma Dövizi
        /// </summary>
        public PricingCurrencyEnum PricingCurrency { get; set; }
        /// <summary>
        ///Fiyatlandırma Dövizi Adı
        /// </summary>
        public string PricingCurrencyName { get; set; }

        [NoDatabaseAction]
        public List<SelectPurchaseRequestLinesDto> SelectPurchaseRequestLines { get; set; }
    }
}
