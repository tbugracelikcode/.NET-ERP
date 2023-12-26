using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos
{
    public class SelectEmployeeScoringsDto : FullAuditedEntityDto
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
        /// Puantaj Durumu
        /// </summary>
        public EmployeeScoringsStateEnum ScoringState { get; set; }
        /// <summary>
        /// Puantaj Durumu Adı
        /// </summary>
        public string ScoringStateName { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectEmployeeScoringLinesDto> SelectEmployeeScoringLines { get; set; }
    }
}
