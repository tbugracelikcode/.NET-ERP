using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos
{
    public class SelectPurchaseOrdersAwaitingApprovalsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
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
        /// Kontrol Adedi
        /// </summary>
        public decimal ControlQuantity { get; set; }
        /// <summary>
        /// Onaylayan Adı
        /// </summary>
        public Guid? ApproverID { get; set; }
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Fiş No
        /// </summary>
        public string PurchaseOrderFicheNo { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Tarihi
        /// </summary>
        public DateTime PurchaseOrderDate { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Satır ID
        /// </summary>
        public Guid? PurchaseOrderLineID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Cari Hesap Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Hesap Ünvanı
        /// </summary>
        public string CurrentAccountCardName { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Stok Giriş Hareketi ID
        /// </summary>
        public Guid? ProductReceiptTransactionID { get; set; }
        /// <summary>
        /// Stok Giriş Hareketi Durum
        /// </summary>
        public PurchaseOrdersAwaitingApprovalStateEnum PurchaseOrdersAwaitingApprovalStateEnum { get; set; }
        /// <summary>
        /// Stok Giriş Hareketi Durum Adı
        /// </summary>
        public string PurchaseOrdersAwaitingApprovalStateEnumName { get; set; }

        [NoDatabaseAction]
        public List<SelectPurchaseOrdersAwaitingApprovalLinesDto> SelectPurchaseOrdersAwaitingApprovalLines { get; set; }
    }
}
