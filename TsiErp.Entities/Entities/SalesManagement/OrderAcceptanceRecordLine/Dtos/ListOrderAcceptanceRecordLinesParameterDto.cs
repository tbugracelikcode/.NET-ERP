using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos
{
    public class ListOrderAcceptanceRecordLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid OrderAcceptanceRecordID { get; set; }
    }
}
