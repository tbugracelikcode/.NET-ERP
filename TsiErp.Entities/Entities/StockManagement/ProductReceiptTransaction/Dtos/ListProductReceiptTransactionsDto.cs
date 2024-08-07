using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos
{
    public class ListProductReceiptTransactionsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }

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
        /// Miktar
        /// </summary>
        public decimal PurchaseOrderQuantity { get; set; }
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
        public ProductReceiptTransactionStateEnum ProductReceiptTransactionStateEnum { get; set; }
        /// <summary>
        /// Tedarikçi Stok Kodu
        /// </summary>
        public string SupplierProductCode { get; set; }
        /// <summary>
        /// Parti No
        /// </summary>
        public string PartyNo { get; set; }
    }
}
