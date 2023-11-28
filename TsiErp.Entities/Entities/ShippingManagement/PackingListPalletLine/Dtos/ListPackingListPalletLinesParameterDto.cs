using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos
{
    public class ListPackingListPalletLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid PackingListID { get; set; }
    }
}
