using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine
{
    public class ProductionTrackingHaltLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Takip ID
        /// </summary>
        public Guid ProductionTrackingID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Duruş Sebebi ID
        /// </summary>
        public Guid HaltID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Duruş Sebebi Kodu
        /// </summary>
        public string HaltCode { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Duruş Sebebi Adı
        /// </summary>
        public string HaltName { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
    }
}
