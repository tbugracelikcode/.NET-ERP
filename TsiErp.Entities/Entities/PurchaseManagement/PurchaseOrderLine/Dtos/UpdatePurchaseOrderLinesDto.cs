using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos
{
    public class UpdatePurchaseOrderLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid PurchaseOrderID { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Üretim Emri ID 
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Bağlı Satın Alma Talep Satır ID
        /// </summary>
        public Guid? LikedPurchaseRequestLineID { get; set; }
        /// <summary>
        /// Bağlı Satın Alma Talep ID
        /// </summary>
        public Guid? LinkedPurchaseRequestID { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// İndirim Oranı
        /// </summary>
        public decimal DiscountRate { get; set; }

        ///<summary>
        /// İndirim Tutarı
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

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

        /// <summary>
        /// KDV Tutarı
        /// </summary>
        public decimal VATamount { get; set; }

        /// <summary>
        /// Kur Turarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid? PaymentPlanID { get; set; }
        /// <summary>
        /// Satış Sipariş Satırı Durumu
        /// </summary>
        public int PurchaseOrderLineStateEnum { get; set; }
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }
    }
}
