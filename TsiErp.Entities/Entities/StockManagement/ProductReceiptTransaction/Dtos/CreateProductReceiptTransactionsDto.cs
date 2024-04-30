using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos
{
    public class CreateProductReceiptTransactionsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }
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
        /// İrsaliye Tarihi
        /// </summary>
        public DateTime WaybillDate { get; set; }
        /// <summary>
        /// İrsaliye Miktarı
        /// </summary>
        public decimal WaybillQuantity { get; set; }
        /// <summary>
        /// İrsaliye Numarası
        /// </summary>
        public string WaybillNo { get; set; }
        /// <summary>
        /// Depo Giriş Miktarı
        /// </summary>
        public decimal WarehouseReceiptQuantity { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public int ProductReceiptTransactionStateEnum { get; set; }
        /// <summary>
        /// Tedarikçi Stok Kodu
        /// </summary>
        public string SupplierProductCode { get; set; }
    }
}
