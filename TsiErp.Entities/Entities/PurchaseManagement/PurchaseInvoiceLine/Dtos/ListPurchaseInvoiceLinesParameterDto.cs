using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos
{
    public class ListPurchaseInvoiceLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Fatura ID
        /// </summary>
        public Guid PurchaseInvoiceID { get; set; }
    }
}
