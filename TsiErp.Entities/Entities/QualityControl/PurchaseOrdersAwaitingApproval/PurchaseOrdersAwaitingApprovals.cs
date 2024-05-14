using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval
{
    public class PurchaseOrdersAwaitingApprovals : FullAuditedEntity
    {
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid PurchaseOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Satın Alma Sipariş Satır ID
        /// </summary>
        public Guid PurchaseOrderLineID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok Giriş Hareketi ID
        /// </summary>
        public Guid ProductReceiptTransactionID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Onaylayan ID
        /// </summary>
        public Guid ApproverID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kontrol Adedi
        /// </summary>
        public decimal ControlQuantity { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Kontrol Adedi
        /// </summary>
        public DateTime QualityApprovalDate { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Stok Giriş Hareketi ID
        /// </summary>
        public PurchaseOrdersAwaitingApprovalStateEnum PurchaseOrdersAwaitingApprovalStateEnum { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Onaylanan Adedi
        /// </summary>
        public decimal ApprovedQuantity { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama 
        /// </summary>
        public string Description_ { get; set; }
    }
}
