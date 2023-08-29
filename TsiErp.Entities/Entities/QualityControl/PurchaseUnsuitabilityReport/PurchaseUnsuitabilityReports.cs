using System.Data;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport
{
    /// <summary>
    /// Satın Alma Uygunsuzluk Raporları
    /// </summary>
    public class PurchaseUnsuitabilityReports : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Rapor Fiş No
        /// </summary>
        public string FicheNo { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Parti No
        /// </summary>
        public string PartyNo { get; set; }


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Uygun Olmayan Miktar
        /// </summary>
        public decimal UnsuitableAmount { get; set; }

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

        [SqlColumnType(MaxLength = 100, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Hata Başlığı ID
        /// </summary>
        public Guid UnsuitabilityItemsID { get; set; }
    }
}
