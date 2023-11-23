using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos
{
    public class UpdateEmployeeGeneralSkillRecordsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ad
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
