using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport
{
    public class ContractUnsuitabilityReports : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Rapor Fiş No
        /// </summary>
        public string FicheNo { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Uygunsuzluk İş Emri Oluşacak
        /// </summary>
        public bool IsUnsuitabilityWorkOrder { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Hata Başlığı ID
        /// </summary>
        public Guid UnsuitabilityItemsID { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Uygun Olmayan Miktar
        /// </summary>
        public decimal UnsuitableAmount { get; set; }

        [SqlColumnType(MaxLength = 100, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Fason Takip Fiş ID
        /// </summary>
        public Guid ContractTrackingFicheID { get; set; }

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
    }
}
