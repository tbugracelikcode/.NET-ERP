using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos
{
    public class SelectSalesOrderDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satış Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
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
        /// Sipariş Durumu
        /// </summary>
        public SalesOrderStateEnum SalesOrderState { get; set; }
        /// <summary>
        /// Bağlı Teklif ID
        /// </summary>
        public Guid LinkedSalesPropositionID { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        /// <summary>
        /// Ödeme Planı Adı
        /// </summary>
        public string PaymentPlanName { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        /// <summary>
        /// Cari Hesap Kartı Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Hesap Kartı Ünvanı
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
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }
        /// <summary>
        /// Sevkiyat Adresi ID
        /// </summary>
        public Guid? ShippingAdressID { get; set; }
        /// <summary>
        /// Sevkiyat Adresi Kodu
        /// </summary>
        public string ShippingAdressCode { get; set; }


        [NoDatabaseAction]
        /// <summary>
        /// Sipariş Satırları
        /// </summary>
        public List<SelectSalesOrderLinesDto> SelectSalesOrderLines { get; set; }
    }
}
