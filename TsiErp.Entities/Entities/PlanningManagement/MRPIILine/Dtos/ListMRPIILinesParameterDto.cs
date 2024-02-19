using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos
{
    public class ListMRPIILinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// MRPII ID
        /// </summary>
        public Guid MRPIIID { get; set; }
    }
}
