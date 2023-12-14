using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos
{
    public class CreateEmployeeScoringsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Puantaj Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        /// <summary>
        /// Ay
        /// </summary>
        public int Month_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectEmployeeScoringLinesDto> SelectEmployeeScoringLines { get; set; }
    }
}
