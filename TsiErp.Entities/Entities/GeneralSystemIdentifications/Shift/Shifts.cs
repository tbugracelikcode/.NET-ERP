using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift
{
    /// <summary>
    /// Vardiyalar
    /// </summary>
    public class Shifts : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Vardiya Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Vardiya Açıklaması
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Çalışma Süresi
        /// </summary>
        public decimal TotalWorkTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Mola Süresi
        /// </summary>
        public decimal TotalBreakTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Net Çalışma Süresi
        /// </summary>
        public decimal NetWorkTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal Overtime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Vardiya Sırası
        /// </summary>
        public int ShiftOrder { get; set; }

    }
}
