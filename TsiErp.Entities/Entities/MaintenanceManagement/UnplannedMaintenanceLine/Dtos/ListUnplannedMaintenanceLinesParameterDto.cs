using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine.Dtos
{
    public class ListUnplannedMaintenanceLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Plansız Bakım ID
        /// </summary>
        public Guid UnplannedMaintenanceID { get; set; }
    }
}
