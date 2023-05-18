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
    }
}
