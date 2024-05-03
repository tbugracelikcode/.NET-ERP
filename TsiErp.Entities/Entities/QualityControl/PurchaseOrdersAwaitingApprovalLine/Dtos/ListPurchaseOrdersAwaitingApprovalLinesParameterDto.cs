using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos
{
    public class ListPurchaseOrdersAwaitingApprovalLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Onay Bekleyen Satın Alma Sipariş ID
        /// </summary>
        public Guid PurchaseOrdersAwaitingApprovalID { get; set; }
    }
}
