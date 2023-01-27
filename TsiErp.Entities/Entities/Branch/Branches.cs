using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;

namespace TsiErp.Entities.Entities.Branch
{
    /// <summary>
    /// Şubeler
    /// </summary>
    public class Branches : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İsim
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Dönemler
        /// </summary>
        public ICollection<Periods> Periods { get; set; }
        /// <summary>
        /// Satış Teklifleri
        /// </summary>
        public ICollection<SalesPropositions> SalesPropositions { get; set; }
        public ICollection<SalesOrders> SalesOrders { get; set; }
        public ICollection<PurchaseOrders> PurchaseOrders { get; set; }
        public ICollection<PurchaseRequests> PurchaseRequests { get; set; }
        public ICollection<Forecasts> Forecasts { get; set; }
    }
}
