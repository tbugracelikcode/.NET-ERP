using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos
{
    public class ListShipmentPlanningLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Yükleme Planlama ID
        /// </summary>
        public Guid? ShipmentPlanningID { get; set; }
    }
}
