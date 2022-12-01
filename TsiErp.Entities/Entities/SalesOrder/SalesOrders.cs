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
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesOrder
{
    public class SalesOrders : FullAuditedEntity
    {
        /// <summary>
        /// Satış Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Saat
        /// </summary>
        public string Time_ { get; set; }

        [Precision(18, 6)]
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

        [Precision(18, 6)]
        /// <summary>
        /// Brüt Tutar
        /// </summary>
        public decimal GrossAmount { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// KDV hariç Tutar
        /// </summary>
        public decimal TotalVatExcludedAmount { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// KDV Tutar
        /// </summary>
        public decimal TotalVatAmount { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Toplam İndirimli Tutar
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        [Precision(18, 6)]
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

        public ICollection<SalesOrderLines> SalesOrderLines { get; set; }

        public ICollection<ProductionOrders> ProductionOrders { get; set; }
    }
}
