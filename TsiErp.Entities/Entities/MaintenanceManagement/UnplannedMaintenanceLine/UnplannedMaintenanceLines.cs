using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine
{
    /// <summary>
    /// Arıza / Plansız Bakım Kayıtları Satırları
    /// </summary>
    public class UnplannedMaintenanceLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Plansız Bakım ID
        /// </summary>
        public Guid UnplannedMaintenanceID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Talimat Açıklaması
        /// </summary>
        public string InstructionDescription { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Bakım Notu
        /// </summary>
        public string MaintenanceNote { get; set; }
    }
}
