using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos
{
    public class ListOperationalQualityPlanLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Operasyon Kalite Planı ID
        /// </summary>
        public Guid OperationalQualityPlanID { get; set; }
    }
}
