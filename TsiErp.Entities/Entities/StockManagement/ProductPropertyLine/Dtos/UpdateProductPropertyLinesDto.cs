using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.ProductPropertyLine.Dtos
{
    public class UpdateProductPropertyLinesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Ürün Özellik ID
        /// </summary>
        public Guid ProductPropertyID { get; set; }

        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }

        /// <summary>
        /// Ürün Grubu ID
        /// </summary>
        public Guid? ProductGroupID { get; set; }

        /// <summary>
        /// Satır Kodu
        /// </summary>
        public string LineCode { get; set; }

        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineName { get; set; }
    }
}
