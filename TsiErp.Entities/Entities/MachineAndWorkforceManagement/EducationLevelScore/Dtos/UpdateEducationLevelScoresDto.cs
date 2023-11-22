using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos
{
    public class UpdateEducationLevelScoresDto : FullAuditedEntityDto
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
        /// Puan
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
