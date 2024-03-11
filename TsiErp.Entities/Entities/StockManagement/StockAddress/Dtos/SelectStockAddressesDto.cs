using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos;

namespace TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos
{
    public class SelectStockAddressesDto : FullAuditedEntityDto
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

        [NoDatabaseAction]
        public List<SelectStockAddressLinesDto> SelectStockAddressLines { get; set; }
    }
}
