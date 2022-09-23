using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesProposition.Dtos
{
    public class ListSalesPropositionsParamaterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
