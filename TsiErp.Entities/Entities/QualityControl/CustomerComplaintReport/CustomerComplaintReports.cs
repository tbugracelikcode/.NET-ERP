using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport
{
    public class CustomerComplaintReports : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Rapor No
        /// </summary>
        public string ReportNo { get; set; }

        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Rapor Tarihi
        /// </summary>
        public DateTime? ReportDate { get; set; }

        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Satış Siparişi ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Şikayet Başlığı ID
        /// </summary>
        public Guid UnsuitqabilityItemsID { get; set; }

        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Domain
        /// </summary>
        public string Domain_ { get; set; }

        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Dosya Yolu
        /// </summary>
        public string FilePath { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// 8D Raporu Oluşturulacak
        /// </summary>
        public bool is8DReport { get; set; }

        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Rapor Durumu
        /// </summary>
        public string ReportState { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Sevk Edilen Miktar
        /// </summary>
        public decimal DeliveredQuantity { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Hatalı Miktar
        /// </summary>
        public decimal DefectedQuantity { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Rapor Sonucu
        /// </summary>
        public string ReportResult { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// İmalat Referans Numarası
        /// </summary>
        public string ProductionReferanceNumber { get; set; }
    }
}
