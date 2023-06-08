using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos
{
    public class ListPurchasePriceLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fiyat Listesi ID
        /// </summary>
        public Guid PurchasePriceID { get; set; }
    }
}
