using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine.Dtos
{
    public class ListCPRManufacturingCostLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// CPR ID
        /// </summary>
        public Guid CPRID { get; set; }
    }
}
