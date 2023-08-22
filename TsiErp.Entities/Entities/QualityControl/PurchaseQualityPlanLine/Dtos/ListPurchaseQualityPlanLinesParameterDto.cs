using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos
{
    public class ListPurchaseQualityPlanLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Kalite Planı ID
        /// </summary>
        public Guid PurchaseQualityPlanID { get; set; }
    }
}
