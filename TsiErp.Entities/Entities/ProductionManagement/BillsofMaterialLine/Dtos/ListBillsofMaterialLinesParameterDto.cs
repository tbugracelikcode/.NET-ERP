using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos
{
    public class ListBillsofMaterialLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Reçete ID
        /// </summary>
        public Guid BoMID { get; set; }
    }
}
