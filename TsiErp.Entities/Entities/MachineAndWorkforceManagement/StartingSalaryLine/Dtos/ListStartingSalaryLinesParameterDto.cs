using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos
{
    public class ListStartingSalaryLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Başlangıç Maaş ID
        /// </summary>
        public Guid StartingSalaryID { get; set; }
    }
}
