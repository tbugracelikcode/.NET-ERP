using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos
{
    public class ListMaintenanceInstructionsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Bakım Talimat Adı
        /// </summary>
        public string InstructionName { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Periyot Adı
        /// </summary>
        public string PeriodName { get; set; }
        /// <summary>
        /// Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        /// <summary>
        /// Planlanan Bakım Süresi
        /// </summary>
        public decimal PlannedMaintenanceTime { get; set; }
        /// <summary>
        /// Not
        /// </summary>
        public string Note_ { get; set; }
    }
}
