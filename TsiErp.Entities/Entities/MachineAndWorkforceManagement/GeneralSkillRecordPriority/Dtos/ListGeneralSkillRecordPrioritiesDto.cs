using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos
{
    public class ListGeneralSkillRecordPrioritiesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Genel Beceri ID
        /// </summary>
        public Guid? GeneralSkillID { get; set; }
        /// <summary>
        /// Genel Beceri Adı
        /// </summary>
        public string GeneralSkillName { get; set; }
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
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
