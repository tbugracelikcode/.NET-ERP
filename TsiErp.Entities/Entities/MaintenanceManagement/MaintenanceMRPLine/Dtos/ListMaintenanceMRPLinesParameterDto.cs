using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos
{
    public class ListMaintenanceMRPLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Bakım MRP ID
        /// </summary>
        public Guid MaintenanceMRPID { get; set; }
    }
}
