using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesPropositionLine.Dtos
{
    public class ListSalesPropositionLinesParamaterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satış Teklif ID
        /// </summary>
        public Guid SalesPropositionID { get; set; }
    }
}
