using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos
{
    public class SelectEmployeeAnnualSeniorityDifferencesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid? SeniorityID { get; set; }
        /// <summary>
        /// Kıdem Adı
        /// </summary>
        public string SeniorityName { get; set; }
        /// <summary>
        /// Fark
        /// </summary>
        public decimal Difference { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
    }
}
