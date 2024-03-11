using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos
{
    public class ListStockAddressesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Stok Adres Kodu
        /// </summary>
        public string Code { get; set; }
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
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

    }
}
