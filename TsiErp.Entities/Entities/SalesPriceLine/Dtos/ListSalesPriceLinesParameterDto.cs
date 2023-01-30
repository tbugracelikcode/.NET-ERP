using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.SalesPriceLine.Dtos
{
    public class ListSalesPriceLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fiyat Listesi ID
        /// </summary>
        public Guid SalesPriceID { get; set; }
    }
}
