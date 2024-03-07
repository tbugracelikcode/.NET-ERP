using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.StockColumn.Dtos
{
    public class SelectStockColumnsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İsim
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
