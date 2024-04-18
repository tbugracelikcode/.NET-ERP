using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.ProductPropertyLine.Dtos
{
    public class ListProductPropertyLinesParameterDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Ürün Özellik ID
        /// </summary>
        public Guid ProductPropertyID { get; set; }
    }
}
