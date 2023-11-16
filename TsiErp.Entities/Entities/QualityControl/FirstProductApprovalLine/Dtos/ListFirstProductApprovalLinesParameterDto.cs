using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos
{
    public class ListFirstProductApprovalLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İlk Ürün Onay ID
        /// </summary>
        public Guid FirstProductApprovalID { get; set; }
    }
}
