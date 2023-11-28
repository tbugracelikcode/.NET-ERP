using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos
{
    public class CreatePackingListPalletCubageLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid PackingListID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// En
        /// </summary>
        public decimal Width_ { get; set; }
        /// <summary>
        /// Boy
        /// </summary>
        public decimal Height_ { get; set; }
        /// <summary>
        /// Yük
        /// </summary>
        public decimal Load_ { get; set; }
        /// <summary>
        /// Kübaj
        /// </summary>
        public decimal Cubage { get; set; }
    }
}
