using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MaintenanceInstructionLine.Dtos
{
    public class ListMaintenanceInstructionLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Bakım Talimat ID
        /// </summary>
        public Guid InstructionID { get; set; }
    }
}
