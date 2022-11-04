using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesOrderLine.Dtos
{
    public class CreateSalesOrderLinesDto : FullAuditedEntityDto
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
        public DateTime WorkOrderCreationDate { get; set; }
    }
}
