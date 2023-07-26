using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos
{
    public class ListTemplateOperationUnsuitabilityItemsParameterDto : FullAuditedEntityDto
    {
        ///<summary>
        ///Şablon Operasyon ID
        /// </summary
        public Guid TemplateOperationId { get; set; }
    }
}
