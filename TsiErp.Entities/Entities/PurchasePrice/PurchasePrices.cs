using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.WareHouse;

namespace TsiErp.Entities.Entities.PurchasePrice
{
    public class PurchasePrices : FullAuditedEntity
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
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        public Currencies Currencies { get; set; }
        public Branches Branches { get; set; }
        public Warehouses Warehouses { get; set; }
        public ICollection<PurchasePriceLines> PurchasePriceLines { get; set; }
    }
}
