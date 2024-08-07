using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos
{
    public class CreateProductGroupsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// GTIP
        /// </summary>
        public string GTIP { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
    }
}
