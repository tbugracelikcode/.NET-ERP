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
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Stok Giriş Hareketi ID
        /// </summary>
        public PurchaseOrdersAwaitingApprovalStateEnum PurchaseOrdersAwaitingApprovalStateEnum { get; set; }
    }
}
