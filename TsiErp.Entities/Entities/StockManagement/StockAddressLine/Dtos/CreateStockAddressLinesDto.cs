using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos
{
    public class CreateStockAddressLinesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Stok Adres ID
        /// </summary>
        public Guid StockAdressID { get; set; }

        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Stok Bölüm ID
        /// </summary>
        public Guid? StockSectionID { get; set; }

        /// <summary>
        /// Stok Raf ID
        /// </summary>
        public Guid? StockShelfID { get; set; }

        /// <summary>
        /// Stok Sütun ID
        /// </summary>
        public Guid? StockColumnID { get; set; }

        /// <summary>
        /// Stok No ID
        /// </summary>
        public Guid? StockNumberID { get; set; }
    }
}
