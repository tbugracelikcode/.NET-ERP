using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos
{
    public class ListProductPropertiesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Ürün Grubu ID
        /// </summary>
        public Guid ProductGroupID { get; set; }

        /// <summary>
        /// Ürün Özellikleri Kodu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Ürün Özellikleri Açıklaması
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ürün Özellikleri Açıklaması
        /// </summary>
        public bool isChooseFromList { get; set; }
    }
}
