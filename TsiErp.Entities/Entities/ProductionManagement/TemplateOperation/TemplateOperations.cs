using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperation
{
    /// <summary>
    /// Operasyonlar
    /// </summary>
    public class TemplateOperations : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid WorkCenterID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Yüksek Tamir Maliyeti
        /// </summary>
        public bool IsHighRepairCost { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Hassas
        /// </summary>
        public bool IsSensitive { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Fiziksel Zor
        /// </summary>
        public bool IsPhysicallyHard { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Bilgi Gereklilik
        /// </summary>
        public bool IsRequiresKnowledge { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Yetenek Gereklilik
        /// </summary>
        public bool IsRequiresSkill { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Operatöre Riskli
        /// </summary>
        public bool IsRiskyforOperator { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Operatör Uzun Çalışma Süresi
        /// </summary>
        public bool IsLongWorktimeforOperator { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Tespit Edilebilir 
        /// </summary>
        public bool IsCanBeDetected { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Ekstra Maaliyet
        /// </summary>
        public bool IsCauseExtraCost { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// İş Puanı
        /// </summary>
        public int WorkScore { get; set; }






    }
}
