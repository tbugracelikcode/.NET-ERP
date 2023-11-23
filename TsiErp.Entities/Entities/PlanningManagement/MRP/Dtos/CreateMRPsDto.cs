using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;

namespace TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos
{
    public class CreateMRPsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public string State_ { get; set; }
        /// <summary>
        /// Bakım MRP'den mi
        /// </summary>
        public bool IsMaintenanceMRP { get; set; }
        /// <summary>
        /// Bakım MRP ID
        /// </summary>
        public Guid? MaintenanceMRPID { get; set; }

        [NoDatabaseAction]
        public List<SelectMRPLinesDto> SelectMRPLines { get; set; }
    }
}
