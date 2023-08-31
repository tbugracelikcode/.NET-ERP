using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos
{
    public class ListOperationalSPCLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// SPC ID
        /// </summary>
        public Guid OperationalSPCID { get; set; }
    }
}
