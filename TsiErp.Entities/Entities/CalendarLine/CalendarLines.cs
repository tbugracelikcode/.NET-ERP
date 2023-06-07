using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.CalendarLine
{
    /// <summary>
    /// Çalışma takvimi satırları
    /// </summary>
    public class CalendarLines : FullAuditedEntity
    {

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal ShiftOverTime { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Vardiya Süresi
        /// </summary>
        public decimal ShiftTime { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Planlanan Duruş Süresi
        /// </summary>
        public decimal PlannedHaltTimes { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Çalışılabilir Süre
        /// </summary>
        public decimal AvailableTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Çalışma takvimi ID
        /// </summary>
        public Guid CalendarID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
    }
}
