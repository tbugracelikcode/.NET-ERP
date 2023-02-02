using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlannedMaintenanceLine.Dtos
{
    public class ListPlannedMaintenanceLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Planlı Bakım ID
        /// </summary>
        public Guid PlannedMaintenanceID { get; set; }
    }
}
