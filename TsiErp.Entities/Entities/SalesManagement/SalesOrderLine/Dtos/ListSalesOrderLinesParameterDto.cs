using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos
{
    public class ListSalesOrderLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
    }
}
