using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Product.Dtos
{
    public class ListProductsParameterDto : FullAuditedEntityDto
    {
        public bool IsActive { get; set; } = true;
    }
}
