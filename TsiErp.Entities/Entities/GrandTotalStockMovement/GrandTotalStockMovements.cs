using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GrandTotalStockMovement
{
    /// <summary>
    /// Genel Toplam Stok Hareketleri
    /// </summary>
    public class GrandTotalStockMovements : FullAuditedEntity
    {
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Rezerve Toplamı
        /// </summary>
        public decimal TotalReserved { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Satın Alma Talep Toplamı
        /// </summary>
        public decimal TotalPurchaseRequest { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Satın Alma Sipariş Toplamı
        /// </summary>
        public decimal TotalPurchaseOrder { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Verilen Teklif Toplamı
        /// </summary>
        public decimal TotalSalesProposition { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Satış Sipariş Toplamı
        /// </summary>
        public decimal TotalSalesOrder { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Sarf Toplamı
        /// </summary>
        public decimal TotalConsumption { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Fire Toplamı
        /// </summary>
        public decimal TotalWastage { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Üretim Toplamı
        /// </summary>
        public decimal TotalProduction { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Stok Giriş Toplamı
        /// </summary>
        public decimal TotalGoodsReceipt { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Stok Çıkış Toplamı
        /// </summary>
        public decimal TotalGoodsIssue { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  Şube ID
        /// </summary>
        public Guid BranchID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Stok Miktarı
        /// </summary>
        public decimal Amount { get; set; }

    }
}
