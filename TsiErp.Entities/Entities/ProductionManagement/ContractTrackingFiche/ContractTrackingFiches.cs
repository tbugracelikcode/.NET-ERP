using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche
{
    public class ContractTrackingFiches : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Fiş No
        /// </summary>
        public string FicheNr { get; set; }
        [SqlColumnType(  SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Fiş Tarihi
        /// </summary>
        public DateTime FicheDate_ { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Kalite Planı Cari Hesap ID
        /// </summary>
        public Guid QualityPlanCurrentAccountCardID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Tanımı ID
        /// </summary>
        public Guid ContractQualityPlanID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Adet
        /// </summary>
        public int Amount_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Gerçekleşen Adet
        /// </summary>
        public int OccuredAmount_ { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tahmini Geliş Tarihi
        /// </summary>
        public DateTime EstimatedDate_ { get; set; }
    }
}
