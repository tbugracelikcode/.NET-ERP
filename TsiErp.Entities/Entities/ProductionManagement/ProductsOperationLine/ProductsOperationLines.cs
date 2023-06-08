using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine
{
    public class ProductsOperationLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// İşlem Adet
        /// </summary>
        public int ProcessQuantity { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public decimal AdjustmentAndControlTime { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Alternatif
        /// </summary>
        public bool Alternative { get; set; }
    }
}
