using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;

namespace TsiErp.Entities.Entities.PaymentPlan 
{
    public class PaymentPlans : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Günler
        /// </summary>
        public int Days_ { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Gecikme Vade Farkı
        /// </summary>
        public decimal DelayMaturityDifference { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
