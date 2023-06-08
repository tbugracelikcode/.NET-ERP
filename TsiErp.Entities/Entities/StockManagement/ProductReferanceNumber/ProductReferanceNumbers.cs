using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber
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
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

    }
}
