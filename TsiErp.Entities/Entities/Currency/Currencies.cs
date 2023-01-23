using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.ExchangeRate;
using TsiErp.Entities.Entities.SalesProposition;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchasePrice;

namespace TsiErp.Entities.Entities.Currency
{
    /// <summary>
    /// Para Birimleri
    /// </summary>
    public class Currencies : FullAuditedEntity
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

        public ICollection<CurrentAccountCards> CurrentAccountCards { get; set; }
        public ICollection<PurchaseRequests> PurchaseRequests { get; set; }
        public ICollection<SalesPropositions> SalesPropositions { get; set; }
        public ICollection<SalesOrders> SalesOrders { get; set; }
        public ICollection<PurchaseOrders> PurchaseOrders { get; set; }
        public ICollection<ExchangeRates> ExchangeRates { get; set; }
        public ICollection<SalesPrices> SalesPrices { get; set; }
        public ICollection<SalesPriceLines> SalesPriceLines { get; set; }
        public ICollection<PurchasePriceLines> PurchasePriceLines { get; set; }
        public ICollection<PurchasePrices> PurchasePrices { get; set; }
    }
}
