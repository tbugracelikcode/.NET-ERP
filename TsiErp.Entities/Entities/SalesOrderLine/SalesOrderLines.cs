using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.SalesOrderLine
{
    public class SalesOrderLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Teklif Satır ID
        /// </summary>
        public Guid LikedPropositionLineID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İndirim Oranı
        /// </summary>
        public decimal DiscountRate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        ///<summary>
        /// İndirim Tutarı
        /// </summary>
        public decimal DiscountAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Satır Toplar Tutarı
        /// </summary>
        public decimal LineTotalAmount { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// KDV Oranı
        /// </summary>
        public int VATrate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// KDV Tutarı
        /// </summary>
        public decimal VATamount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kur Turarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satış Sipariş Satırı Durumu
        /// </summary>
        public SalesOrderLineStateEnum SalesOrderLineStateEnum { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }
    }
}
