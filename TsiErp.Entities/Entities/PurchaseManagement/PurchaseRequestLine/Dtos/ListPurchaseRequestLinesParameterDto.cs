using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos
{
    public class ListPurchaseRequestLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Talep ID
        /// </summary>
        public Guid PurchaseRequestID { get; set; }
    }
}
