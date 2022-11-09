using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesOrderLine
{
    public class SalesOrderLines : FullAuditedEntity
    {
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        /// <summary>
        /// Bağlı Teklif Satır ID
        /// </summary>
        public Guid LikedPropositionLineID { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// İndirim Oranı
        /// </summary>
        public decimal DiscountRate { get; set; }

        [Precision(18, 6)]
        ///<summary>
        /// İndirim Tutarı
        /// </summary>
        public decimal DiscountAmount { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Satır Toplar Tutarı
        /// </summary>
        public decimal LineTotalAmount { get; set; }
        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }
        /// <summary>
        /// KDV Oranı
        /// </summary>
        public int VATrate { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// KDV Tutarı
        /// </summary>
        public decimal VATamount { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Kur Turarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        /// <summary>
        /// Satış Sipariş Satırı Durumu
        /// </summary>
        public SalesOrderLineStateEnum SalesOrderLineStateEnum { get; set; }
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }





        /// <summary>
        /// Stoklar
        /// </summary>
        public Products Products { get; set; }
        /// <summary>
        /// Birim Setleri
        /// </summary>
        public UnitSets UnitSets { get; set; }
        /// <summary>
        /// Ödeme Planları
        /// </summary>
        public PaymentPlans PaymentPlans { get; set; }
        /// <summary>
        /// Satış Siparişi
        /// </summary>
        public SalesOrders SalesOrders { get; set; }
        /// <summary>
        /// Satış Teklifi Satırı
        /// </summary>
        public SalesPropositionLines SalesPropositionLines { get; set; }
    }
}
