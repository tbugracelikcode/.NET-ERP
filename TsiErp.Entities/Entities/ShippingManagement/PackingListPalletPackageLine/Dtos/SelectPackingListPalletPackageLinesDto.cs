using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos
{
    public class SelectPackingListPalletPackageLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid PackingListID { get; set; }
        /// <summary>
        /// Paket Fiş ID
        /// </summary>
        public Guid PackageFicheID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Paket No
        /// </summary>
        public string PackageNo { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Satış Sipariş Satır ID
        /// </summary>
        public Guid? SalesOrderLineID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        ///  Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        ///  Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        ///  Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Müşteri ID
        /// </summary>
        public Guid? CustomerID { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Paket Cinsi
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// Paket İçeriği
        /// </summary>
        public int PackageContent { get; set; }
        /// <summary>
        /// Paket Sayısı
        /// </summary>
        public int NumberofPackage { get; set; }
        /// <summary>
        /// Toplam Adet
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// Bir Koli Net KG
        /// </summary>
        public decimal OnePackageNetKG { get; set; }
        /// <summary>
        /// Bir Koli Brüt KG
        /// </summary>
        public decimal OnePackageGrossKG { get; set; }
        /// <summary>
        /// Toplam Net KG
        /// </summary>
        public decimal TotalNetKG { get; set; }
        /// <summary>
        /// Toplam Brüt KG
        /// </summary>
        public decimal TotalGrossKG { get; set; }
    }
}
