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
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;
using TsiErp.Entities.Entities.PurchasePrice;

namespace TsiErp.Entities.Entities.Currency
{
    /// <summary>
    /// Para Birimleri
    /// </summary>
    public class Currencies : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
