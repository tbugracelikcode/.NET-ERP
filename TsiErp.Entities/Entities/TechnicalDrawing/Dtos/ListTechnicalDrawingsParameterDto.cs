using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.TechnicalDrawing.Dtos
{
    public class ListTechnicalDrawingsParameterDto : FullAuditedEntity
    {
        public Guid? ProductId { get; set; }
    }
}
