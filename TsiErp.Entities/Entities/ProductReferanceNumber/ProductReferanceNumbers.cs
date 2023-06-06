using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.Product;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductReferanceNumber
{
    /// <summary>
    /// Ürün Referans Numaraları
    /// </summary>
    public class ProductReferanceNumbers : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ürün Referans Numarası
        /// </summary>
        public string ReferanceNo { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

    }
}
