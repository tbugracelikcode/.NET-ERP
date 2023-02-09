using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductReferanceNumber.Dtos
{
    public class ListProductReferanceNumbersParameterDto : FullAuditedEntityDto
    {
        public Guid? ProductId { get; set; }
    }
}
