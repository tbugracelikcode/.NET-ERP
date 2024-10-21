using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine.Dtos
{
    public class ListSalesInvoiceLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satış Fatura ID
        /// </summary>
        public Guid SalesInvoiceID { get; set; }
    }
}
