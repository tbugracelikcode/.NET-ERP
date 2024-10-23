using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice.Dtos
{
    public class CreatePurchaseInvoicesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// İşlem Dövizi ID
        /// </summary>
        public Guid? TransactionExchangeCurrencyID { get; set; }
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
        /// Bağlı Satın Alma Talep ID
        /// </summary>
        public Guid? LinkedPurchaseRequestID { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid? PaymentPlanID { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }

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
        /// MRP ID
        /// </summary>
        public Guid? MRPID { get; set; }
        /// <summary>
        /// Bakım MRP ID
        /// </summary>
        public Guid? MaintenanceMRPID { get; set; }

        /// <summary>
        /// Toplam İndirimli Tutar
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>
        /// Net Tutar
        /// </summary>
        public decimal NetAmount { get; set; }
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }
        /// <summary>
        /// Sevkiyat Adresi ID
        /// </summary>
        public Guid? ShippingAdressID { get; set; }
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceID { get; set; }

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
        ///Fiyatlandırma Dövizi
        /// </summary>
        public int PricingCurrency { get; set; }
        /// <summary>
        /// Sipariş Fatura Durumu
        /// </summary>
        public PurchaseInvoiceStateEnum PurchaseInvoiceState { get; set; }

        /// <summary>
        /// Sipariş Fatura İrsaliye Durumu
        /// </summary>
        public PurchaseInvoiceWayBillStatusEnum PurchaseInvoiceWayBillStatusEnum { get; set; }

        /// <summary>
        /// Fatura Fiyat Onay Durumu
        /// </summary>
        public PurchaseInvoicePriceApprovalStateEnum PriceApprovalState { get; set; }

        [NoDatabaseAction]
        public List<SelectPurchaseInvoiceLinesDto> SelectPurchaseInvoiceLinesDto { get; set; }
    }
}
