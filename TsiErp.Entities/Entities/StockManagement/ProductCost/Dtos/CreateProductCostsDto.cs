namespace TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos
{
    public class CreateProductCostsDto
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
        /// Geçerlilik
        /// </summary>
        public bool IsValid { get; set; }
    }
}
