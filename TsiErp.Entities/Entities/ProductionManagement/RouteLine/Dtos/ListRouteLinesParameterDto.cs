using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos
{
    public class ListRouteLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
    }
}
