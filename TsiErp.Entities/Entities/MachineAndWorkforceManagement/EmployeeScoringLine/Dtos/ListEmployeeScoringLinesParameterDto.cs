using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos
{
    public class ListEmployeeScoringLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Puantaj ID
        /// </summary>
        public Guid EmployeeScoringID { get; set; }
    }
}
