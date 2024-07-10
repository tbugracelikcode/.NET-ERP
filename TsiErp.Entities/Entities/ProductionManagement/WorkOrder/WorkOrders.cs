using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.WorkOrder
{
    /// <summary>
    /// İş Emirleri
    /// </summary>
    public class WorkOrders : FullAuditedEntity
    {

        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// İptal
        /// </summary>
        public bool IsCancel { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Durum
        /// </summary>
        public WorkOrderStateEnum WorkOrderState { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public decimal AdjustmentAndControlTime { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Gerçekleşen Başlangıç Tarihi
        /// </summary>
        public DateTime? OccuredStartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Gerçekleşen Bitiş Tarihi
        /// </summary>
        public DateTime? OccuredFinishDate { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Gerçekleşen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı İş Emri ID
        /// </summary>
        public Guid? LinkedWorkOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid PropositionID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid StationID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş İstasyonu Grup ID
        /// </summary>
        public Guid StationGroupID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Parçalama Miktarı
        /// </summary>
        public int SplitQuantity { get; set; }

    }
}
