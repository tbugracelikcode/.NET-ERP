using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.Station
{
    /// <summary>
    /// İş İstasyonları
    /// </summary>
    public class Stations : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Makine Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Makine Açıklaması
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Marka
        /// </summary>
        public string Brand { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Model
        /// </summary>
        public int Model { get; set; }
        [SqlColumnType(MaxLength = 85, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kapasite
        /// </summary>
        public string Capacity { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// KWA
        /// </summary>
        public decimal KWA { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Grup ID
        /// </summary>
        public Guid GroupID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// X
        /// </summary>
        public decimal X { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Y
        /// </summary>
        public decimal Y { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kapladığı Alan
        /// </summary>
        public decimal AreaCovered { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kullanım Alanı
        /// </summary>
        public decimal UsageArea { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Amortisman
        /// </summary>
        public int Amortization { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Makine Maliyeti
        /// </summary>
        public decimal MachineCost { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Vardiya
        /// </summary>
        public int Shift { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Vardiya Çalışma Süresi
        /// </summary>
        public decimal ShiftWorkingTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Güç Faktörü
        /// </summary>
        public decimal PowerFactor { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Demir Başlar
        /// </summary>
        public bool IsFixtures { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Fason
        /// </summary>
        public bool IsContract { get; set; }

    }
}
