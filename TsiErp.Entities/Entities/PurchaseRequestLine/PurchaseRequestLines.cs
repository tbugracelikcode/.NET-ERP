using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseRequestLine
{
    public class PurchaseRequestLines: FullAuditedEntity
    {
        /// <summary>
        /// Satın Alma Talep ID
        /// </summary>
        public Guid PurchaseRequestID { get; set; }
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
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        /// <summary>
        /// Satış Teklif Satırı Durumu
        /// </summary>
        public PurchaseRequestLineStateEnum PurchaseRequestLineState { get; set; }
        /// <summary>
        /// Siparişe Çevirilme Tarihi
        /// </summary>
        public DateTime? OrderConversionDate { get; set; }

    }
}
