using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos
{
    public class UpdatePurchaseOrdersAwaitingApprovalsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Onaylanan Adedi
        /// </summary>
        public decimal ApprovedQuantity { get; set; }
        /// <summary>
        /// Açıklama 
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Kontrol Adedi
        /// </summary>
        public DateTime QualityApprovalDate { get; set; }
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }
        /// <summary>
        /// Kontrol Adedi
        /// </summary>
        public decimal ControlQuantity { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Satır ID
        /// </summary>
        public Guid? PurchaseOrderLineID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Giriş Hareketi ID
        /// </summary>
        public Guid? ProductReceiptTransactionID { get; set; }
        /// <summary>
        /// Onaylayan Adı
        /// </summary>
        public Guid? ApproverID { get; set; }
        /// <summary>
        /// Stok Giriş Hareketi ID
        /// </summary>
        public int PurchaseOrdersAwaitingApprovalStateEnum { get; set; }

        [NoDatabaseAction]
        public List<SelectPurchaseOrdersAwaitingApprovalLinesDto> SelectPurchaseOrdersAwaitingApprovalLines { get; set; }
    }
}
