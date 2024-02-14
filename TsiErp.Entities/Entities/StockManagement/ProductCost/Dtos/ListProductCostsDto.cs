namespace TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos
{
    public class ListProductCostsDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
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
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Birim Maliyet
        /// </summary>
        public decimal UnitCost { get; set; }
        /// <summary>
        /// Reçete Maliyeti
        /// </summary>
        public decimal BillCost { get; set; }
        /// <summary>
        /// Üretim Maliyeti
        /// </summary>
        public decimal ProductionCost { get; set; }
        /// <summary>
        /// Genel Giderler
        /// </summary>
        public decimal Overheads { get; set; }
        /// <summary>
        /// Uygunsuzluk Maliyeti
        /// </summary>
        public decimal UnsuitabilityCost { get; set; }
    }
}
