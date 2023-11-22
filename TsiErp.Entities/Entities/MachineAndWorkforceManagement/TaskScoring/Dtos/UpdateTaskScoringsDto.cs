using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos
{
    public class UpdateTaskScoringsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid? SeniorityID { get; set; }
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Görev Yapıldı mı?
        /// </summary>
        public bool IsTaskDone { get; set; }
        /// <summary>
        /// Hatayı Fark Etti mi?
        /// </summary>
        public bool IsDetectFault { get; set; }
        /// <summary>
        /// Ayar Yapabilir mi?
        /// </summary>
        public bool IsAdjustment { get; set; }
        /// <summary>
        /// Geliştirecek Fikir Üretebilir mi?
        /// </summary>
        public bool IsDeveloperIdea { get; set; }
        /// <summary>
        /// Görev Paylaşımı Yapabilir mi?
        /// </summary>
        public bool IsTaskSharing { get; set; }
        /// <summary>
        /// Puan
        /// </summary>
        public int Score { get; set; }
    }
}
