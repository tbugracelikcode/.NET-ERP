using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.RouteLine
{
    /// <summary>
    /// Rota Satırları
    /// </summary>
    public class RouteLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ana Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Havuz ID
        /// </summary>
        public Guid ProductionPoolID { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Üretim Havuz Açıklama
        /// </summary>
        public string ProductionPoolDescription { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public int AdjustmentAndControlTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
    }
}
