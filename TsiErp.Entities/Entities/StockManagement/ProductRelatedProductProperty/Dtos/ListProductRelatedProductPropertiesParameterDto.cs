using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos
{
    public class ListProductRelatedProductPropertiesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
    }
}
