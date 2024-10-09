using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine.Dtos
{
    public class ListCPRSetupCostLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// CPR ID
        /// </summary>
        public Guid CPRID { get; set; }
    }
}
