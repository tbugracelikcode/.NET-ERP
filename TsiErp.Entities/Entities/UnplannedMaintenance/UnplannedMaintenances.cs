using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.UnplannedMaintenance
{
    /// <summary>
    /// Arıza / Plansız Bakım Kayıtları
    /// </summary>
    public class UnplannedMaintenances : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kayıt No
        /// </summary>
        public string RegistrationNo { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Periyot ID
        /// </summary>
        public Guid PeriodID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Durum
        /// </summary>
        public UnplannedMaintenanceStateEnum Status { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Bakımı Yapan
        /// </summary>
        public string Caregiver { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Bakımı Yapan Kişi Sayısı
        /// </summary>
        public int NumberofCaregivers { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kalan Süre
        /// </summary>
        public decimal RemainingTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Not
        /// </summary>
        public string Note_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Planlanan Bakım Süre
        /// </summary>
        public decimal UnplannedTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Gerçekleşen Bakım Süre
        /// </summary>
        public decimal OccuredTime { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Planlanan Bakım Tarihi
        /// </summary>
        public DateTime? UnplannedDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        ///  Tamamlama Tarihi
        /// </summary>
        public DateTime? CompletionDate { get; set; }

    }
}
