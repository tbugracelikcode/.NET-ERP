using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;

namespace TsiErp.Entities.Entities.WareHouse
{
    /// <summary>
    /// Depo
    /// </summary>
    public class Warehouses : FullAuditedEntity
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Satış Teklifleri 
        /// </summary>
        public ICollection<SalesPropositions> SalesPropositions { get; set; }
        public ICollection<SalesOrders> SalesOrders { get; set; }
        public ICollection<PurchaseOrders> PurchaseOrders { get; set; }
        public ICollection<PurchaseRequests> PurchaseRequests { get; set; }
        public ICollection<PurchasePrices> PurchasePrices { get; set; }
        public ICollection<SalesPrices> SalesPrices { get; set; }


    }
}
