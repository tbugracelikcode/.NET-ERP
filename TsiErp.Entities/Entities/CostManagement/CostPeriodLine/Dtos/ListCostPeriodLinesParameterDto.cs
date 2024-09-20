using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CostManagement.CostPeriodLine.Dtos
{
    public class ListCostPeriodLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Maliyet Periyodu ID
        /// </summary>
        public Guid CostPeriodID { get; set; }
    }
}
