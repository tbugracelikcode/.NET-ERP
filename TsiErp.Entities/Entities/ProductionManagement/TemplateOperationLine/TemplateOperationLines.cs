using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine
{
    /// <summary>
    /// Operasyon Satırları
    /// </summary>
    public class TemplateOperationLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Şablon Operasyon ID
        /// </summary>
        public Guid TemplateOperationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// İşlem Adet
        /// </summary>
        public int ProcessQuantity { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public int AdjustmentAndControlTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Alternatif
        /// </summary>
        public bool Alternative { get; set; }
    }
}
