using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.StockManagement.ProductPropertyLine.Dtos;

namespace TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos
{
    public class CreateProductPropertiesDto : FullAuditedEntityDto
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

        [NoDatabaseAction]
        public List<SelectProductPropertyLinesDto> SelectProductPropertyLines { get; set; }
    }
}
