using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos
{
    public class ListMRPLinesParameterDto : FullAuditedEntityDto
    {

        /// <summary>
        /// MRP ID
        /// </summary>
        public Guid MRPID { get; set; }
    }
}
