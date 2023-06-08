using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction
{
    /// <summary>
    /// Bakım Talimatları
    /// </summary>
    public class MaintenanceInstructions : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Bakım Talimat Adı
        /// </summary>
        public string InstructionName { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bakım Periyodu ID
        /// </summary>
        public Guid PeriodID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Planlanan Bakım Süresi
        /// </summary>
        public decimal PlannedMaintenanceTime { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Not
        /// </summary>
        public string Note_ { get; set; }
    }
}
